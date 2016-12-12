using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Collections.Generic;

namespace valeria2{
    class FileManager{ 
        private IMongoDatabase db; 

        public FileManager(IMongoDatabase DataBase){
            db = DataBase;
        }
        public bool CreateDirectory(ObjectId dir, string author, string dirName){
            var collection = db.GetCollection<BsonDocument>("filesystem");

            BsonDocument doc = new BsonDocument
                {
                    {"id", ObjectId.GenerateNewId() },
                    {"Directory", dir},
                    {"Author", author}, 
                    {"DateCreated", System.DateTime.Now}, 
                    {"DateUpdated", System.DateTime.Now}, 
                    {"Name", dirName}, 
                    {"ObjectType", objectItem.ObType.directory}
                };

                collection.InsertOne(doc); 

                return true;
        } 

        public content CreateContent(ObjectId dir, string author, string contentName, string content){
            var collection = db.GetCollection<BsonDocument>("filesystem");

            var id = ObjectId.GenerateNewId();

            BsonDocument doc = new BsonDocument
                {
                    {"id", id},
                    {"Directory", dir},
                    {"Author", author}, 
                    {"DateCreated", System.DateTime.Now}, 
                    {"DateUpdated", System.DateTime.Now}, 
                    {"Name", contentName}, 
                    {"ContentData", content}, 
                    {"ObjectType", objectItem.ObType.content}
                };

                collection.InsertOne(doc); 

                return new content{
                    id = id,
                    Directory = dir, 
                    ContentData = content, 
                    Author = author
                };
        }

        public bool createOrUpdate(ObjectId _object, ObjectId dir, string author, string contentName, string _content){
            var collection = db.GetCollection<BsonDocument>("filesystem"); 
            var filter = Builders<BsonDocument>.Filter.Eq("Id", _object); 

            var result = collection.Find(filter).SingleOrDefault();

            if (result == null){
                CreateContent(dir, author, contentName, _content); 
            }else{
                BsonDocument update = new BsonDocument{
                    {"content", _content}
                }; 

                collection.UpdateOne(filter, update);
            }

            return true;


        }

        public List<content> GetContent(ObjectId directory, string Author){
            var collection = db.GetCollection<BsonDocument>("filesystem"); 
            var filter = Builders<BsonDocument>.Filter.Eq("Directory", directory); 

            var result = collection.Find(filter).ToList();

            List<content> contentList = new List<content>();

            foreach (BsonDocument item in result)
            {
                contentList.Add(new content
                {
                    id = ObjectId.Parse(item.GetValue("Id").ToString()),
                    Directory = ObjectId.Parse(item.GetValue("Directory").ToString()), 
                    Author = item.GetValue("Author").ToString(), 
                    ContentData = item.GetValue("Content").ToString()
                });
            }

            return contentList;

        } 

        public content retriveContent(string ob){
            var collection = db.GetCollection<BsonDocument>("filesystem"); 
            var filter = Builders<BsonDocument>.Filter.Eq("id", ObjectId.Parse(ob)); 

            var result = collection.Find(filter).SingleOrDefault();

            return new content{
                id = (ObjectId)result.GetValue("id"),
                ContentData = result.GetValue("ContentData").ToString(), 
                Author = result.GetValue("Author").ToString()
            };


        }
        
    }
}
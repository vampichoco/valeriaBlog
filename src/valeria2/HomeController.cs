using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace valeria2
{
    public class HomeController
    {
        // GET: /<controller>/

        public static IMongoClient _client;
        public static IMongoDatabase _dataBase;
        public static Dictionary<string, Action<HttpContext>> controllers;

        

        public static void blog(HttpContext ctx)
        {
            _client = new MongoClient();
            _dataBase = _client.GetDatabase("test");

            var collection = _dataBase.GetCollection<BsonDocument>("vdb1");
            var filter = new BsonDocument();
            var posts = new List<post>();

            BsonDocument sort = new BsonDocument
            {
                {"date", -1 }
            };

            var result = collection.Find(filter).Sort(sort).ToList();

            foreach (BsonDocument item in result)
            {
                posts.Add(new post
                {
                    text = item.GetValue("text").ToString(),
                    title = item.GetValue("title").ToString()
                });
            }

            ctx.Response.WriteAsync(JsonConvert.SerializeObject(posts));

        } 

        public static void upload(HttpContext ctx)
        {

            string ipString = StreamToString(ctx.Request.Body);

            if (ipString != "")
            {
                _client = new MongoClient();
                _dataBase = _client.GetDatabase("test");

                var collection = _dataBase.GetCollection<BsonDocument>("vdb1");

                JObject j = JObject.Parse(ipString);

                BsonDocument doc = new BsonDocument
                {
                    {"title", j["title"].ToString() },
                    {"text" , j["text"].ToString()  },
                    {"date" , DateTime.Now          }
                };

                collection.InsertOne(doc);

                var res = new { Message = "Entrada creada :D" };
                ctx.Response.WriteAsync(JsonConvert.SerializeObject(res));
            }
            
        }

        public static void createUser(string nickname, string email, string password){
            string hash = "";
            using(var sha256 = SHA256.Create()){
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password)); 
                hash = Convert.ToBase64String(bytes); 
            }

            _client = new MongoClient();
            _dataBase = _client.GetDatabase("test");

            var collection = _dataBase.GetCollection<BsonDocument>("users");


            ObjectId homeDir = ObjectId.GenerateNewId();

            BsonDocument doc = new BsonDocument
                {
                    {"user", nickname },
                    {"email" ,email },
                    {"password" , hash }, 
                    {"homeDir", homeDir}
                };

                collection.InsertOne(doc);

        }

        public static void createContent(HttpContext ctx){

            string ipString = StreamToString(ctx.Request.Body); 

            if (ipString != ""){
                _client = new MongoClient();
            _dataBase = _client.GetDatabase("test");

            var collection = _dataBase.GetCollection<BsonDocument>("filesystem");
                JObject j = JObject.Parse(ipString);

                FileManager fm = new FileManager(_dataBase);

                content c = fm.CreateContent(ObjectId.Empty, "Eskimo", j["name"].ToString(), j["content"].ToString());

                var contentResult = new {
                    id = c.id.ToString(),
                    Directory = c.Directory.ToString(), 
                    ContentData = c.ContentData.ToString(), 
                    Author = c.Author
                };

                ctx.Response.WriteAsync(JsonConvert.SerializeObject(contentResult));

            }
        }

        public static void retrive(HttpContext ctx){
            if (ctx.Request.Query != null){
                var id = ctx.Request.Query["id"];
                _client = new MongoClient();
                _dataBase = _client.GetDatabase("test");

                var collection = _dataBase.GetCollection<BsonDocument>("filesystem");

                FileManager fm = new FileManager(_dataBase); 
                var content = fm.retriveContent(id); 

                var res = new {
                    id = content.id.ToString(), 
                    content = content.ContentData, 
                    author = content.Author
                };

                ctx.Response.WriteAsync(JsonConvert.SerializeObject(res));
                
            }


        }

        public static void createUser(HttpContext ctx){
            string ipString = StreamToString(ctx.Request.Body); 

            if (ipString != ""){
                JObject j = JObject.Parse(ipString); 

                string nickname = j["nickname"].ToString();
                string email = j["email"].ToString();
                string password = j["password"].ToString();

                createUser(nickname, email, password);

            }
        }

        public static void Valeria()
        {
            controllers = new Dictionary<string, Action<HttpContext>>();

            controllers.Add("/sandra" , (ctx) => ctx.Response.WriteAsync("This is a controller"             ));
            controllers.Add("/"       , (ctx) => ctx.Response.WriteAsync("This is the home controller"      ));
            controllers.Add("/valeria", (ctx) => ctx.Response.WriteAsync("This is the 'valeria' controller" ));
            controllers.Add("/blog"   , blog                                                                 );
            controllers.Add("/upload" , upload                                                               );
            controllers.Add("/createuser", createUser);
            controllers.Add("/createcontent", createContent);
            controllers.Add("/content", retrive);


        }

        public static async Task proccess(HttpContext ctx)
        {
            string path = ctx.Request.Path;
            Console.WriteLine("====" + path);

            await Task.Run(() => {
                controllers[path].Invoke(ctx); }
            );
        }

        public static string StreamToString(Stream stream)
        {
            //stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

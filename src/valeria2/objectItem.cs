using MongoDB;
using MongoDB.Bson;

namespace valeria2{
    class objectItem{
        ObjectId _id; 
        string name; 
        ObjectId directory;
        string author; 
        System.DateTime dateCreated; 
        System.DateTime dateUpdated; 

        public ObjectId id{
            get{return _id;}
            set {_id = value;}
        } 

        public string Name{
            get{return name;}
            set{name = value;}
        } 

        public ObjectId Directory{
            get{return directory;}
            set {directory = value;}
        } 

        public string Author{
            get{return author;}
            set{author = value;}
        } 

        public System.DateTime DateCreated{
            get{return dateCreated;}
            set{dateCreated = value;}
        } 

        public System.DateTime DateUpdated{
            get{return dateUpdated;}
            set{dateUpdated = value;}
        } 

        ObType objectType; 

        public ObType ObjectType{
            get{return objectType;}
            set{objectType = value;}
        }

        public enum ObType{
            content = 0, 
            link = 1, 
            directory = 2
        }

    }
}
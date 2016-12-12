using MongoDB;
using MongoDB.Bson;

namespace valeria2 {
    class link : objectItem{
        
         string href; 

         public string Href{
             get{return href;}
             set{href = value;}
         }
        

    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Bson;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace valeria2
{
    public class HomeController
    {
        // GET: /<controller>/

        public static IMongoClient _client;
        public static IMongoDatabase _dataBase;

        public static async Task hello(HttpContext ctx)
        {
            //string name;
            //if (ctx.Request.Query.ContainsKey("name") == false)
            //{
            //    name = "World";
            //}else
            //{
            //    name = ctx.Request.Query["name"];
            //}
            
            //await ctx.Response.WriteAsync(string.Format("hello, {0}", name));

            _client = new MongoClient();
            _dataBase = _client.GetDatabase("test");

            var collection = _dataBase.GetCollection<BsonDocument>("vdb");
            var filter = new BsonDocument();

            var result = await collection.Find(filter).ToListAsync(); 

            foreach (BsonDocument item in result)
            {
                string op = string.Format("<strong>title: {0}</strong><br /> Text: {1}<br />", item.GetValue("title").ToString(), item.GetValue("text").ToString());
                await ctx.Response.WriteAsync(op);
            }
            


        }
    }
}

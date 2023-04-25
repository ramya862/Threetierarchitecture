using System;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCartList;
using Microsoft.Azure.Cosmos;
using ShoppingCartList.Models;

namespace cosmosdata
{
    public class dal{
      private const string DatabaseName = "ShoppingCartItems";
        private const string CollectionName = "Items";
        private readonly CosmosClient _cosmosClient;
        private static Container documentContainer;
       
        public dal(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            documentContainer = _cosmosClient.GetContainer("ShoppingCartItems", "Items");
        }

      
         public static async Task<dynamic> getallitems(HttpRequestMessage req,Object gmydata)
         {
           string json = Newtonsoft.Json.JsonConvert.SerializeObject(gmydata);

           return gmydata;
         }
         public static async Task<dynamic> gettitembyid(string id,string category)
         {
         
                var item =await documentContainer.ReadItemAsync<ShoppingCartList.Models.ShoppingCartItem>(id, new Microsoft.Azure.Cosmos.PartitionKey(category));
                return item;
         }
         public static async Task<dynamic> Createitem(object item,string id,string category)
         {
            await documentContainer.CreateItemAsync(item, new Microsoft.Azure.Cosmos.PartitionKey(category));
            string responsemessage="Created an item successfully";
            dynamic cmydata = new ExpandoObject();
            cmydata.message = responsemessage;
            cmydata.Data=item; 
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(cmydata);
            return new OkObjectResult(json);
         }
         public static async Task<dynamic>updateitem(HttpRequest req,string id,string category)
         {
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UpdateShoppingCartItem>(requestData);
            var item=await documentContainer.ReadItemAsync<ShoppingCartItem>(id, new Microsoft.Azure.Cosmos.PartitionKey(category));

            if (item.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string responseMessage="There is no item with the mentioned id";
                return new NotFoundObjectResult(responseMessage);
            }

            item.Resource.Collected = data.Collected;
            await documentContainer.UpsertItemAsync(item.Resource);
            return new OkObjectResult(item);

      
         }
         public static async Task<dynamic>Deleteitem(string id,string category)
         {
            await documentContainer.DeleteItemAsync<ShoppingCartItem>(id, new Microsoft.Azure.Cosmos.PartitionKey(category));
           string message="Deleted successfully";
            return new OkObjectResult(message);
         }
    }
}

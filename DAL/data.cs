using System;
using ShoppingCartList;
namespace cosmosdata
{
   public interface mydb
   {
   Task<dynamic> getallitems();
   Task<dynamic> gettitembyid(string id,string category);
   Task<dynamic> Createitem(string id,string category);
   Task<dynamic>updateitem(string id,string category);
   Task<dynamic>Deleteitem(string id,string category);
   }    
   public class data:mydb{
        private const string DatabaseName = "ShoppingCartItems";
        private const string CollectionName = "Items";
        private readonly CosmosClient _cosmosClient;
        private Container documentContainer;
       
        public ShoppingCartApi(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            documentContainer = _cosmosClient.GetContainer("ShoppingCartItems", "Items");
        }
         public static async Task<dynamic> getallitems()
         {
          dynamic gmydata = new ExpandoObject();
           gmydata.message = gmessage;
           gmydata.Data=shp;
           return gmydata;
         }
         public static async Task<dynamic> gettitembyid(string id,string category)
         {
         
                var item =await documentContainer.ReadItemAsync<ShoppingCartItem>(id, new Microsoft.Azure.Cosmos.PartitionKey(category));
                return item;
         }
         public static async Task<dynamic> Createitem(string id,string category)
         {

            return await documentContainer.CreateItemAsync(item, new Microsoft.Azure.Cosmos.PartitionKey(item.Category));           
         }
         public static async Task<dynamic>updateitem(string id,string category)
         {
            string requestData =await new StreamReader(req.Body).ReadToEndAsync();
            return requestedData;
         }
         public static async Task<dynamic>Deleteitem(string id,string category)
         {
            return await documentContainer.DeleteItemAsync<ShoppingCartItem>(id, new Microsoft.Azure.Cosmos.PartitionKey(category));
            
         }
    }
}

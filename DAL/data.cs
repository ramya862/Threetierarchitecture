using System;
using ShoppingCartList;
namespace cosmosdata
{
    public class data{
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
         public static async Task<dynamic> gettitembyid()
         {
         
                var item =await documentContainer.ReadItemAsync<ShoppingCartItem>(id, new Microsoft.Azure.Cosmos.PartitionKey(category));
                return item;
         }
         public static async Task<dynamic> Createitem()
         {

            return await documentContainer.CreateItemAsync(item, new Microsoft.Azure.Cosmos.PartitionKey(item.Category));           
         }
         public static async Task<dynamic>updateitem()
         {
            string requestData =await new StreamReader(req.Body).ReadToEndAsync();
            return requestedData;
         }
         public static async Task<dynamic>Deleteitem(IDocumentClient client, string id,string category)
         {
            return await documentContainer.DeleteItemAsync<ShoppingCartItem>(id, new Microsoft.Azure.Cosmos.PartitionKey(category));
            
         }
    }
}

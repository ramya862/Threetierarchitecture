using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingCartList.Models;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using cosmosdata;
namespace ShoppingCartList
{
    public class ShoppingCartApi
    {
        public ShoppingCartApi(mydb db)
        {
          _db=db;
        }
    [FunctionName("Getallshoppingcartitems")]
    public static async IActionResult Getallemps(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getallshoppingcartitems")] HttpRequest req,
        [CosmosDB(
            DatabaseName,
                CollectionName,
                Connection ="CosmosDBConnectionString",
                SqlQuery = "SELECT * FROM c")]
               System.Collections.Generic.IEnumerable<ShoppingCartItem> shp,
        ILogger log)
    {
        
        log.LogInformation("Getting list of all employees ");
        await _db.getallitems();
        string gmessage="Retrieved all items successfully";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(gmydata);

        return new OkObjectResult(json);
    }


    [FunctionName("GetShoppingCartItemById")]
        public async Task<IActionResult> GetShoppingCartItemById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getshoppingcartitembyid/{id}/{category}")]
             HttpRequest req, ILogger log,string id,string category)
                   
        {
            log.LogInformation($"Getting Shopping Cart Item with ID: {id}");
            try
            {
                await _db.gettitembyid();
                string getmessage="Retrived  an item successfully by Id";
                dynamic gmydata = new ExpandoObject();
                gmydata.message = getmessage;
                gmydata.Data=item.Resource;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(gmydata);
                return new OkObjectResult(json);
            }
            catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string responseMessage="Invalid input params,Please check";
                return new NotFoundObjectResult(responseMessage);
            }
        }
        

    [FunctionName("CreateShoppingCartItem")]
        public async Task<IActionResult> CreateShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "createshoppingcartitem")] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("Creating Shopping Cart Item");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateShoppingCartItem>(requestData);

            var item = new ShoppingCartItem
            {
                ItemName = data.ItemName,
                 Category = data.Category
            };
            await _db.Createitem();
            string responsemessage="Created an item successfully";
            dynamic cmydata = new ExpandoObject();
            cmydata.message = responsemessage;
            cmydata.Data=item;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(cmydata);
            return new OkObjectResult(json);
        }

        [FunctionName("UpdateShoppingCartItem")]
        public async Task<IActionResult> UpdateShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "updateshoppingcartitem/{id}/{category}")] HttpRequest req,
            ILogger log, string id,string category)
        {
            log.LogInformation($"Updating Shopping Cart Item with ID: {id}");
            await _db.updateitem();
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();

            var data = JsonConvert.DeserializeObject<UpdateShoppingCartItem>(requestData);
            await _db.updateitem();

            if (item.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string responseMessage="There is no item with the mentioned id";
                return new NotFoundObjectResult(responseMessage);
            }

            item.Resource.Collected = data.Collected;
            await documentContainer.UpsertItemAsync(item.Resource);
            string updatemessage="Updated successfully";
            dynamic upmydata = new ExpandoObject();
            upmydata.message = updatemessage;
            upmydata.Data=item.Resource;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(upmydata);
            return new OkObjectResult(json);
        }

        [FunctionName("DeleteShoppingCartItem")]
        public async Task<IActionResult> DeleteShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delshoppingcartitem/{id}/{category}")] HttpRequest req,
            ILogger log, string id,string category)
        {
            log.LogInformation($"Deleting Shopping Cart Item with ID: {id}");
             await _db.Deleteitem();
            string responseMessage="Deleted sucessfully";         
            return new OkObjectResult(responseMessage);
        }
    }
}
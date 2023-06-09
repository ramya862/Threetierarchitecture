using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingCartList.Models;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using cosmosdata;
using System.Net.Http;
using System;

namespace Domain.logic
{
    public class Mylogic
{    public static async Task<ObjectResult>Getaitems(HttpRequestMessage req,ILogger log,object shp)
    {
        string gmessage="Retrieved all items successfully";
        dynamic gmydata = new ExpandoObject();
        gmydata.message = gmessage;
        gmydata.Data=shp;
        return await dal.getallitems(req,gmydata);
    }
    public static async Task<IActionResult> GetItemById(HttpRequestMessage req, ILogger log, string id, string category)
{
    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(category))
    {
        return new BadRequestObjectResult("Please provide an ID and category.");
    }

    try
    {
        var item = await dal.gettitembyid(id, category, log);
        if (item == null)
        {
            return new NotFoundObjectResult($"Item with ID {id} and category {category} not found.");
        }

        string message = "Retrieved an item successfully by ID.";
        dynamic mydata = new ExpandoObject();
        mydata.message = message;
        mydata.data = item;
        string json = JsonConvert.SerializeObject(mydata);
        return new OkObjectResult(json);
    }
    catch (Exception ex)
    {
        log.LogError(ex, "Error retrieving item by ID.");
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}

       public static async Task<ObjectResult>Createitem(HttpRequest req,ILogger log,string id,string category)
    {
      log.LogInformation("Creating Shopping Cart Item");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateShoppingCartItem>(requestData);

            var item = new ShoppingCartItem
            {
                ItemName = data.ItemName,
                Category = data.Category
            };

            return await dal.Createitem(item,id,category);
    }
    public static async Task<ObjectResult>Updateitem(HttpRequest req,ILogger log,string id,string category)
    {
       log.LogInformation($"Updating Shopping Cart Item with ID: {id}");

            var item=await dal.updateitem(req,id,category);
            string updatemessage="Updated successfully";
            dynamic upmydata = new ExpandoObject();
            upmydata.message = updatemessage;
            upmydata.Data=item.Resource;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(upmydata);
            return new OkObjectResult(json);
    }
    public static async Task<ObjectResult>DeleteItem(HttpRequestMessage req,ILogger log,string id ,string category)
    {
            log.LogInformation($"Deleting Shopping Cart Item with ID: {id}");

            return await dal.Deleteitem(id,category);
    }
}
}
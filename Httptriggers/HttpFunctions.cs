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
using Domain.logic;
using System;
using System.Net.Http;

namespace ShoppingCartLists
{
    public class ShoppingCartApi
    {
        private const string DatabaseName = "ShoppingCartItems";
        private const string CollectionName = "Items";
        private readonly CosmosClient _cosmosClient;
        private Container documentContainer;
       
        public ShoppingCartApi(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            documentContainer = _cosmosClient.GetContainer("ShoppingCartItems", "Items");
        }
    [FunctionName("Getallshoppingcartitems")]
    public static async Task<IActionResult> GetallempsAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getallshoppingcartitems")] HttpRequestMessage req,
        [CosmosDB(
            DatabaseName,
                CollectionName,
                Connection ="CosmosDBConnectionString",
                SqlQuery = "SELECT * FROM c")]
               System.Collections.Generic.IEnumerable<ShoppingCartItem> shp,
        ILogger log)
    {
            try
            {
                return await Mylogic.Getaitems(req, log,shp);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex.Message);
            }
    }


    [FunctionName("GetShoppingCartItemById")]
        public async Task<IActionResult> GetShoppingCartItemById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getshoppingcartitembyid/{id}/{category}")]
             HttpRequestMessage req, ILogger log,string id,string category)
                   
        {
            log.LogInformation($"Getting Shopping Cart Item with ID: {id}");
            try
            {
                return await Mylogic.GetitembyId(req,log,id,category);
            }
            catch (CosmosException ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
        }
        

    [FunctionName("CreateShoppingCartItem")]
        public async Task<IActionResult> CreateShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "createshoppingcartitem")] HttpRequest req,
           ILogger log,string id,string category)
        {
           try{
            return await Mylogic.Createitem(req,log,id,category);

           }
           catch(Exception ex)
           {
             return new NotFoundObjectResult(ex.Message);
           }
        }

        [FunctionName("UpdateShoppingCartItem")]
        public async Task<IActionResult> UpdateShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "updateshoppingcartitem/{id}/{category}")] HttpRequest req,
            ILogger log, string id,string category)
        {
            try{
                return await Mylogic.Updateitem(req,log,id,category);

            }
            catch(Exception ex)
            {
                return new NotFoundObjectResult(ex.Message);

            }
        }

        [FunctionName("DeleteShoppingCartItem")]
        public async Task<IActionResult> DeleteShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delshoppingcartitem/{id}/{category}")] HttpRequestMessage req,
            ILogger log, string id,string category)
        {
            try{
                return await Mylogic.DeleteItem(req,log,id,category);
                
            }
            catch(Exception ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
        }
    }
}
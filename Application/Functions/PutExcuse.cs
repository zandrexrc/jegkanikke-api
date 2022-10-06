using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Application.Domain.Common;
using Application.Domain.Models;

namespace Application.Functions
{
    public static class PutExcuse
    {
        [FunctionName("PutExcuse")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "excuses/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.CosmosDbDatabaseName,
                collectionName: Constants.CosmosDbCollectionName,
                Id = "{id}",
                PartitionKey = "{id}",
                ConnectionStringSetting = Constants.CosmosDbConnectionStringSetting
                )] Document document,
            [CosmosDB(
                databaseName: Constants.CosmosDbDatabaseName,
                collectionName: Constants.CosmosDbCollectionName,
                ConnectionStringSetting = Constants.CosmosDbConnectionStringSetting
                )] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("PutExcuse function processed a request.");

            if (document == null || string.IsNullOrEmpty(document.Id))
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data == null || data.message == null)
            {
                return new BadRequestResult();
            }
            else
            {
                document.SetPropertyValue("message", data.message);
                Document res = await client.ReplaceDocumentAsync(document.SelfLink, document);
                Excuse updatedExcuse = new() { Id = res.Id, Message = res.GetPropertyValue<string>("message") };

                return new OkObjectResult(updatedExcuse);
            }
        }
    }
}


using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Application.Domain.Common;

namespace Application.Functions
{
    public static class DeleteExcuse
    {
        [FunctionName("DeleteExcuse")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "excuses/{id}")] HttpRequest req,
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
            log.LogInformation("DeleteExcuse function processed a request.");

            if (document == null || string.IsNullOrEmpty(document.Id))
            {
                return new NotFoundResult();
            }

            await client.DeleteDocumentAsync(
                document.SelfLink,
                new RequestOptions()
                {
                    PartitionKey = new PartitionKey(document.Id)
                }
            );

            return new NoContentResult();
        }
    }
}


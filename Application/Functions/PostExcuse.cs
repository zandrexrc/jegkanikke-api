using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Application.Domain.Models;
using Application.Domain.Common;

namespace Application.Functions
{
    public static class PostExcuse
    {
        [FunctionName("PostExcuse")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "excuses")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.CosmosDbDatabaseName,
                collectionName: Constants.CosmosDbCollectionName,
                ConnectionStringSetting = Constants.CosmosDbConnectionStringSetting
                )] IAsyncCollector<dynamic> excusesOut,
            ILogger log)
        {
            log.LogInformation("PostExcuse function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data == null || data.message == null)
            {
                return new BadRequestResult();
            }
            else
            {
                await excusesOut.AddAsync(new
                {
                    message = data.message
                });
            }

            return new NoContentResult();
        }
    }
}


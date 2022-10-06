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

namespace Application
{
    public static class GetExcuse
    {
        [FunctionName("GetExcuse")]
        public static ActionResult<Excuse> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "excuses/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.CosmosDbDatabaseName,
                collectionName: Constants.CosmosDbCollectionName,
                ConnectionStringSetting = Constants.CosmosDbConnectionStringSetting,
                SqlQuery = "SELECT TOP 1 * FROM c WHERE c.id = {id}"
                )] IEnumerable<Excuse> excuses,
            ILogger log)
        {
            log.LogInformation("GetExcuse function processed a request.");
            List<Excuse> excusesList = excuses.ToList();

            if (excusesList.Count < 1)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(excusesList[0]);
        }
    }
}


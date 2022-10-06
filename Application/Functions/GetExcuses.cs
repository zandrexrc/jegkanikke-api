using System;
using System.Linq;
using System.IO;
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
    public static class GetExcuses
    {
        [FunctionName("GetExcuses")]
        public static ActionResult<List<Excuse>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "excuses")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.CosmosDbDatabaseName, 
                collectionName: Constants.CosmosDbCollectionName,
                ConnectionStringSetting = Constants.CosmosDbConnectionStringSetting,
                SqlQuery = "SELECT * FROM c order by c._ts desc"
                )] IEnumerable<Excuse> excuses,
            ILogger log)
        {
            log.LogInformation("GetExcuses function processed a request.");
            List<Excuse> excusesList = excuses.ToList();

            if (excusesList.Count == 0)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(excusesList);
        }
    }
}


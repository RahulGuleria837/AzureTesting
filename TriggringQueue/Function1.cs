using System;
using System.Net.Http;
using System.Text;
using AzureCrud.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TriggringQueue
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([QueueTrigger("newqueuecheck", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            //var fileData = myQueueItem;
            var finalData = JsonConvert.DeserializeObject<FileData>(myQueueItem);
            SendingQueueDataToTable(finalData);
        }

        private static bool SendingQueueDataToTable(FileData queueData)
        {
            // now we will call owr web api to send the data to the table.
            HttpClient httpClient = new HttpClient();
            using var content = new StringContent(JsonConvert.SerializeObject(queueData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(Environment.GetEnvironmentVariable("url"), content).Result;
            var dataStatus = response.RequestMessage;


            return true;
        }

    }
}

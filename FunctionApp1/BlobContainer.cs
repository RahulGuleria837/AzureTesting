using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure;
using System.Web.Http;
using FunctionApp1;
using Azure.Storage.Queues;
using System.Text;

namespace AzureFunctionBlob
{
    public static class BlobHttpTriggerFunc
    {
        [FunctionName("BlobContainer")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {

            // Get the connection string and container name from environment variables
            string connectionstring = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");

            // Create or check if the container already exists

            var createOrCheckContainer = await CreateContainerIfNotExists(log, containerName, connectionstring);
            if (createOrCheckContainer == null)
            {
                return new InternalServerErrorResult();
            }



            // HERE WE ARE SENDING OUR FILE TO CONTAINER
            // Get the file from the HTTP request
          
            var file = req.Form.Files[0];

            // Open a stream to read the file data
            Stream abc =  file.OpenReadStream();

            // Create a new BlobContainerClient using the connection string and container name

            var blobClient = new BlobContainerClient(connectionstring, containerName);
            // Get a reference to the blob using the file name
            var blob = blobClient.GetBlobClient(file.FileName);
            // Upload the file to the blob
            await blob.UploadAsync(abc);

            //Creating queue
            var SendToQueue = CreateQueue(file.FileName, connectionstring, Environment.GetEnvironmentVariable("QueueName"));

            // Return a result indicating that the container was created or already exists
            return new ObjectResult(createOrCheckContainer);
        }

        // Create a container if it doesn't exist
        private static async Task<dynamic> CreateContainerIfNotExists(ILogger logger, string containerName, string connectionstring)
        {

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionstring);

            try
            {
                // first we will check the container that we create is available or not.
                var getAlreadyCreateContainer = blobServiceClient.GetBlobContainerClient(containerName);
                bool containerExist = await getAlreadyCreateContainer.ExistsAsync();
                if (containerExist)
                {
                    return "Container already exist";
                }


                // here we will create container if the specified name container not available.
                var createContainer = await blobServiceClient.CreateBlobContainerAsync(containerName);
                return createContainer.Value.Name;

            }
            catch (Exception ex)
            {
                // Log any exceptions
                logger.LogInformation(ex.ToString());
                return null;
            }




           
            }

        // Create the messagequeue
        private static async Task<bool> CreateQueue(string fileDetail, string connectionstring, string QueueName)
        {
            // here we will split the string in two parts.
            string[] strname = fileDetail.Split('.');

            // here we will create the information that will be store in the queue.
            var fileData = new FileData()
            {
                FileExtension = strname[1],
                FileName = strname[0],
                FileCreated = DateTime.Now.ToUniversalTime()
            };

            QueueServiceClient queueServiceClient = new QueueServiceClient(connectionstring);
            try
            {
                // here we will add a message to the queue.
                QueueClient queueClient = new QueueClient(connectionstring, QueueName);
                queueClient.CreateIfNotExists();
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fileData).ToString());
                await queueClient.SendMessageAsync(Convert.ToBase64String(bytes));
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }




    }


﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace SynchronousIO.WebRole.Controllers
{
    public class AsyncUploadController : ApiController
    {
        [HttpGet]
        public async Task UploadFileAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("uploadedfiles");
            
            await container.CreateIfNotExistsAsync();

            var blockBlob = container.GetBlockBlobReference("myblob");

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = File.OpenRead(HostingEnvironment.MapPath("~/FileToUpload.txt")))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }
    }
}

using Azure.Storage.Blobs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Text;

namespace SimpleAppUsingAzureAd.Controllers
{
    [ApiController]
    [Route("/inspect")]
    [AllowAnonymous]
    public class Inspect : Controller
    {
        [Route("resolvehost")]
        public async Task<string> ResolveHostNameAsync([FromQuery] string hostName)
        {
            IPAddress[] addresses = await Dns.GetHostAddressesAsync(hostName);

            return string.Join(
                "\r\n",
                addresses.Select(address => address.ToString()));
        }

        [Route("showcontainers")]
        public async Task<string> ShowContainersAsync([FromServices] IConfiguration config)
        {
            // Set locally using:
            //  dotnet user-secrets set "Storage:ConnectionString" "<connection string>"
            string connectionString = config["Storage:ConnectionString"]!;
            BlobServiceClient blobServiceClient = new(connectionString);

            StringBuilder result = new();
            await foreach(Azure.Storage.Blobs.Models.BlobContainerItem container in blobServiceClient.GetBlobContainersAsync())
            {
                result.AppendLine(container.Name);
            }

            return result.Length == 0 ? "No containers" : result.ToString();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using TestApp.Services;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KrpController : ControllerBase
    {
        /// <summary>
        /// Get all JSON items
        /// </summary>
        /// <returns>Get all JSON items</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "utf-8");

            var json = webClient.DownloadString(@"C:\Users\Coding\source\repos\TestProject\TestProject.API\wwwroot\items.json");
            var items = JsonConvert.DeserializeObject<KRPService>(json);

            return Ok(items);
        }
    }
}

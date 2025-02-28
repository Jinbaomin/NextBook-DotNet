using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace WebApplication1.Controllers
{
    public class HelloWorldController : Controller
    {
        // HTTP GET
        public IActionResult Index()
        {
            return View();
        }

        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }
    }
}

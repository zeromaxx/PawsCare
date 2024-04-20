using Microsoft.AspNetCore.Mvc;

namespace PawsCare.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/Status/{statusCode}")]
        public IActionResult Status(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }

            return View("Error");
        }
    }
}

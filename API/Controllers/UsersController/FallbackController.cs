using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UsersController;

public class FallbackController : Controller
{
    public ActionResult Index()
    {
        return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dating_app", "index.html"),
            "text/HTML");
    }
}
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UsersController
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController: ControllerBase
    {
        //public BaseApiController(){}
    }
}

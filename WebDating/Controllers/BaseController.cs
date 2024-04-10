using Microsoft.AspNetCore.Mvc;
using WebDating.Helpers;

namespace WebDating.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] //Áp dụng filter LogUserActivity cho tất cả các controllers kế thừa từ BaseApiController.
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}

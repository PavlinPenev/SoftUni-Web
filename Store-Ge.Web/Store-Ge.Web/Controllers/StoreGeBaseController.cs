using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Store_Ge.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class StoreGeBaseController : ControllerBase
    {
    }
}

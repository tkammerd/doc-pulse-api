using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doc.Pulse.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class BaseApiController : Controller
    {

        protected readonly IMediator _mediator;

        public BaseApiController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }
    }
}

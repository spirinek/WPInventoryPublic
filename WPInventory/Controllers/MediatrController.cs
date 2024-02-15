using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WPInventory.Controllers
{
    public class MediatorController : Controller
    {
        protected IMediator _mediator;
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetRequiredService<IMediator>());
    }
}

using Backend.Controllers;
using Backend.Models;
using Backend.Models.Form;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[Action]")]
[Produces("application/json")]
public class FormController : ControllerBase
{
    /// <summary>
    /// Get the Form Infomation
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpGet]
    public Response Get(Guid Id)
    {
        return new Response(false);
    }

    /// <summary>
    /// Create a new Form
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Response Create(CreateReq request)
    {
        return new Response(false);
    }
}
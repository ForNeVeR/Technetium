using Microsoft.AspNetCore.Mvc;
using Technetium.Data;

namespace Technetium.Web.Controllers;

[Microsoft.AspNetCore.Components.Route("/api/task")]
public class TaskController(TechnetiumDataContext db) : Controller
{
    [HttpPut]
    public void Import(IFormFile file)
    {
    }
}

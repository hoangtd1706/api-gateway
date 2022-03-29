using Microsoft.AspNetCore.Mvc;

namespace Ecoba.TestService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController
{
    [HttpGet]
    public string get()
    {
        return "Test service";
    }
}
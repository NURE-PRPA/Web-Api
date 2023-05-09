using DL;
using Microsoft.AspNetCore.Mvc;
using Core.Models;
using Core.Enums;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        private QuantEdDbContext _dbContext;
        public TestController(QuantEdDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("/index")]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(true);
        }
    }
}

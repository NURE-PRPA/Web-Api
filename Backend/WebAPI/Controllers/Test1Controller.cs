using DL;
using Microsoft.AspNetCore.Mvc;
using Core.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test1Controller : Controller
    {
        private QuantEdDbContext _dbContext;
        public Test1Controller(QuantEdDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("/index1")]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(true);
        }
    }
}

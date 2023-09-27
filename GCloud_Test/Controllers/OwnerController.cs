using GCloud_Test.Models;
using GCloud_Test.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace GCloud_Test.Controllers
{
    //[("api/owner")]
    //[ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly ILogger<OwnerController> _logger;
        private readonly SampleObjectContext _context;

        public OwnerController(ILogger<OwnerController> logger, SampleObjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        //[HttpGet]
        public IActionResult Index()
        {
            try
            {
                string s = "Test Successful";
                _logger.LogInformation("Get success");
                return Ok(s);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Index action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpGet]
        public IActionResult TestAction()
        {
            try
            {
                string s = "Test 2 Successful";
                _logger.LogInformation("Get success 2");
                return Ok(s);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside TestAction: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult AddToDatabase(int id, string name, string description)
        {
            SampleObject obj = new SampleObject()
            {
                Id = id,
                Name = name,
                Description = description
            };
            _context.SampleObjects.Add(obj);
            _context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult GetDatabase()
        {
            string s = string.Empty;
            foreach (var obj in _context.SampleObjects)
            {
                s += obj.ToString() + "\n\n";
            }
            return Ok(s);
        }
    }
}

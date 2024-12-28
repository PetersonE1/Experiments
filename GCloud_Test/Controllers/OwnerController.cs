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
        private readonly OtherObjectContext _otherContext;

        public OwnerController(ILogger<OwnerController> logger, SampleObjectContext context, OtherObjectContext otherContext)
        {
            _logger = logger;
            _context = context;
            _otherContext = otherContext;
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

        public IActionResult AddToDatabase(int database, string name, string description)
        {
            if (database == 0)
            {
                SampleObject obj = new SampleObject()
                {
                    Name = name,
                    Description = description
                };
                _context.SampleObjects.Add(obj);
                _context.SaveChanges();
                return Ok();
            }
            OtherObject obj2 = new OtherObject()
            {
                Name = name,
                Description = description
            };
            _otherContext.OtherObjects.Add(obj2);
            _otherContext.SaveChanges();
            return Ok();
        }

        public IActionResult GetDatabase(int database)
        {
            string s = string.Empty;
            if (database == 0)
            {
                foreach (var obj in _context.SampleObjects)
                {
                    s += obj.ToString() + "\n\n";
                }
                return Ok(s);
            }
            foreach (var obj in _otherContext.OtherObjects)
            {
                s += obj.ToString() + "\n\n";
            }
            return Ok(s);
        }
    }
}

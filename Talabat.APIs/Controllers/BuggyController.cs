using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repositry.Data;

namespace Talabat.APIs.Controllers
{
    
    public class BuggyController : BaseAPIController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context) 
        {
            _context = context;
        }

        [HttpGet(template:"NotFound") ]
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(keyValues: 1000);

            if (product is null)
                return NotFound(value: new ApiResponse(statusCode:404));

            return Ok(product);

        }

        [HttpGet(template: "ServerError")]
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(keyValues: 1000);
            var result = product.ToString(); 

            return Ok(result);

        }

        [HttpGet(template: "badrequest")]
        public ActionResult GetBadRequest()
        {
          return BadRequest(error: new ApiResponse(statusCode: 400));

        }

        [HttpGet(template: "badrequest/{id}")]
        public ActionResult GetBadRequest(int? id)
        {
            return Ok();

        }

        [HttpGet(template: "unauthorized")]
        public ActionResult GetUnauthorizedError(int? id)
        {
            return Unauthorized(value: new ApiResponse(statusCode: 401));

        }


    }
}

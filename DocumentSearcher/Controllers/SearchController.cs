using Microsoft.AspNetCore.Mvc;
using DocumentSearcher.Entities;
using DocumentSearcher.Services;

namespace DocumentSearcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        SearchService _service;
        public SearchController(SearchService service)
        {
            _service = service;
        }
        [HttpGet]
        public IEnumerable<Payment> Get([FromQuery(Name = "q")]string query)
        {
            var res = _service.SearchPayment(query);
            return res;
        }


    }

}
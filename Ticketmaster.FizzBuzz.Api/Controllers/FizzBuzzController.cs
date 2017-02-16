using System.Web.Http;
using Ticketmaster.FizzBuzz.Models;
using Ticketmaster.FizzBuzz.Rules;

namespace Ticketmaster.FizzBuzz.Api.Controllers
{
    public class FizzBuzzController : ApiController
    {
        private readonly IFizzBuzzCalculator _calculator;
        private readonly IFizzBuzzRuleRepository _repository;

        public FizzBuzzController(IFizzBuzzCalculator calculator, IFizzBuzzRuleRepository repository)
        {
            _repository = repository;
            _calculator = calculator;
        }

        [HttpGet]
        [Route("api/FizzBuzz/Start/{start}/End/{end}")]
        public IHttpActionResult GetFizzBuzz(int start, int end)
        {
            return Ok(_calculator.GetResultRange(start, end));
        }


        [HttpPost]
        [Route("api/FizzBuzz/Rules")]
        public IHttpActionResult AddRule([FromBody]Rule rule)
        {
            if (_repository.CheckExists(rule.Index))
            {
                return Conflict();
            }

            _repository.Add(rule);
            return Created("FizzBuzz", rule);
        }

        [HttpPut]
        [Route("api/FizzBuzz/Rules/{index}")]
        public IHttpActionResult UpdateRule(int index, [FromBody]Rule rule)
        {
            if (!_repository.CheckExists(rule.Index))
            {
                return NotFound();
            }

            _repository.Update(index, rule);
            return Ok(rule);
        }

        [HttpPut]
        [Route("api/FizzBuzz/Rules/")]
        public IHttpActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }
    }
}

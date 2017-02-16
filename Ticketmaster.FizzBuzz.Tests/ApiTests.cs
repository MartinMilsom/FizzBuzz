using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ticketmaster.FizzBuzz.Api.Controllers;
using Ticketmaster.FizzBuzz.Rules;
using Rhino.Mocks;
using System.Web.Http.Results;
using Ticketmaster.FizzBuzz.Models;
using System.Collections.Generic;
using System.Linq;

namespace Ticketmaster.FizzBuzz.Tests
{
    [TestClass]
    public class ApiTests
    {
        private FizzBuzzController _target;
        private IFizzBuzzRuleRepository _repository;

        [TestInitialize]
        public void Init()
        {
            var calculator = MockRepository.GenerateMock<IFizzBuzzCalculator>();
            calculator.Stub(x => x.GetResultRange(Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                .Return(new FizzBuzzResult
                {
                    Result = "1 2 fizz",
                    Summary = new Dictionary<string, int>
                    {
                         { "fizz", 1 },
                         { "integer", 2 }
                    }
                });

            _repository = MockRepository.GenerateMock<IFizzBuzzRuleRepository>();
            _target = new FizzBuzzController(calculator, _repository);
        }

        [TestMethod]
        public void GetFizzBuzzResult_ExpectString()
        {
            var result = _target.GetFizzBuzz(1, 20);

            Assert.IsNotNull(result);
            var response = result as OkNegotiatedContentResult<FizzBuzzResult>;
            Assert.IsNotNull(response);
            Assert.AreEqual("1 2 fizz", response.Content.Result);
            Assert.IsNotNull(response.Content.Summary);
            Assert.AreEqual(2, response.Content.Summary.Count);

        }
        
        [TestMethod]
        public void AddNewRule_ExpectRuleStored()
        {
            _repository.Stub(x => x.Add(Arg<Rule>.Is.Anything));
            _repository.Stub(x => x.CheckExists(Arg<int>.Is.Anything)).Return(false);

            var ruleToAdd = new Rule { Index = 8, Word = "boing", RuleType = RuleType.MultipleOf };
            var result = _target.AddRule(ruleToAdd);

            Assert.IsNotNull(result);
            var response = result as CreatedNegotiatedContentResult<Rule>;
            Assert.IsNotNull(response);
            Assert.AreEqual(ruleToAdd.Index, response.Content.Index);
            Assert.AreEqual(ruleToAdd.Word, response.Content.Word);
            Assert.AreEqual(ruleToAdd.RuleType, response.Content.RuleType);

            _repository.AssertWasCalled(x => x.Add(Arg<Rule>.Is.Anything));
        }

        [TestMethod]
        public void AddNewRule_WhereIndexExists_ExpectConflict()
        {
            _repository.Stub(x => x.CheckExists(Arg<int>.Is.Anything)).Return(true);

            var result = _target.AddRule(new Rule());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConflictResult));
            _repository.AssertWasNotCalled(x => x.Add(Arg<Rule>.Is.Anything));
        }

        [TestMethod]
        public void UpdateRule_WhereExists_ExpectRuleUpdated()
        {
            _repository.Stub(x => x.Update(Arg<int>.Is.Anything, Arg<Rule>.Is.Anything));
            _repository.Stub(x => x.CheckExists(Arg<int>.Is.Anything)).Return(true);

            var updatedRule = new Rule { Index = 12, Word = "boom", RuleType = RuleType.MultipleOf };
            var result = _target.UpdateRule(updatedRule.Index, updatedRule);

            Assert.IsNotNull(result);
            var response = result as OkNegotiatedContentResult<Rule>;
            Assert.IsNotNull(response);
            Assert.AreEqual(updatedRule.Index, response.Content.Index);
            Assert.AreEqual(updatedRule.Word, response.Content.Word);
            Assert.AreEqual(updatedRule.RuleType, response.Content.RuleType);

            _repository.AssertWasCalled(x => x.Update(Arg<int>.Is.Anything, Arg<Rule>.Is.Anything));
        }

        [TestMethod]
        public void UpdateRule_WhereNotExists_ExpectNotFound()
        {
            _repository.Stub(x => x.Update(Arg<int>.Is.Anything, Arg<Rule>.Is.Anything));
            _repository.Stub(x => x.CheckExists(Arg<int>.Is.Anything)).Return(false);

            var updatedRule = new Rule { Index = 12, Word = "boom", RuleType = RuleType.MultipleOf };
            var result = _target.UpdateRule(updatedRule.Index, updatedRule);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            _repository.AssertWasNotCalled(x => x.Update(Arg<int>.Is.Anything, Arg<Rule>.Is.Anything));
        }

        [TestMethod]
        public void GetRules_ExpectRulesReturned()
        {
            var rule = new Rule { Index = 12, Word = "boom", RuleType = RuleType.MultipleOf };
            _repository.Stub(x => x.GetAll()).Return(new List<Rule> { rule });

            var result = _target.GetAll();

            Assert.IsNotNull(result);
            var response = result as OkNegotiatedContentResult<IList<Rule>>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Content.Count());
            var retrievedRule = response.Content.First();
            Assert.AreEqual(rule.Index, retrievedRule.Index);
            Assert.AreEqual(rule.Word, retrievedRule.Word);
            Assert.AreEqual(rule.RuleType, retrievedRule.RuleType);
        }
    }
}

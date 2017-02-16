using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ticketmaster.FizzBuzz;
using Ticketmaster.FizzBuzz.Rules;
using Rhino.Mocks;
using Ticketmaster.FizzBuzz.Models;
using System.Collections.Generic;

namespace Ticketmaster.FizBuzz.Tests
{
    [TestClass]
    public class FizzBuzzCalculatorTests
    {
        private FizzBuzzCalculator _target;
        [TestInitialize]
        public void Init()
        {
            var repository = MockRepository.GenerateMock<IFizzBuzzRuleRepository>();
            repository.Stub(x => x.GetAll()).Return(new List<Rule>
            {
                new Rule { Index = 3, Word= "fizz", RuleType = RuleType.MultipleOf },
                new Rule { Index = 5, Word= "buzz", RuleType = RuleType.MultipleOf }
            });

            _target = new FizzBuzzCalculator(repository, new FizzBuzzRuleFactory());
        }


        [TestMethod]
        public void InputRangeOneToTwenty_ExpectFizzbuzz()
        {
            var result = _target.GetResultRange(1, 20);

            Assert.AreEqual("1 2 fizz 4 buzz fizz 7 8 fizz buzz 11 fizz 13 14 fizzbuzz 16 17 fizz 19 buzz", result.Result);
            Assert.AreEqual(result.Summary["fizz"], 5);
            Assert.AreEqual(result.Summary["buzz"], 3);
            Assert.AreEqual(result.Summary["fizzbuzz"], 1);
            Assert.AreEqual(result.Summary["integer"], 11);
        }

        [TestMethod]
        public void InputNumberOne_ExpectStringOne()
        {
            var result = _target.GetResult(1);

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void InputNumberThree_ExpectStringFizz()
        {
            var result = _target.GetResult(3);

            Assert.AreEqual("fizz", result);
        }

        [TestMethod]
        public void InputNumberSix_ExpectStringFizz()
        {
            var result = _target.GetResult(6);

            Assert.AreEqual("fizz", result);
        }

        [TestMethod]
        public void InputNumberFive_ExpectStringBuzz()
        {
            var result = _target.GetResult(5);

            Assert.AreEqual("buzz", result);
        }

        [TestMethod]
        public void InputNumberTen_ExpectStringBuzz()
        {
            var result = _target.GetResult(10);

            Assert.AreEqual("buzz", result);
        }

        [TestMethod]
        public void InputNumberFifteen_ExpectStringFizzBuzz()
        {
            var result = _target.GetResult(15);

            Assert.AreEqual("fizzbuzz", result);
        }

        [TestMethod]
        public void InputNumberSixty_ExpectStringFizzBuzz()
        {
            var result = _target.GetResult(60);

            Assert.AreEqual("fizzbuzz", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InputNumberLessThanOne_ExpectError()
        {
            var result = _target.GetResult(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void InputRange_EndGreaterThanStart_ExpectError()
        {
            var result = _target.GetResultRange(10,9);
        }
    }
}

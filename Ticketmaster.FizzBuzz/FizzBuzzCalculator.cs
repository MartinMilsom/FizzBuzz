using System;
using System.Collections.Generic;
using System.Linq;
using Ticketmaster.FizzBuzz.Models;
using Ticketmaster.FizzBuzz.Rules;

namespace Ticketmaster.FizzBuzz
{
    public interface IFizzBuzzCalculator
    {
        FizzBuzzResult GetResultRange(int start, int end);
        string GetResult(int number);
    }

    public class FizzBuzzCalculator : IFizzBuzzCalculator
    {
        private readonly IFizzBuzzRuleRepository _ruleRepository;
        private readonly IFizzBuzzRuleFactory _ruleTypeFactory;
        public FizzBuzzCalculator(IFizzBuzzRuleRepository ruleRepository, IFizzBuzzRuleFactory ruleTypeFactory)
        {
            _ruleRepository = ruleRepository;
            _ruleTypeFactory = ruleTypeFactory;
        }

        public FizzBuzzResult GetResultRange(int start, int end)
        {
            if (start > end) throw new ApplicationException("Start of the range must be a lower number than the end");

            var results = new List<string>();

            for(int i = start; i <= end; i++)
            {
                results.Add(GetResult(i));
            }

            return new FizzBuzzResult
            {
                Result = string.Join(" ", results),
                Summary = GetSummary(results)
            };
        }

        public string GetResult(int number)
        {
            if (!IsValid(number)) throw new ArgumentOutOfRangeException("Number must be greater than 0");

            string result = "";
            foreach (var rule in _ruleRepository.GetAll())
            {
                var resolver = _ruleTypeFactory.Create(rule.RuleType);
                result += resolver.GetWord(number, rule);
            }

            return string.IsNullOrEmpty(result) ? number.ToString() : result;
        }

        private Dictionary<string, int> GetSummary(List<string> words)
        {
            var wordRules = words.Where(x =>
            {
                int output;
                return !int.TryParse(x, out output);
            }).Distinct(); 
            var result = new Dictionary<string, int>();
            foreach (var rule in wordRules)
            {
                result.Add(rule, words.Where(x => string.Equals(rule, x)).Count());
            }
            result.Add("integer", words.Except(wordRules).Count());
            return result;
        }

        private bool IsValid(int number)
        {
            if (number < 1) return false;

            return true;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Ticketmaster.FizzBuzz.Models;

namespace Ticketmaster.FizzBuzz.Rules
{
    public interface IFizzBuzzRuleRepository
    {
        void Add(Rule rule);
        void Update(int index, Rule rule);
        IList<Rule> GetAll();
        bool CheckExists(int number);
    }

    public class FizzBuzzRuleRepository : IFizzBuzzRuleRepository
    {
        private List<Rule> _rules;

        public FizzBuzzRuleRepository()
        {
            //Just using a list in-memory for the purposes of this.
            
            _rules = new List<Rule>
            { 
                //seed
                new Rule { Index = 3, Word= "fizz", RuleType = RuleType.MultipleOf },
                new Rule { Index = 5, Word= "buzz", RuleType = RuleType.MultipleOf }
            };
        }

        public void Add(Rule rule)
        {
            _rules.Add(rule);
        }

        public void Update(int number, Rule rule)
        {
            var existing = _rules.SingleOrDefault(x => x.Index == number);
            if (existing == null) return;

            _rules.Remove(existing);
            _rules.Add(rule);
        }

        public IList<Rule> GetAll()
        {
            return _rules.OrderBy(x=>x.Index).ToList();
        }

        public bool CheckExists(int number)
        {
            return _rules.Any(x => x.Index == number);
        }

    }
}

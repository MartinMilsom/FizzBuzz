using Ticketmaster.FizzBuzz.Models;

namespace Ticketmaster.FizzBuzz.Rules
{
    public interface IFizzBuzzRuleFactory
    {
        IRuleResolver Create(RuleType ruleType);
    }

    public class FizzBuzzRuleFactory : IFizzBuzzRuleFactory
    {
        public IRuleResolver Create(RuleType ruleType)
        {
            // as it stands the game only has one type of rule (ie, if multiple of 'N', return <string>
            // this factory can handle different rule types where needed. e.g. if the number is equal to exactly 'N' return <string>
            return new MultipleOfRuleResolver();
        }
    }
}

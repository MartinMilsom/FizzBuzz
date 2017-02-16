using Ticketmaster.FizzBuzz.Models;

namespace Ticketmaster.FizzBuzz.Rules
{
    public class MultipleOfRuleResolver : IRuleResolver
    {
        public string GetWord(int number, Rule rule)
        {
            return (number % rule.Index == 0) ? rule.Word : string.Empty;
        }
    }
}

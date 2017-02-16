using Ticketmaster.FizzBuzz.Models;

namespace Ticketmaster.FizzBuzz.Rules
{
    public interface IRuleResolver
    {
        string GetWord(int index, Rule rule);
    }
}

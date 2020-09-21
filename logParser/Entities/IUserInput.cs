using System.Collections.Generic;

namespace logParser
{
    public interface IUserInput
    {
        char Delemeter { get; } 
        string Destination { get; }
        string Source { get; }
        HashSet<string> UserGivenLevels { get; }

    }
}
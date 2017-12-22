using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class TournamentState
    {
        public TypesOfObject[,] Map { get; set; }
        public Statistics WhiteStatistics { get; set; }
        public Statistics BlackStatistics { get; set; }
    }
}
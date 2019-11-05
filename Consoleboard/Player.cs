using System;

namespace Consoleboard
{
    public class Player
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public int Scores { get; set; }
        public string Status { get; set; }
        public string PlayerName { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    enum Status
    {
        RUNNING = 1,
        STOPPED = 2,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZitaAsteria.Scene
{
    public class ProfileEntry
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ProfileEntry(string name, DateTime start)
        {
            Name = name;
            StartTime = start;
        }

        public double GetIntervalMS()
        {
            return (EndTime - StartTime).TotalMilliseconds;
        }
    }
}

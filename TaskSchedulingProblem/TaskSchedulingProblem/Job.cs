using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulingProblem
{
    class Job
    {
        public int Number { get; set; }
        public int TimeSpan { get; set; }
        public int StartTime { get; set; }
        public bool Assigned { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulingProblem
{
    class Machine
    {
        public int id { get; set; }
        public List<Job> jobs { get; set; }
        public int IdleTime { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulingProblem
{
    class Bat
    {
        public List<Machine> Machines { get; set; } = new();
        public float Frequency { get; set; }
        public float Velocity { get; set; } = default;

    }
}

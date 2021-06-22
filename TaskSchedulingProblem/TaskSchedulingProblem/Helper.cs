using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulingProblem
{
    static class Helper
    {
        static public bool BatEqual(Bat bat1, Bat bat2)
        {
            return bat1.Machines.Equals(bat2.Machines);
        }

        static public Tuple<int, int, int> Read()
        {
            int numberOfMachines = 0;
            int numberOfJobs = 0;
            int numberOfBats = 0;
            while (!numberOfMachines.Equals(numberOfJobs))
            {
                Console.WriteLine("The number of machines must be equal to the number of jobs.");
                bool a = Int32.TryParse(Console.ReadLine(), out numberOfMachines);
                bool b = Int32.TryParse(Console.ReadLine(), out numberOfJobs);
                //validation (it should be a number)
                while (!a)
                {
                    a = Int32.TryParse(Console.ReadLine(), out numberOfMachines);
                }
                while (!b)
                {
                    b = Int32.TryParse(Console.ReadLine(), out numberOfJobs);
                }
            }
            bool c = Int32.TryParse(Console.ReadLine(), out numberOfBats);
            while (!c)
            {
                c = Int32.TryParse(Console.ReadLine(), out numberOfBats);
            }
            return new Tuple<int, int, int>(numberOfMachines, numberOfJobs, numberOfBats);
        }

        static public List<Bat> InitializePopolaution(int numberOfMachines,int numberOfJobs,int numberOfBats)
        {
            List<Tuple<int, int>> list=new();
            int machine = 1;
            int job= 1;
            for(int index=0;index<numberOfJobs*numberOfMachines;++index)
            {
                list.Add(new Tuple<int, int>(machine, job));
                ++job;
                if(job>numberOfJobs)
                {
                    job = 1;
                    ++machine;
                }
            }

            return new List<Bat>();
        }

    }
}

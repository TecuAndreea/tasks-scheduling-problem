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
            for (int row = 0; row < bat1.Machines.Count; ++row)
            {
                for (int column = 0; column < bat1.Machines[row].Jobs.Count; ++column)
                {
                    if (bat1.Machines[row].Jobs[column].Number != bat2.Machines[row].Jobs[column].Number)
                    {
                        return true;
                    }
                }
            }
            return false;
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

        static public List<Bat> InitializeBatPopulation(int numberOfMachines, int numberOfJobs, int numberOfBats)
        {
            List<List<Tuple<int, int>>> list = HelperInitialization(numberOfMachines, numberOfJobs);

            List<Bat> bats = BatMachines(list, numberOfMachines).OrderBy(x => Guid.NewGuid()).Take(numberOfBats).ToList();

            foreach (Bat bat in bats)
            {
                PrintResult(bat);
            }

            return bats;
        }

        static private Bat InitializeFirstBat(List<List<Tuple<int, int>>> list, int numberOfMachines)
        {
            Bat posibleBat = new();

            for (int i = 0; i < numberOfMachines; ++i)
            {
                posibleBat.Machines.Add(new Machine());
            }

            for (int column = 0; column < list[0].Count; ++column)
            {
                posibleBat.Machines[list[0][column].Item1 - 1].Id = list[0][column].Item1;
                posibleBat.Machines[list[0][column].Item1 - 1].Jobs.Add(new Job() { Number = list[0][column].Item2 });
            }

            return posibleBat;
        }

        static private List<Bat> BatMachines(List<List<Tuple<int, int>>> list, int numberOfMachines)
        {
            List<Bat> batMachines = new();
            batMachines.Add(InitializeFirstBat(list, numberOfMachines));

            for (int row = 1; row < list.Count; ++row)
            {
                Bat posibleBat = new();

                for (int i = 0; i < numberOfMachines; ++i)
                {
                    posibleBat.Machines.Add(new Machine());
                }

                for (int column = 0; column < list[row].Count; ++column)
                {
                    posibleBat.Machines[list[row][column].Item1 - 1].Id = list[row][column].Item1;
                    posibleBat.Machines[list[row][column].Item1 - 1].Jobs.Add(new Job() { Number = list[row][column].Item2 });
                }

                bool exists = false;
                for (int index = 0; index < batMachines.Count; ++index)
                {
                    if (!BatEqual(batMachines[index], posibleBat))
                    {
                        exists = true;
                    }
                }

                if(!exists)
                {
                    batMachines.Add(posibleBat);
                }
            }

            Console.WriteLine("Here");
            return batMachines;
        }

        static public List<List<Tuple<int, int>>> HelperInitialization(int numberOfMachines, int numberOfJobs)
        {
            List<List<Tuple<int, int>>> permutations = new();

            List<Tuple<int, int>> list = new();
            int machine = 1;
            int job = 1;
            for (int index = 0; index < numberOfJobs * numberOfMachines; ++index)
            {
                list.Add(new Tuple<int, int>(machine, job));
                ++job;
                if (job > numberOfJobs)
                {
                    job = 1;
                    ++machine;
                }
            }

            return DoPermute(list, 0, list.Count - 1, permutations);
        }

        static public List<List<Tuple<int, int>>> DoPermute(List<Tuple<int, int>> nums, int start, int end, List<List<Tuple<int, int>>> list)
        {
            if (start == end)
            {
                list.Add(new List<Tuple<int, int>>(nums));
            }
            else
            {
                for (var index = start; index <= end; ++index)
                {
                    var temp = nums[start];
                    nums[start] = nums[index];
                    nums[index] = temp;
                    DoPermute(nums, start + 1, end, list);
                    temp = nums[start];
                    nums[start] = nums[index];
                    nums[index] = temp;
                }
            }

            return list;
        }

        static void PrintResult(Bat bat)
        {
            for (int row = 0; row < bat.Machines.Count; ++row)
            {
                for (int column = 0; column < bat.Machines[row].Jobs.Count; ++column)
                {
                    Console.Write(bat.Machines[row].Jobs[column].Number + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}

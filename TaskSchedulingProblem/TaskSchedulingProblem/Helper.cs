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

        static public Tuple<int, int, int, int> Read()
        {
            int numberOfMachines = 0;
            int numberOfJobs = 0;
            int numberOfBats = 0;
            int maxGeneration = 0;
            while (numberOfMachines == 0)
            {
                Console.WriteLine("Introduce number of machines and jobs:");

                bool a = Int32.TryParse(Console.ReadLine(), out numberOfMachines);
                while (!a)
                {
                    Console.WriteLine("Introduce only numbers");
                    a = Int32.TryParse(Console.ReadLine(), out numberOfMachines);
                }
            }
            numberOfJobs = numberOfMachines;

            while (numberOfBats == 0)
            {
                Console.WriteLine("Introduce number of bats:");

                bool a = Int32.TryParse(Console.ReadLine(), out numberOfBats);
                while (!a)
                {
                    Console.WriteLine("Introduce only numbers");
                    a = Int32.TryParse(Console.ReadLine(), out numberOfBats);
                }
            }

            while (maxGeneration == 0)
            {
                Console.WriteLine("Introduce number of maxGeneration:");

                bool a = Int32.TryParse(Console.ReadLine(), out maxGeneration);
                while (!a)
                {
                    Console.WriteLine("Introduce only numbers");
                    a = Int32.TryParse(Console.ReadLine(), out maxGeneration);
                }
            }

            return new Tuple<int, int, int, int>(numberOfMachines, numberOfJobs, numberOfBats, maxGeneration);
        }

        static public List<Bat> InitializeBatPopulation(int numberOfMachines, int numberOfJobs, int numberOfBats)
        {
            List<List<Tuple<int, int>>> list = HelperInitialization(numberOfMachines, numberOfJobs);

            List<Bat> bats = BatMachines(list, numberOfMachines).OrderBy(x => Guid.NewGuid()).Take(numberOfBats).ToList();

            Console.WriteLine();
            for (int bat = 0; bat < numberOfBats; bat++)
            {
                Console.WriteLine("Bat " + (bat + 1));
                PrintResult(bats[bat]);
            }

            var rand = new Random();

            foreach (Bat bat in bats)
            {
                foreach (var machine in bat.Machines)
                {
                    foreach (var job in machine.Jobs)
                    {
                        job.TimeSpan = rand.Next(1, 31);
                    }
                }
            }
            foreach (Bat bat in bats)
            {
                for (int machine = 0; machine < numberOfMachines; ++machine)
                {
                    for (int job = 0; job < numberOfJobs; ++job)
                    {
                        List<int> possibleSolution = new();
                        possibleSolution.Add(bat.Machines[machine].IdleTime);
                        for (int k = 0; k < numberOfMachines; ++k)
                        {
                            if (k != machine && bat.Machines[k].Jobs[job].Assigned == true)
                            {
                                possibleSolution.Add(bat.Machines[k].Jobs[job].StartTime + bat.Machines[k].Jobs[job].TimeSpan);

                            }
                        }
                        bat.Machines[machine].Jobs[job].StartTime = possibleSolution.Max();
                        bat.Machines[machine].Jobs[job].Assigned = true;
                        bat.Machines[machine].IdleTime = bat.Machines[machine].Jobs[job].StartTime + bat.Machines[machine].Jobs[job].TimeSpan;
                    }
                }
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

                if (!exists)
                {
                    batMachines.Add(posibleBat);
                }
            }

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

        static public void PrintResult(Bat bat)
        {
            for (int row = 0; row < bat.Machines.Count; ++row)
            {
                Console.Write("Machine " + bat.Machines[row].Id + " : ");
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

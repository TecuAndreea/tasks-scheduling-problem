using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulingProblem
{
    class TaskSchedule
    {
        public List<Bat> Bats { get; set; }
        public int MaxGeneration { get; set; }

        public int MachineNumber { get; set; }

        public int JobNumber { get; set; }

        public int BatNumber { get; set; }

        public Bat Solution { get; set; }

        public TaskSchedule()
        {
            Tuple<int, int, int, int> values = Helper.Read();
            MachineNumber = values.Item1;
            JobNumber = values.Item2;
            BatNumber = values.Item3;
            MaxGeneration = values.Item4;
            Bats = Helper.InitializeBatPopulation(MachineNumber, JobNumber, BatNumber);
            Solution = BatAlgorithm();
            Console.WriteLine("The solution is :");
            Helper.PrintResult(Solution);
        }

        public void ShiftUp(ref Bat bat, int column)
        {
            var value = bat.Machines[0].Jobs[column].Number;
            for (int index1 = 0; index1 < MachineNumber - 1; ++index1)
            {
                for (int index2 = 0; index2 < JobNumber; ++index2)
                {

                    if (column!=index2 && bat.Machines[index1].Jobs[index2].Number == bat.Machines[index1 + 1].Jobs[column].Number )
                    {
                        bat.Machines[index1].Jobs[index2].Number = bat.Machines[index1].Jobs[column].Number;
                        break;
                    }
                }
                bat.Machines[index1].Jobs[column].Number = bat.Machines[index1 + 1].Jobs[column].Number;
            }
            for (int index2 = 0; index2 < JobNumber; ++index2)
            {
                if (column != index2 && bat.Machines[MachineNumber - 1].Jobs[index2].Number == value)
                {
                    bat.Machines[MachineNumber - 1].Jobs[index2] .Number= bat.Machines[MachineNumber - 1].Jobs[column].Number;
                    break;
                }
            }
            bat.Machines[MachineNumber - 1].Jobs[column].Number = value;
        }

        public void ShiftDown(ref Bat bat, int column)
        {
            var value = bat.Machines[MachineNumber - 1].Jobs[column].Number;
            for (int index1 = 1; index1 < MachineNumber; ++index1)
            {
                for (int index2 = 0; index2 < JobNumber; ++index2)
                {
                    if (column != index2 && bat.Machines[index1].Jobs[index2].Number == bat.Machines[index1 - 1].Jobs[column].Number)
                    {
                        bat.Machines[index1].Jobs[index2].Number = bat.Machines[index1].Jobs[column].Number;
                        break;
                    }
                }
                bat.Machines[index1].Jobs[column].Number = bat.Machines[index1 - 1].Jobs[column].Number;
            }
            for (int index2 = 0; index2 < JobNumber; ++index2)
            {
                if (column != index2 && bat.Machines[0].Jobs[index2].Number == value)
                {
                    bat.Machines[0].Jobs[index2].Number = bat.Machines[0].Jobs[column].Number;
                    break;
                }
            }
            bat.Machines[0].Jobs[column] .Number= value;
        }

        public int ColReuse(Bat bat, Bat bestBat)
        {
            int maxColReuseBat = 0;
            int maxColReuseBestBat = 0;

            for (int column = 0; column < JobNumber; ++column)
            {
                int[] markBat = new int[JobNumber];
                Array.Clear(markBat, 0, JobNumber);

                int[] markBestBat = new int[JobNumber];
                Array.Clear(markBestBat, 0, JobNumber);

                for (int row = 0; row < MachineNumber; ++row)
                {
                    ++markBat[bat.Machines[row].Jobs[column].Number - 1];
                    ++markBestBat[bestBat.Machines[row].Jobs[column].Number - 1];

                    if (markBat[bat.Machines[row].Jobs[column].Number - 1] > maxColReuseBat)
                    {
                        maxColReuseBat = markBat[bat.Machines[row].Jobs[column].Number - 1];
                    }

                    if (markBestBat[bestBat.Machines[row].Jobs[column].Number - 1] > maxColReuseBestBat)
                    {
                        maxColReuseBestBat = markBestBat[bestBat.Machines[row].Jobs[column].Number - 1];
                    }
                }
            }

            return Math.Abs(maxColReuseBat - maxColReuseBestBat);
        }

        public void InactionDel(ref Bat bat)
        {
            int maxIdleTime = 0;
            int row = 0;

            foreach (var machine in bat.Machines)
            {
                if (machine.IdleTime > maxIdleTime)
                {
                    maxIdleTime = machine.IdleTime;
                    row = machine.Id - 1;
                }
            }

            var value = bat.Machines[row].Jobs[JobNumber - 1].Number;
            for (int column = 1; column < JobNumber; ++column)
            {
                bat.Machines[row].Jobs[column].Number = bat.Machines[row].Jobs[column - 1].Number;
            }
            bat.Machines[row].Jobs[0].Number = value;
        }

        public void SmallWalk(ref Bat bat)
        {
            var random = new Random();

            int row1 = random.Next(0, MachineNumber);
            int column1 = random.Next(0, JobNumber);

            int row2 = random.Next(0, MachineNumber);
            int column2 = random.Next(0, JobNumber);

            var value = bat.Machines[row1].Jobs[column1].Number;
            for (int column = 0; column < JobNumber; ++column)
            {
                if (bat.Machines[row1].Jobs[column].Number == bat.Machines[row2].Jobs[column2].Number && column!=column1)
                {
                    bat.Machines[row1].Jobs[column].Number = bat.Machines[row1].Jobs[column1].Number;
                    bat.Machines[row1].Jobs[column1].Number = bat.Machines[row2].Jobs[column2].Number;
                    break;
                }
            }
            for (int column = 0; column < JobNumber; ++column)
            {
                if (bat.Machines[row2].Jobs[column].Number == value && column!=column2)
                {
                    bat.Machines[row2].Jobs[column].Number = bat.Machines[row2].Jobs[column2].Number;
                    bat.Machines[row2].Jobs[column2].Number = value;
                    break;
                }
            }
        }

        public void Fold(ref Bat bat)
        {
            var rand = new Random();
            int row = rand.Next(0, MachineNumber);
            bat.Machines[row].Jobs.Reverse();
        }

        public void FullReverse(ref Bat bat)
        {
            foreach (var machine in bat.Machines)
            {
                machine.Jobs.Reverse();
            }
        }

        public List<int> SelectFewRows()
        {
            var rand = new Random();
            int nrRows = rand.Next(0, MachineNumber);
            List<int> rows = new();
            int index = 0;
            while (index < nrRows)
            {
                int row = rand.Next(0, MachineNumber);
                if (!rows.Contains(row))
                {
                    rows.Add(row);
                    ++index;
                }
            }
            return rows;
        }

        public void Join(ref Bat bat)
        {
            List<int> rows = SelectFewRows();
            var rand = new Random();
            int nrBat = rand.Next(0, Bats.Count);
            Bat selectedBat = Bats[nrBat];

            for (int row = 0; row < rows.Count; ++row)
            {
                bat.Machines[rows[row]].Jobs = selectedBat.Machines[rows[row]].Jobs;
            }
        }

        public float ObjectiveFunction(Bat bat)
        {
            float sum = 0;
            foreach (var machine in bat.Machines)
            {
                foreach (var job in machine.Jobs)
                {
                    sum += job.TimeSpan;
                }
            }

            return sum;
        }

        public Bat InitialBestBat()
        {
            float min = ObjectiveFunction(Bats[0]);
            Bat bestBat = new();

            foreach (Bat bat in Bats)
            {
                float fitness = ObjectiveFunction(bat);
                if (fitness <= min)
                {
                    min = fitness;
                    bestBat = bat;
                }
            }

            return bestBat;
        }

        public void ChooseMoveFunction(ref Bat bat)
        {
            var rand = new Random();
            int option = rand.Next(0, 5);
            switch (option)
            {
                case 0:
                    {
                        Fold(ref bat);
                        break;
                    }
                case 1:
                    {
                        FullReverse(ref bat);
                        break;
                    }
                case 2:
                    {
                        Join(ref bat);
                        break;
                    }
                case 3:
                    {
                        ShiftDown(ref bat, rand.Next(0, JobNumber));
                        break;
                    }
                case 4:
                    {
                        ShiftUp(ref bat, rand.Next(0, JobNumber));
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
        }

        public Bat BatAlgorithm()
        {
            float loudness = 0.95f;
            float pulseRate = 1f;
            var rand = new Random();
            Bat bestBat = InitialBestBat();

            for (int index1 = 1; index1 < MaxGeneration; ++index1)
            {
                for (int index2 = 0; index2 < BatNumber; ++index2)
                {
                    Bat tempBat = Bats[index2];
                    Bats[index2].Frequency = rand.Next(1, 100);
                    Bats[index2].Velocity += ColReuse(Bats[index2], bestBat) * Bats[index2].Frequency;
                    ChooseMoveFunction(ref tempBat);

                    float randomPulseRate = rand.Next(0, 1);
                    if (randomPulseRate > 1 - pulseRate)
                    {
                        SmallWalk(ref tempBat);
                        InactionDel(ref tempBat);
                    }

                    float randomLoudness = rand.Next(0, 1);
                    if (randomLoudness < loudness)
                    {
                        Bats[index2] = tempBat;
                    }

                    if (ObjectiveFunction(Bats[index2]) < ObjectiveFunction(bestBat))
                    {
                        bestBat = Bats[index2];
                    }
                }
                pulseRate = 1 - (1 / (MaxGeneration + 1 - index1));
            }
            return bestBat;
        }
    }
}

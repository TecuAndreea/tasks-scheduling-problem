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

        public TaskSchedule()
        {
            Bats = Helper.InitializeBatPopulation(3, 3, 4);
        }

        public void ShiftUp(ref Bat bat, int column)
        {
            var value = bat.Machines[0].Jobs[column];
            for (int index1 = 0; index1 < MachineNumber - 1; ++index1)
            {
                for (int index2 = 0; index2 < JobNumber; ++index2)
                {
                    if (bat.Machines[index1].Jobs[index2] == bat.Machines[index1++].Jobs[column])
                    {
                        bat.Machines[index1].Jobs[index2] = bat.Machines[index1].Jobs[column];
                    }
                }
                bat.Machines[index1].Jobs[column] = bat.Machines[++index1].Jobs[column];
            }
            for (int index2 = 0; index2 < JobNumber; ++index2)
            {
                if (bat.Machines[MachineNumber - 1].Jobs[index2] == value)
                {
                    bat.Machines[MachineNumber - 1].Jobs[index2] = bat.Machines[MachineNumber - 1].Jobs[column];
                }
            }
            bat.Machines[MachineNumber - 1].Jobs[column] = value;
        }

        public void ShiftDown(ref Bat bat, int column)
        {
            var value = bat.Machines[MachineNumber - 1].Jobs[column];
            for (int index1 = 1; index1 < MachineNumber; ++index1)
            {
                for (int index2 = 0; index2 < JobNumber; ++index2)
                {
                    if (bat.Machines[index1].Jobs[index2] == bat.Machines[--index1].Jobs[column])
                    {
                        bat.Machines[index1].Jobs[index2] = bat.Machines[index1].Jobs[column];
                    }
                }
                bat.Machines[index1].Jobs[column] = bat.Machines[--index1].Jobs[column];
            }
            for (int index2 = 0; index2 < JobNumber; ++index2)
            {
                if (bat.Machines[0].Jobs[index2] == value)
                {
                    bat.Machines[0].Jobs[index2] = bat.Machines[0].Jobs[column];
                }
            }
            bat.Machines[0].Jobs[column] = value;
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

                    if (markBestBat[bat.Machines[row].Jobs[column].Number - 1] > maxColReuseBestBat)
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

            var value = bat.Machines[row].Jobs[JobNumber - 1];
            for (int column = 1; column < JobNumber; ++column)
            {
                bat.Machines[row].Jobs[column] = bat.Machines[row].Jobs[--column];
            }
            bat.Machines[row].Jobs[0] = value;
        }

        public void SmallWalk(ref Bat bat)
        {
            var random = new Random();

            int row1 = random.Next(0, MachineNumber);
            int column1 = random.Next(0, JobNumber);

            int row2 = random.Next(0, MachineNumber);
            int column2 = random.Next(0, JobNumber);

            var value = bat.Machines[row1].Jobs[column1];
            for (int column = 0; column < JobNumber; ++column)
            {
                if (bat.Machines[row1].Jobs[column].Number == bat.Machines[row2].Jobs[column2].Number)
                {
                    bat.Machines[row1].Jobs[column] = bat.Machines[row1].Jobs[column1];
                    bat.Machines[row1].Jobs[column1] = bat.Machines[row2].Jobs[column2];
                }

                if (bat.Machines[row2].Jobs[column].Number == bat.Machines[row1].Jobs[column1].Number)
                {
                    bat.Machines[row2].Jobs[column] = bat.Machines[row2].Jobs[column2];
                    bat.Machines[row2].Jobs[column2] = value;
                }
            }
        }

        public void Fold(Bat bat)
        {
            var rand = new Random();
            int row = rand.Next(0, MachineNumber);
            bat.Machines[row].Jobs.Reverse();
        }

        public void FullReverse(Bat bat)
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

        public void Join(Bat bat)
        {
            List<int> rows = SelectFewRows();
            var rand = new Random();
            int nrBat = rand.Next(0, Bats.Count);
            Bat selectedBat = Bats[nrBat];
            for(int row=0; row<rows.Count; ++row)
            {
                bat.Machines[rows[row]].Jobs = selectedBat.Machines[rows[row]].Jobs;
            }
        }
    }
}

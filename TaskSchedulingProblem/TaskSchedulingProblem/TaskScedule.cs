using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulingProblem
{
    class TaskScedule
    {
        public List<Bat> Bats { get; set; }
        public int MaxGeneration { get; set; }

        public int MachineNumber { get; set; }

        public int JobNumber { get; set; }

        public void ShiftUp(ref Bat bat, int column)
        {
            var value = bat.Machines[0].Jobs[column];
            for (int index1 = 0; index1 < MachineNumber-1; ++index1)
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
                if (bat.Machines[MachineNumber-1].Jobs[index2] == value)
                {
                    bat.Machines[MachineNumber - 1].Jobs[index2] = bat.Machines[MachineNumber - 1].Jobs[column];
                }
            }
            bat.Machines[MachineNumber - 1].Jobs[column] = value;
        }
        public void ShiftDown(ref Bat bat,int column)
        {
            var value = bat.Machines[MachineNumber-1].Jobs[column];
            for (int index1 = 1; index1 < MachineNumber ; ++index1)
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
    }
}

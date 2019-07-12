using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen
{
    public class TaskProgressManager
    {
        public readonly List<TaskProgress> TaskProgressList;
        public int OverallProgress { get { return (100 * TaskProgressList.Sum(t => t.Complete) / TaskProgressList.Sum(t => t.Weight)); } }
        public int TaskIndex { get; private set; }
        public TaskProgress CurrentTask { get { return TaskProgressList[TaskIndex]; } }
        public TaskProgressState Status { get { return CurrentTask.Status; } }
        public bool Completed { get { return CurrentTask.Status == TaskProgressState.Completed; } }
        public bool NoData { get; set; }
        public bool Canceled { get; set; }

        //進捗オブジェクト
        public IProgress<bool> Progress { get; set; }

        public TaskProgressManager(List<TaskProgress> taskProgressList)
        {
            if (taskProgressList == null || taskProgressList.Count == 0) throw new ArgumentNullException(nameof(taskProgressList));

            TaskProgressList = taskProgressList;
            TaskProgressList.First().Status = TaskProgressState.InProcess;
        }

        public void UpdateState()
        {
            if (CurrentTask == null) return;
            if (CurrentTask.Status != TaskProgressState.InProcess) return;

            CurrentTask.Complete++;
            var taskCompleted = false;
            if (CurrentTask.Complete >= CurrentTask.Weight)
            {
                CurrentTask.Status = TaskProgressState.Completed;
                CurrentTask.EndTime = DateTime.Now;
                taskCompleted = true;

                MoveNextTask();
            }

            Progress?.Report(taskCompleted);
        }

        private bool MoveNextTask()
        {
            if (TaskIndex < TaskProgressList.Count - 1)
            {
                TaskIndex++;
                return true;
            }
            return false;
        }

        public void Abort()
        {
            CurrentTask.Status = TaskProgressState.Error;
            Progress?.Report(false);
        }
        public void Cancel()
        {
            CurrentTask.Status = TaskProgressState.Cancel;
            Progress?.Report(false);
        }

        public void NotFind()
        {
            NoData = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen
{
    public class TaskProgress
    {
        /// <summary>進捗状況</summary>
        public TaskProgressState Status { get; set; } = TaskProgressState.InProcess;

        /// <summary>タスク名</summary>
        public string Name { get; }
        /// <summary>処理件数</summary>
        public int Weight { get; }
        /// <summary>完了件数</summary>
        public int Complete { get; set; }

        /// <summary>終了時刻（ステータスが Completed となった時刻</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">タスク名</param>
        /// <param name="weight">処理件数
        /// 同一タスク内で複数回 処理状況を更新したい場合に指定する</param>
        public TaskProgress(string name, int weight = 1)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name));
            }
            if (weight <= 0)
            {
                throw new ArgumentException(nameof(weight));
            }

            Name = name;
            Weight = weight;
        }

        private bool ValidateNewStatus(TaskProgressState state)
        {
            // ToDo:Completed を Outstanding に戻そうとした場合などにエラーとして検知する

            return true;
        }
    }
}

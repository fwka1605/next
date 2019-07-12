using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rac.VOne.Common.DataHandling
{
    /// <summary>
    ///  再帰処理を利用しない Index の組み合わせを返すメソッド
    /// </summary>
    /// <remarks>
    /// http://stackoverflow.com/a/1121912
    /// </remarks>
    public class IndicesCombinationEnumerable
        : IEnumerable<List<int>>
    {
        private readonly IndicesCombinationEnumerator enumerator;
        public IndicesCombinationEnumerable(int itemCount, int selectCount)
        {
            enumerator = new IndicesCombinationEnumerator(itemCount, selectCount);
        }
        public IEnumerator<List<int>> GetEnumerator()
            => enumerator;

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private class IndicesCombinationEnumerator
            : IEnumerator<List<int>>
        {
            private readonly int itemCount;
            private readonly int selectCount;
            internal IndicesCombinationEnumerator(int itemCount, int selectCount)
            {
                this.itemCount = itemCount;
                this.selectCount = selectCount;
            }

            private List<int> indices = new List<int>();
            private int currenctIndex;
            private bool finised;

            private List<int> Current
            { get { return finised ? null : indices; } }

            List<int> IEnumerator<List<int>>.Current
            { get { return Current; } }

            object IEnumerator.Current
            { get { return Current; } }

            private void Dispose()
            {
                indices = null;
            }

            void IDisposable.Dispose()
                => Dispose();

            private bool MoveNext()
            {
                if (!indices.Any())
                {
                    Reset();
                    return true;
                }
                while (-1 < currenctIndex && (!IsFree(indices[currenctIndex] + 1)))
                {
                    currenctIndex--;
                }
                if (currenctIndex == -1)
                {
                    finised = true;
                    return false;
                }
                indices[currenctIndex]++;
                var newindex = indices[currenctIndex] + 1;
                for (var i = currenctIndex + 1; i < indices.Count; i++)
                {
                    indices[i] = newindex;
                    newindex += 1;
                }
                currenctIndex = indices.Count - 1;
                return true;
            }

            bool IEnumerator.MoveNext()
                => MoveNext();

            private bool IsFree(int index)
                => 0 <= index && index < itemCount
                && !indices.Any(x => x == index);

            private void Reset()
            {
                finised = false;
                indices.Clear();
                indices.AddRange(Enumerable.Range(0, selectCount).ToArray());
                currenctIndex = selectCount - 1;
            }

            void IEnumerator.Reset()
                => Reset();
        }
    }
}

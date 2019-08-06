using System;
using System.Collections.Generic;
using System.Text;

namespace AStar
{
    /// <summary>
    /// 二叉堆
    /// @author cary
    /// </summary>
    internal class BinaryHeaps<T>
    {
        private List<T> m_Data;
        internal int Length => m_Data.Count;
        public T[] data => m_Data.ToArray();

        private Func<T, T, int> _compare;

        internal BinaryHeaps(Func<T, T, int> compare)
        {
            m_Data = new List<T>();
            _compare = compare;
        }

        public void enqueue(T obj)
        {
            m_Data.Add(obj);
            int parentIndex = (Length - 2) >> 1;
            int objIndex = Length - 1;
            T temp = m_Data[objIndex];
            while (objIndex > 0)
            {
                if (_compare(temp, m_Data[parentIndex]) > 0)
                {
                    m_Data[objIndex] = m_Data[parentIndex];
                    objIndex = parentIndex;
                    parentIndex = (parentIndex - 1) >> 1;
                }
                else
                {
                    break;
                }
            }
            m_Data[objIndex] = temp;
        }

        public bool modify(T oldObj, T newObj)
        {
            int objIndex = m_Data.IndexOf(oldObj);
            if (objIndex == -1)
            {
                return false;
            }
            m_Data[objIndex] = newObj;
            int parentIndex = (objIndex - 1) >> 1;
            T temp = m_Data[objIndex];
            while (objIndex > 0)
            {
                if (_compare(temp, m_Data[parentIndex]) > 0)
                {
                    m_Data[objIndex] = m_Data[parentIndex];
                    objIndex = parentIndex;
                    parentIndex = (parentIndex - 1) >> 1;
                }
                else
                {
                    break;
                }
            }
            m_Data[objIndex] = temp;
            return true;
        }

        public T dequeue()
        {
            if (Length < 2)
            {
                var tempData = m_Data[0];
                Dispose();
                return tempData;
            }
            T result = m_Data[0];
            m_Data.RemoveAt(0);
            int parentIndex = 0;
            int childIndex = 1;
            T temp = m_Data[parentIndex];
            while (childIndex <= Length - 1)
            {
                if (m_Data.Count > childIndex + 1 && _compare(m_Data[childIndex], m_Data[childIndex + 1]) < 0)
                {
                    childIndex++;
                }
                if (_compare(temp, m_Data[childIndex]) < 0)
                {
                    m_Data[parentIndex] = m_Data[childIndex];
                    parentIndex = childIndex;
                    childIndex = (childIndex << 1) + 1;
                }
                else
                {
                    break;
                }
            }
            m_Data[parentIndex] = temp;
            return result;
        }

        public void Dispose()
        {
            m_Data.Clear();
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_Data.Count; i++)
            {
                sb.Append($",{m_Data[i]}");
            }
            return sb.ToString();
        }
    }
}

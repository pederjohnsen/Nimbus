using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using Nimbus.Utilities;

namespace Nimbus
{
    public class GameList<T> : IList<T>
    {

        private List<T> m_Inner;

        private readonly object m_Lock = new object();
        private bool raiseListChangedEvents;

        public bool RaiseListChangedEvents
        {
            get { return raiseListChangedEvents; }
            set { raiseListChangedEvents = value; }
        }


        public GameList()
        {
            m_Inner = new List<T>();
        }
        // To be actually thread-safe, our collection
        // must be locked on all other operations
        // For example, this is how Add() method should look
        public void Add(T item)
        {
            lock (m_Lock)
                m_Inner.Add(item);
        }


        /// <summary>
        /// Sorts using the default IComparer of T
        /// </summary>
        public void Sort()
        {
            m_Inner.Sort();
        }
        public void Sort(IComparer<T> p_Comparer)
        {
            sort(p_Comparer, null);
        }
        public void Sort(Comparison<T> p_Comparison)
        {
            sort(null, p_Comparison);
        }
        private void sort(IComparer<T> p_Comparer, Comparison<T> p_Comparison)
        {

            //Extract items and sort separately
            List<T> sortList = new List<T>(this);

            if (p_Comparison == null)
            {
                sortList.Sort(p_Comparer);
            }//if
            else
            {
                sortList.Sort(p_Comparison);
            }//else

            //Disable notifications, rebuild, and re-enable notifications

        }



        public int IndexOf(T item)
        {
            return m_Inner.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_Inner.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Inner.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return m_Inner[index];
            }
            set
            {
                m_Inner[index] = value;
            }
        }


        public void Clear()
        {
            m_Inner.Clear();
        }

        public bool Contains(T item)
        {
            return m_Inner.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Inner.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_Inner.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
           return m_Inner.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SafeEnumerator<T>(m_Inner.GetEnumerator(), m_Lock);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new SafeEnumerator<T>(m_Inner.GetEnumerator(), m_Lock);
        }
    }
}
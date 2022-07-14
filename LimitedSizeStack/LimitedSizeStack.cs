using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        private int limit;
        private int count;

        private StackNode<T> head;
        private StackNode<T> tail;

        public LimitedSizeStack(int limit)
        {
            this.limit = limit;
        }

        public void Push(T item)
        {
            if (limit < 1)
                return;

            if (count == limit)
                RemoveLast();

            var node = new StackNode<T> { Value = item, Next = head };
            if (tail is null)
                tail = node;
            else
                head.Previous = node;
            head = node;
            ++count;
        }

        public T Pop()
        {
            if (head is null)
                throw new InvalidOperationException();

            var temp = head;
            RemoveFirst();
            return temp.Value;
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        private void RemoveLast()
        {
            tail = tail.Previous;
            if (tail is null)
                head = null;
            else
                tail.Next = null;
            --count;
        }

        private void RemoveFirst()
        {
            head = head.Next;
            if (head is null)
                tail = null;
            else
                head.Previous = null;
            --count;
        }
    }

    public class StackNode<T>
    {
        public T Value { get; set; }
        public StackNode<T> Previous { get; set; }
        public StackNode<T> Next { get; set; }
    }
}

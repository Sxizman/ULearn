using System;
using System.Linq;
using System.Collections.Generic;

namespace Clones
{
	public class CloneVersionSystem : ICloneVersionSystem
	{
		private List<Clone> clones;

		public CloneVersionSystem()
        {
			clones = new List<Clone> { new Clone() };
        }

		public string Execute(string query)
		{
			var queryWords = query.Split();
			return ExecuteOnClone(
				queryWords[0],
				int.Parse(queryWords[1]) - 1,
				queryWords.Skip(2).ToArray());
		}

		public string ExecuteOnClone(string commandName, int cloneIndex, string[] args)
        {
			switch (commandName)
            {
                case "learn":
					clones[cloneIndex].LearnProgram(int.Parse(args[0]));
					return null;
				case "rollback":
					clones[cloneIndex].RollbackLastProgram();
					return null;
				case "relearn":
					clones[cloneIndex].RelearnLastProgram();
					return null;
				case "clone":
					clones.Add(clones[cloneIndex].Copy());
					return null;
				case "check":
					var program = clones[cloneIndex].CheckLastProgram();
					return program?.ToString() ?? "basic";
			}

			throw new InvalidOperationException();
        }
	}

	public class Clone
    {
		private NodeStack<int> learnedPrograms;
		private NodeStack<int> canceledPrograms;

		public Clone()
        {
			learnedPrograms = new NodeStack<int>();
			canceledPrograms = new NodeStack<int>();
        }

		public void LearnProgram(int program)
        {
			learnedPrograms.Push(program);
			canceledPrograms.Clear();
        }

		public bool RollbackLastProgram()
        {
			if (learnedPrograms.Count == 0)
				return false;

			canceledPrograms.Push(learnedPrograms.Pop());
			return true;
        }

		public bool RelearnLastProgram()
        {
			if (canceledPrograms.Count == 0)
				return false;

			learnedPrograms.Push(canceledPrograms.Pop());
			return true;
		}

		public int? CheckLastProgram()
        {
			if (learnedPrograms.Count == 0)
				return null;

			return learnedPrograms.Peek();
        }

		public Clone Copy()
        {
			Clone newClone = new Clone();
			newClone.learnedPrograms = new NodeStack<int>(learnedPrograms);
			newClone.canceledPrograms = new NodeStack<int>(canceledPrograms);
			return newClone;
        }
    }

	public class NodeStack<T>
    {
		private Node<T> head;
		private int count;

		public NodeStack()
        {

        }

		public NodeStack(NodeStack<T> stack)
        {
			head = stack.head;
			count = stack.count;
        }

		public void Push(T item)
        {
			var node = new Node<T> { Value = item, Next = head };
			if (count != 0)
				head.Previous = node;
			head = node;
			++count;
        }

		public T Pop()
        {
			var value = Peek();

			head = head.Next;
			--count;
			if (count != 0)
				head.Previous = null;

			return value;
        }

		public T Peek()
        {
			if (count == 0)
				throw new InvalidOperationException();

			return head.Value;
        }

		public void Clear()
        {
			head = null;
			count = 0;
        }

		public int Count
        {
            get
            {
				return count;
            }
        }
    }

    public class Node<T>
    {
		public T Value { get; set; }
		public Node<T> Previous { get; set; }
		public Node<T> Next { get; set; }
    }
}

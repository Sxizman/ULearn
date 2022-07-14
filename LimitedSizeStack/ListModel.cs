using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;

        private LimitedSizeStack<IListAction<TItem>> actionsHistory;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            actionsHistory = new LimitedSizeStack<IListAction<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            IListAction<TItem> action = new AddItemAction<TItem>(Items, item);
            actionsHistory.Push(action);
            action.Do();
        }

        public void RemoveItem(int index)
        {
            IListAction<TItem> action = new RemoveItemAction<TItem>(Items, index);
            actionsHistory.Push(action);
            action.Do();
        }

        public bool CanUndo()
        {
            return actionsHistory.Count > 0;
        }

        public void Undo()
        {
            var action = actionsHistory.Pop();
            action.Undo();
        }
    }

    public interface IListAction<TItem>
    {
        void Do();
        void Undo();
    }

    public class AddItemAction<TItem> : IListAction<TItem>
    {
        private List<TItem> list;
        private TItem item;

        public AddItemAction(List<TItem> list, TItem item)
        {
            this.list = list;
            this.item = item;
        }

        public void Do()
        {
            list.Add(item);
        }

        public void Undo()
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    public class RemoveItemAction<TItem> : IListAction<TItem>
    {
        private List<TItem> list;
        private int index;
        private TItem item;

        public RemoveItemAction(List<TItem> list, int index)
        {
            this.list = list;
            this.index = index;
            item = list[index];
        }

        public void Do()
        {
            list.RemoveAt(index);
        }

        public void Undo()
        {
            list.Insert(index, item);
        }
    }
}
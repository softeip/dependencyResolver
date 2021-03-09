using System;
using System.Collections.Generic;

namespace DependencyResolution
{
    public class ItemNode<TItem>
    {
        internal List<ItemNode<TItem>> Dependencies { get; }

        public TItem Item { get; }

        public ItemNode<TItem>[] DependentNodes => Dependencies.ToArray();

        public ItemNode(TItem item)
        {
            Item = item;
            Dependencies = new List<ItemNode<TItem>>();
        }

        public ItemNode(TItem item, params ItemNode<TItem>[] dependencies) : this(item)
        {
            Dependencies.AddRange(dependencies);
        }

        public void IsDependentOn(ItemNode<TItem> dependency)
        {
            if (Dependencies.Contains(dependency))
                return;

            Dependencies.Add(dependency);
        }

        public override string ToString()
        {
            return $"Node: {Item}";
        }
    }
}

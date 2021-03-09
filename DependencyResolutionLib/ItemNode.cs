using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyResolution
{
    public class ItemNode<TItem>
    {
        protected internal virtual List<ItemNode<TItem>> Dependencies { get; }

        public TItem Item { get; }

        public ItemNode(TItem item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            Dependencies = new List<ItemNode<TItem>>();
        }

        public ItemNode(TItem item, params ItemNode<TItem>[] dependencies) : this(item)
        {
            if (dependencies.Any(x => x == null))
                throw new ArgumentNullException(nameof(dependencies) + " contains null");

            Dependencies.AddRange(dependencies);
        }

        public void IsDependentOn(ItemNode<TItem> dependency)
        {
            if (dependency == null)
                throw new ArgumentNullException(nameof(dependency));

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

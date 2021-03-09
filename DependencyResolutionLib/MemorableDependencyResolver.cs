using System.Collections.Generic;
using System.Linq;

namespace DependencyResolution
{
    public class MemorableDependencyResolver<TItem> : DependencyResolver<TItem>
    {
        private readonly List<ItemNode<TItem>> _resolved;

        public MemorableDependencyResolver()
        {
            _resolved = new List<ItemNode<TItem>>();
        }

        public void MarkAsResolved(ItemNode<TItem> node)
        {
            if (_resolved.Contains(node))
                return;

            _resolved.Add(node);
        }

        public void ClearResolved()
        {
            _resolved.Clear();
        }

        public override IEnumerable<ItemNode<TItem>> GetResolved(IEnumerable<ItemNode<TItem>> nodes)
        {
            var preresolved = _resolved.ToArray();
            var result = base.GetResolved(nodes);
            return result.Except(preresolved);
        }

        protected override IList<ItemNode<TItem>> GetResolvedList()
        {
            return _resolved;
        }
    }
}

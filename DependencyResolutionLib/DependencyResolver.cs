using System.Collections.Generic;
using System.Linq;

namespace DependencyResolution
{
    public class DependencyResolver
    {
        public DependencyResolver()
        {
        }

        public IEnumerable<ItemNode<TItem>> GetResolved<TItem>(IEnumerable<ItemNode<TItem>> nodes)
        {
            var resolved = new List<ItemNode<TItem>>();
            var unresolved = new List<ItemNode<TItem>>();

            foreach (var node in nodes)
            {
                Resolve(node, resolved, unresolved);
            }

            return resolved;
        }

        public IEnumerable<ItemNode<TItem>> GetResolved<TItem>(ItemNode<TItem> node)
        {
            var resolved = new List<ItemNode<TItem>>();
            var unresolved = new List<ItemNode<TItem>>();

            Resolve(node, resolved, unresolved);

            return resolved;
        }

        private void Resolve<TItem>(ItemNode<TItem> node, IList<ItemNode<TItem>> resolved, IList<ItemNode<TItem>> unresolved)
        {
            if (resolved.Contains(node))
                return;

            unresolved.Add(node);

            foreach (var dependency in node.Dependencies)
            {
                if (!resolved.Contains(dependency))
                {
                    if (unresolved.Contains(dependency))
                        throw new CircularReferenceException($"Circular reference detected: {node} -> {dependency}");

                    Resolve(dependency, resolved, unresolved);
                }
            }

            resolved.Add(node);
            unresolved.Remove(node);
        }
    }
}

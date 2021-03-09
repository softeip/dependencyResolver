﻿using System.Collections.Generic;

namespace DependencyResolution
{
    public class DependencyResolver<TItem>
    {
        /// <summary>
        /// Uses Dependency resolution algorithm
        /// Goes through node's dependencies recursively and builds an order
        /// in which nodes or their items should be resolved
        /// </summary>
        /// <param name="node">A node to resolve</param>
        /// <returns>Order in which nodes or their items should be resolved</returns>
        public IEnumerable<ItemNode<TItem>> GetResolved(ItemNode<TItem> node)
        {
            return GetResolved(new[] { node });
        }

        /// <summary>
        /// Uses Dependency resolution algorithm
        /// Goes through each node's dependencies recursively and builds an order
        /// in which nodes or their items should be resolved
        /// </summary>
        /// <param name="nodes">The nodes to resolve</param>
        /// <returns>Order in which nodes or their items should be resolved</returns>
        public virtual IEnumerable<ItemNode<TItem>> GetResolved(IEnumerable<ItemNode<TItem>> nodes)
        {
            var resolved = GetResolvedList();
            var unresolved = new List<ItemNode<TItem>>();

            foreach (var node in nodes)
            {
                Resolve(node, resolved, unresolved);
            }

            return resolved;
        }

        /// <summary>
        /// Prepare a list which will be used by algorithm to add resolved nodes
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ItemNode<TItem>> GetResolvedList()
        {
            return new List<ItemNode<TItem>>();
        }

        protected void Resolve(ItemNode<TItem> node, IList<ItemNode<TItem>> resolved, IList<ItemNode<TItem>> unresolved)
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

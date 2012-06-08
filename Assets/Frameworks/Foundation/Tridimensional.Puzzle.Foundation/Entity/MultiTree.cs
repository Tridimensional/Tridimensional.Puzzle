using System.Collections.Generic;

namespace Tridimensional.Puzzle.Foundation.Entity
{
    public class MultiTree<T>
	{
        public T Value { get; set; }
        public MultiTree<T> Parent { get; set; }
        public List<MultiTree<T>> Children { get; set; }

        public void Append(MultiTree<T> node)
        {
            if (Children == null)
            {
                Children = new List<MultiTree<T>>();
            }

            Children.Add(node);
        }
	}
}

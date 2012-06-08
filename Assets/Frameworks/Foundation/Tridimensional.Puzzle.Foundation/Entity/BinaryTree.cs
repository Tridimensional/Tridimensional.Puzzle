using System;

namespace Tridimensional.Puzzle.Foundation.Entity
{
    public class BinaryTree<T>
    {
        public T Value { get; set; }
        public BinaryTree<T> Parent { get; set; }
        public BinaryTree<T> LeftChild { get; set; }
        public BinaryTree<T> RightChild { get; set; }


    }
}

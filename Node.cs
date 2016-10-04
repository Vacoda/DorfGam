using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class Node<T>
    {
        public T data;

        public Edge<T>[] Edges; //nodes leading OUT from this node;

    }
}

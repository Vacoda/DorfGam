using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class Edge<T>
    {
        public float cost; //To transverse along this edge. (Cost to ENTER the reference node)
        public Node<T> node;
    }
}

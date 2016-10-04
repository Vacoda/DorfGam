using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class TileGraph
    {
        // Constructs a path-finding compatible graph of a world. Each tile is a node, each node has edges to adjacent nodes for all legal paths.
        public Dictionary<Tile, Node<Tile>> nodes;

        public TileGraph(World world)
        {
            //For each tile in the world create a node. 
            //do we create nodes for non-transversible tiles? nope;
            nodes = new Dictionary<Tile, Node<Tile>>();
            foreach(Tile t in world.Tiles)
            {
                if(t.Type != TileType.Empty)
                {
                    Node<Tile> n = new Node<Tile>();
                    n.data = t;
                    nodes.Add(t, n);
                }
            }

            foreach(Tile t in nodes.Keys)
            {
                List<Edge<Tile>> edges = new List<Edge<Tile>>();
                Node<Tile> n = nodes[t];
                //get a list of legal neighbours
                Tile[] neighbours = t.GetNeighbours(true); // some will be null;

                // if neighbour is walkable add an edge to the appropriate node
                for(int i = 0; i < neighbours.Length; i++)
                {
                    if(neighbours[i] != null)
                    {
                        Edge<Tile> e = new Edge<Tile>();
                        e.cost = neighbours[i].movecost;
                        e.node = nodes[neighbours[i]];
                        edges.Add(e);
                    }
                }
                n.Edges = edges.ToArray();

            }

            //Loop through all nodes to create the edges.

        }
    }
}

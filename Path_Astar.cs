using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Priority_Queue;
using System.Threading.Tasks;

namespace Game2
{
    class Path_Astar
    {
        Queue<Tile> path;

        public Path_Astar(World world, Tile start, Tile end)
        {
            path = new Queue<Tile>();

            if(world.TileGraph == null)
            {
                System.Diagnostics.Debug.WriteLine("We need a new tilegraph");
                world.TileGraph = new TileGraph(world);
            }

            Dictionary<Tile, Node<Tile>> nodes = world.TileGraph.nodes;

            Node<Tile> beg = nodes[start];
            Node<Tile> fin = nodes[end];

            if(nodes.ContainsKey(start) == false)
            {
                throw new Exception("We don't have this starting tile in our list of nodes");
            }

            if (nodes.ContainsKey(end) == false)
            {
                throw new Exception("Target node is inaccesible");
            } 

            List<Node<Tile>> ClosedSet = new List<Node<Tile>>();
            /*            List<Node<Tile>> OpenSet = new List<Node<Tile>>();
                         OpenSet.Add( nodes[start]);
                          */
            SimplePriorityQueue<Node<Tile>> OpenSet = new SimplePriorityQueue<Node<Tile>>();
            OpenSet.Enqueue(nodes[start], 0);


            Dictionary<Node<Tile>, Node<Tile>> Came_From = new Dictionary<Node<Tile>, Node<Tile>>();

            Dictionary<Node<Tile>, float> g_score = new Dictionary<Node<Tile>, float>();
            foreach(Node<Tile> n in nodes.Values)
            {
                g_score[n] = 80000000f; //'infinity' = 80m
            }
            g_score[nodes[start]] = 0;

            Dictionary<Node<Tile>, float> f_score = new Dictionary<Node<Tile>, float>();
            foreach (Node<Tile> n in nodes.Values)
            {
                f_score[n] = 80000000f; //'infinity' = 80m
            }
            f_score[nodes[start]] = Heuristic_cost_Estimate(beg, fin);

            while(OpenSet.Count > 0)
            {
                Node<Tile> Current = OpenSet.Dequeue(); //Node with LOWEST fscore
                if (Current == fin)
                {
                    //Reconstruct path
                    Reconstructpath(Came_From, Current);
                    path.Enqueue(end);
                    return;
                }

                ClosedSet.Add(Current);

                foreach(Edge<Tile> edge_neighbour in Current.Edges)
                {
                    Node<Tile> neighbour = edge_neighbour.node;
                    if(ClosedSet.Contains(neighbour) == true)
                    {
                        continue;
                    }

                    float temp_g_score = g_score[Current] + dist_between(Current, neighbour);

                    if (OpenSet.Contains(neighbour) && temp_g_score >= g_score[neighbour])
                    {
                        continue;
                    }

                    Came_From[neighbour] = Current;
                    g_score[neighbour] = temp_g_score;
                    f_score[neighbour] = g_score[neighbour] + Heuristic_cost_Estimate(neighbour, fin);

                    if(OpenSet.Contains(neighbour) == false)
                    {
                        OpenSet.Enqueue(neighbour, f_score[neighbour]);
                    }
                    else
                    {
                        OpenSet.UpdatePriority(neighbour, f_score[neighbour]);
                    }
                }
            }

            //if we get here we've done the entire openset and never found the goal.
            // aka no valid path
            throw new Exception("no legal path");

        }

        void Reconstructpath(
            Dictionary<Node<Tile>, Node<Tile>> Came_From,
            Node<Tile> current)
        {
            // If we call this we know we're at the end. We want to make 'came_from' backwards.
            Queue<Tile> Total_Path;
            Total_Path = new Queue<Tile>();
            while(Came_From.ContainsKey(current) )
            {
                //camefrom is a map where key => value relation is 
                // some_node => previous_node
                current = Came_From[current];
                Total_Path.Enqueue(current.data);
            }

            //At this point total_path is a queue from end to start backwards
            
            path = new Queue<Tile>(Total_Path.Reverse());
        }

        float dist_between(Node<Tile> a, Node<Tile> b)
        {
            if (Math.Abs(a.data.X - b.data.X ) + Math.Abs(a.data.Y - b.data.Y) == 1)
            {
                return 1f;
            }
            if (Math.Abs(a.data.X - b.data.X) == 1 && Math.Abs(a.data.Y - b.data.Y) == 1)
            {
                return 1.414f;
            }

            return (float)(Math.Sqrt(Math.Pow(a.data.X - b.data.X, 2f) + Math.Pow(a.data.Y - b.data.Y, 2f)));

        }

        float Heuristic_cost_Estimate( Node<Tile> start, Node<Tile> fin)
        {
            //euclidian dist, aka hypot
            return (float)(Math.Sqrt(Math.Pow(start.data.X - fin.data.X, 2f) + Math.Pow(start.data.Y - fin.data.Y, 2f)));
        }

        public Tile Dequeue()
        {
            if(path.Count > 0 )
            {
                return path.Dequeue();
            }
            return null;
        }

        public int Length()
        {
            if(path == null)
            {
                return 0;
            } 
            else
            {
                return path.Count;
            }
        }
    }
}

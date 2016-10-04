using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game2
{
    enum TileType { Empty, Grass, Floor}
    class Tile
    {

        public World world;
        public int X;
        public int Y;
        public int Z;

        string[] Grass_Graphics = new string[5] { "'", ";", "_", "''", "~" };
        public string TileGraphic;

        public StationaryObject stationaryObject;
        public List<LooseObject> Inventory;
        public List<Creature> Creatures;
        public float movecost = 1;
        public bool Selected = false;

        public Job PendingJob;

        Brush[] Grass_FGs = new Brush[4] { Brushes.Green, Brushes.Olive, Brushes.ForestGreen, Brushes.LightGreen };
        public Brush FG;
        public Brush BG = Brushes.Black;

        public void DestroyStationaryObject()
        {
            this.Inventory.Add(stationaryObject.Harvest());
            this.stationaryObject = null;
        }

        public void SetJob(Job j)
        {
            this.PendingJob = j;
        }

        public Tile[] GetNeighbours(bool diagokay = false)
        {
            Tile[] output;
            if(diagokay == true)
            {
                output = new Tile[8];
            }
            else
            {
                output = new Tile[4];
            }

            output[0] = world.GetTile(X, Y + 1, this.Z);
            output[1] = world.GetTile(X + 1, Y, this.Z);
            output[2] = world.GetTile(X, Y - 1, this.Z);
            output[3] = world.GetTile(X - 1, Y, this.Z);
            if(diagokay == true)
            {
                output[4] = world.GetTile(X + 1, Y + 1, this.Z);
                output[5] = world.GetTile(X + 1, Y - 1, this.Z);
                output[6] = world.GetTile(X - 1, Y - 1, this.Z);
                output[7] = world.GetTile(X - 1, Y + 1, this.Z);
            }

            return output;

        }

        public TileType Type;

        public Tile( World world, int x, int y, int z)
        {
            this.world = world;
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.Inventory = new List<LooseObject>();
            this.Creatures = new List<Creature>();
            this.Type = TileType.Grass;
            Random RNG = new Random(GetHashCode());
            switch(this.Type)
            {
                case TileType.Grass:
                    this.TileGraphic = Grass_Graphics[RNG.Next(0, 5)];
                    this.FG = Grass_FGs[RNG.Next(0, 4)];
                    if(RNG.Next(0,10) > 9)
                    {
                        this.stationaryObject = new StationaryObject("T", ObjectType.Tree, world.Mats[RNG.Next(0, world.Mats.Count)]);
                    }
                    break;
            };

        }
    }
}

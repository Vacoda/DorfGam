using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class WorldInventory
    {
        // Our world
        World world;
        // A list for COMS of each TYPE of material
        List<LooseObject> Organic_Wood_Coms = new List<LooseObject>();

        // a list of OBJECTS of each TYPE of material
        List<LooseObject> Organic_Wood_Objects = new List<LooseObject>();

        public void AddComm(MatType mt, LooseObject lo)
        {
            switch(mt)
            {
                case MatType.Organic_Wood:
                    Organic_Wood_Coms.Add(lo);
                    break;
            }
        }

        public WorldInventory(World world)
        {
            this.world = world;
        }
    }
}

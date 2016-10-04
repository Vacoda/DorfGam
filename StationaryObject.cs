using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public enum ObjectType { Tree, Wall, Structure }
    class StationaryObject
    {
        // These are structures and anything which can be 'WORKED' via a designation.
        public ObjectType Type;
        public string ObjectGraphic;
        public Material Mat;

        Action<StationaryObject> ObjectHarvestcb;



        // HARVEST
        public LooseObject Harvest()
        {
            switch (this.Type)
            {
                case ObjectType.Tree:
                    return new LooseObject("=", 20, LObjectType.Raw_Log, this.Mat);
                default:
                    return null;
            }

        }
        // HARVEST CB HOOKS

        public void RegisterOnHarvestcb(Action<StationaryObject> cb)
        {
            ObjectHarvestcb += cb;
        }

        // CONSTRUCTORS
        public StationaryObject(string og, ObjectType ot, Material mat)
        {
            this.Type = ot;
            this.Mat = mat;
            this.ObjectGraphic = og;
        }
    }
}

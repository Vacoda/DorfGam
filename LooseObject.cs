using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public enum LObjectType { Raw_Log }
    class LooseObject
    {
        public LObjectType Type;
        public Material Mat;
        public string ObjectGraphic;
        public int Mass;


        public LooseObject(string og, int m, LObjectType ot, Material mat)
        {
            this.Type = ot;
            this.Mat = mat;
            this.Mass = m;
            this.ObjectGraphic = og;
        }

    }
}

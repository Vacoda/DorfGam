using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public enum MatType { Organic_Wood, Inorganic_Metal }
    class Material
    {
        public MatType Type;
        public System.Drawing.Brush FG;
        float Density;
        int Hardness;
        int Flexibility;
        string Name;

        // All params
        public Material (string name,System.Drawing.Brush fg, float d, int h, int f)
        {
            this.Name = name;
            this.Flexibility = f;
            this.Hardness = h;
            this.FG = fg;
            this.Density = d;
        }
        // flexibility f(h)
        public Material(string name, System.Drawing.Brush fg, float d, int h, MatType mt)
        {
            this.Name = name;
            this.Type = mt;
            this.Hardness = h;
            this.Flexibility = 1 - h / 1000;
            this.FG = fg;
            this.Density = d;
        }
    }
}

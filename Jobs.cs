using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class Job
    {
        // Holds information for a job at (xyz);
        public Tile tile;
        int Duration = 5;

        // Callbacks
        Action<Job> cbJobComplete;
        Action<Job> cbJobCancel;

        public Job (Tile tyle, Action<Job> cbJobComplete)
        {
            this.tile = tyle;
            this.cbJobComplete += cbJobComplete;
        }

        public void RegisterJobCompleteCallBack(Action<Job> cb)
        {
            cbJobComplete += cb;
        }

        public void RegisterJobCancelCallBack(Action<Job> cb)
        {
            cbJobCancel += cb;
        }


        public void Work()
        {
            this.Duration -= 1;
            if(this.Duration <= 0)
            {
                cbJobComplete(this);
            }
        }
    }
}

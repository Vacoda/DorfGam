using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class Job_Queue
    {
        public Queue<Job> Queue;
        public int Count
        {
            get
            {
                return Queue.Count;
            }
        }
        public Job_Queue ()
        {
            Queue = new Queue<Job>();
        }

        public void Enqueue(Job j)
        {
            Queue.Enqueue(j);
        }

        public Job Dequeue()
        {
            if(Queue.Count > 0)
            {
                return Queue.Dequeue();
            }
            else
            {
                return null;
            }
        }


    }
}

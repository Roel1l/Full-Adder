using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_Adder.Nodes
{
    class PROBE : INode
    {
        public List<int> input { get; set; }
        public int output { get; set; }

        public List<INode> prevNodes { get; set; }

        public List<INode> nextNodes { get; set; }

        public PROBE()
        {
            input = new List<int>();
            output = -1;
            prevNodes = new List<INode>();
            nextNodes = new List<INode>();
        }

        public void calculateOutput()
        {
            int i = input.Sum();
            if (i > 1 || i < 0)
            {
                output = -1;
            }
            else
            {
                output = i;
            }
        }
    }
}

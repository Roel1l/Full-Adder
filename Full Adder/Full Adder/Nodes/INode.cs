using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Full_Adder.Nodes
{
    interface INode
    {
        List<int> input { get; set; }
        int output { get; set; }
        List<INode> prevNodes { get; set; }

        List<INode> nextNodes { get; set; }

        void calculateOutput();
    }
}

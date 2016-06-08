using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_Adder.Nodes
{
    class NAND : INode
    {
        public List<int> input { get; set; }
        public int output { get; set; }

        public List<INode> prevNodes { get; set; }

        public List<INode> nextNodes { get; set; }

        public NAND()
        {
            input = new List<int>();
            output = -1;
            prevNodes = new List<INode>();
            nextNodes = new List<INode>();
        }

        public void calculateOutput()
        {
            setIn();
            if (input.Count == prevNodes.Count)
            {
                int i = input.Sum();
                switch (i)
                {
                    case 0:
                        output = 1;
                        break;
                    case 1:
                        output = 1;
                        break;
                    case 2:
                        output = 0;
                        break;
                    default:
                        output = -1;
                        break;

                }
            }
            else
            {
                output = -1;
            }
        }
        public void setIn()
        {
            if (input.Count != prevNodes.Count)
            {
                input.Clear();
                foreach (var node in prevNodes)
                {
                    input.Add(node.getOutput());
                }
            }
        }

        public int getOutput()
        {
            if (output == -1)
            {
                calculateOutput();
            }
            return output;
        }

        public Boolean validate()
        {
            if (prevNodes.Count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

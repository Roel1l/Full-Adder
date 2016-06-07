﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_Adder.Nodes
{
    class NOT : INode
    {
        public List<int> input { get; set; }
        public int output { get; set; }


        public List<INode> prevNodes { get; set; }

        public List<INode> nextNodes { get; set; }

        public NOT()
        {
            input = new List<int>();
            output = -1;
            prevNodes = new List<INode>();
            nextNodes = new List<INode>();
        }

        public void calculateOutput()
        {
            if (input.Count == prevNodes.Count)
            {
                int i = input.Sum();
                switch (i)
                {
                    case 0:
                        output = 1;
                        break;
                    case 1:
                        output = 0;
                        break;
                    case 2:
                        Console.WriteLine("ERROR: NOT poort getting multiple inputs!");
                        output = -1;
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
    }
}

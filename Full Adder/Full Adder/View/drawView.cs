using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Full_Adder.Nodes;

namespace Full_Adder
{
    class DrawView
    {
        public void draw(Dictionary<string, INode>  _nodeDictionary)
        {
            foreach (var node in _nodeDictionary)
            {
                node.Value.calculateOutput();
                Boolean mInput = false;

                Console.WriteLine("------------------------------------");
                Console.WriteLine(node.Key + "     " + node.Value.GetType());
                Console.Write("Input(s): ");
                foreach (int i in node.Value.input)
                {
                    Console.Write(i);
                    if (node.Value.input.Count > 1 && !mInput)
                    {
                        Console.Write(" + ");
                        mInput = true;
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Output: " + node.Value.output);
            }
            Console.ReadLine();

        }
    }
}

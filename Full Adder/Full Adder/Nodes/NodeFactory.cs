using Full_Adder.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_Adder
{
    class NodeFactory
    {
        private Dictionary<string, INode> _nodes = new Dictionary<string,INode>();

        public void addNode(string name, INode node)
        {
                _nodes[name] = node;
        }

        public INode createNode(string name)
        {
            INode node = name.Contains("INPUT") ? _nodes["INPUT"] : _nodes[name];
            INode newNode = (INode)Activator.CreateInstance(node.GetType());
            return newNode;
        }
    }
}

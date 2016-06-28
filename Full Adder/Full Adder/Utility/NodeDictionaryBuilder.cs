using Full_Adder.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_Adder.Utility
{
    class NodeDictionaryBuilder
    {
        private NodeFactory _factory;
        private Dictionary<string, INode> _nodeDictionary;

        public NodeDictionaryBuilder()
        {
            _factory = new NodeFactory();
            _nodeDictionary = new Dictionary<string, INode>();
            addINodesTypesToFactory();
        }

        public void addNodes(Dictionary<string, string> nodes)
        {
            _nodeDictionary = new Dictionary<string, INode>();

            foreach (var i in nodes)
            {
                string nodeType = i.Value.Contains("INPUT") ? "INPUT" : i.Value;
                _nodeDictionary.Add(i.Key, _factory.createNode(nodeType));
            }
        }

        public void addEdges(Dictionary<string, string> edges)
        {
            foreach (var i in edges)
            {
                string[] s = i.Value.Split(',');
                List<INode> list = new List<INode>();
                foreach (var x in s)
                {
                    list.Add(_nodeDictionary[x]);
                    _nodeDictionary[x].prevNodes.Add(_nodeDictionary[i.Key]);
                }
                _nodeDictionary[i.Key].nextNodes = list;
            }
        }

        public Dictionary<string, INode> getNodeDictionary()
        {
            return _nodeDictionary;
        }
        private void addINodesTypesToFactory()
        {
            //Looks up all available INode types in the current running assembly and adds these as possible nodes to the factory
            var type = typeof(INode);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            var x = types.ToList();


            foreach (var i in x)
            {
                string s = i.FullName.Replace("Full_Adder.Nodes.", "");

                if (s != "INode")
                {
                    INode node = (INode)Activator.CreateInstance(i.UnderlyingSystemType);
                    _factory.addNode(s, node);
                }
            }
        }
    }
}

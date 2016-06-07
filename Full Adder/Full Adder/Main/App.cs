using Full_Adder;
using Full_Adder.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

public class App
{
        private NodeFactory _factory = new NodeFactory();
        private Dictionary<string, string> _nodes;
        private Dictionary<string, string> _edges;
        private Dictionary<string, INode> _nodeDictionary;

        public App(){
            FileReader reader = new FileReader();
            _nodes = reader.getNodes();
            _edges = reader.getEdges();
            
            createINodeClasses();
            fillNodeDictionary();
            setEdges();
            getInputsReady();
            //run();
        }

        private void createINodeClasses()
        {
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
       
        private void fillNodeDictionary()
        {
            _nodeDictionary = new Dictionary<string, INode>();

            foreach(var i in _nodes){
                _nodeDictionary.Add(i.Key, _factory.createNode(i.Value));
            }
            
        }

        private void setEdges()
        {
            foreach (var i in _edges)
            {
                string[] s = i.Value.Split(',');
                List<INode> list = new List<INode>();
                foreach(var x in s){
                    list.Add(_nodeDictionary[x]);
                    _nodeDictionary[x].prevNodes.Add(_nodeDictionary[i.Key]);
                }
                _nodeDictionary[i.Key].nextNodes = list;
            }
        }

        private void getInputsReady()
        {
            List<string> inputList = new List<string>();
            foreach (var i in _nodes)
            {
                //get input ready
                if (i.Value == "INPUT_HIGH")
                {
                    List<int> l = new List<int>();
                    l.Add(1);
                    _nodeDictionary[i.Key].input = l;
                    _nodeDictionary[i.Key].calculateOutput();
                    inputList.Add(i.Key);
                }
                else if(i.Value =="INPUT_LOW")
                {
                    List<int> l = new List<int>();
                    l.Add(0);
                    _nodeDictionary[i.Key].input = l;
                    _nodeDictionary[i.Key].calculateOutput();
                    inputList.Add(i.Key);
                }
            }

            //run
            foreach(string input in inputList)
            {
                nextOutput(_nodeDictionary[input]);
            }

            //write to console
            foreach (var i in _nodes)
            {
                if (i.Value == "PROBE")
                {
                    string one = i.Key;
                    string two = _nodeDictionary[i.Key].output.ToString();
                    string three = string.Format("{0} ended with value: {1}", one, two);
                    Console.WriteLine(three);

                }
            }
            Console.ReadLine();
        }

        private void nextOutput(INode node)
        {
            node.calculateOutput();
            foreach (INode nextNode in node.nextNodes)
            {
                nextNode.input.Add(node.output);
                nextNode.calculateOutput();
                if(nextNode.output == 0 || nextNode.output == 1){
                    nextOutput(nextNode);
                }
            }

            Console.WriteLine(node.GetType().ToString());
            Console.WriteLine(node.output);
        }

        
}

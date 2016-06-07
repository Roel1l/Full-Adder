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
        run();
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
            else if (i.Value == "INPUT_LOW")
            {
                List<int> l = new List<int>();
                l.Add(0);
                _nodeDictionary[i.Key].input = l;
                _nodeDictionary[i.Key].calculateOutput();
                inputList.Add(i.Key);
            }
        }
    }
    private void run()
    {
        int nodeCount = -4;
        foreach(var node in _nodeDictionary){
            node.Value.calculateOutput();
            Boolean mInput = false;


            Console.WriteLine("------------------------------------");
            Console.WriteLine("NODE" + nodeCount + "     " + node.Value.GetType());
            Console.Write("Input(s): ");
            foreach(int i in node.Value.input){
                Console.Write(i);
                if(node.Value.input.Count > 1 && !mInput){
                    Console.Write(" + ");
                    mInput = true;
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Output: " + node.Value.output);
            nodeCount++;
        }
        Console.ReadLine();

    }

}

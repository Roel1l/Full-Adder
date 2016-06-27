using Full_Adder;
using Full_Adder.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

public class App
{
    private NodeFactory _factory;
    private Dictionary<string, string> _nodes;
    private Dictionary<string, string> _edges;
    private Dictionary<string, INode> _nodeDictionary;
    private FileReader _reader;
    private DrawView _view;

    public App(){
            _reader = new FileReader();
            _factory = new NodeFactory();
            _view = new DrawView();
    }
    public void run()
    {
        _reader.chooseFile();
        while (true)
        {
            _reader.readFile();

            _nodes = _reader.getNodes();
            _edges = _reader.getEdges();

            createINodeClasses();
            createNodes();
            setEdges();
            getInputsReady();
            validateNodes();

            //Give te user the opportunity to change the default inputs
            _nodeDictionary = _reader.setInputs(_nodeDictionary);

            foreach (var node in _nodeDictionary)
            {
                    node.Value.calculateOutput();
            }
            _view.draw(_nodeDictionary);
            _view.writeEnd();
        }
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
       
    private void createNodes()
    {
        _nodeDictionary = new Dictionary<string, INode>();

        foreach(var i in _nodes){
            string nodeType = i.Value.Contains("INPUT") ? "INPUT" : i.Value;
            _nodeDictionary.Add(i.Key, _factory.createNode(nodeType));
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

    private void validateNodes() { 
        foreach(var node in _nodeDictionary){
            if(!node.Value.validate()){
                Console.WriteLine("Error in file");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }

}

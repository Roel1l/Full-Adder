using Full_Adder;
using Full_Adder.Nodes;
using Full_Adder.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

public class App
{

    private Dictionary<string, INode> _nodeDictionary;
    private FileReader _reader;
    private DrawView _view;

    public App(){
            _reader = new FileReader();
            _view = new DrawView();
    }
    public void run()
    {
        _reader.chooseFile();
        while (true)
        {
            _reader.readFile();

            //Get a dictionary with the actual INode classes in it
            NodeDictionaryBuilder builder = new NodeDictionaryBuilder();
            builder.addNodes(_reader.getNodes());
            builder.addEdges(_reader.getEdges());
            _nodeDictionary = builder.getNodeDictionary();

            getInputsReady();
            validateNodes();

            //Give te user the opportunity to change the default inputs
            _nodeDictionary = _reader.setInputs(_nodeDictionary);

            //Run the simulation, calculate each node output
            foreach (var node in _nodeDictionary)
            {
                node.Value.calculateOutput();
            }

            //Draw the simulation
            _view.draw(_nodeDictionary);
            _view.writeEnd();
        }
    }

    private void getInputsReady()
    {
        foreach (var i in _reader.getNodes())
        {
            if (i.Value == "INPUT_HIGH")
            {
                List<int> l = new List<int>();
                l.Add(1);
                _nodeDictionary[i.Key].input = l;
                _nodeDictionary[i.Key].calculateOutput();
            }
            else if (i.Value == "INPUT_LOW")
            {
                List<int> l = new List<int>();
                l.Add(0);
                _nodeDictionary[i.Key].input = l;
                _nodeDictionary[i.Key].calculateOutput();
            }
        }
    }

    private void validateNodes() {
        //validates each node using its own validate method which checks if its configured correctly
        foreach(var node in _nodeDictionary){
            if(!node.Value.validate()){
                Console.WriteLine("Error in file");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }

}

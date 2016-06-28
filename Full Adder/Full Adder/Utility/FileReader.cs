using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Full_Adder.Nodes;

namespace Full_Adder
{
    class FileReader
    {
        private string _filecontents;
        private Dictionary<string, string> _nodes = new Dictionary<string, string>();
        private Dictionary<string, string> _edges = new Dictionary<string, string>();
        private int counter = 0;
        public void readFile()
        {
            try
            {
                _nodes.Clear();
                _edges.Clear();
                createNodesAndEdges();
                validateEdges();

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid file");
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public Dictionary<string, string> getNodes()
        {
            return _nodes;
        }

        private void validateEdges()
        {
            //Check if edges dont have some sort of infinite loop in them
            Dictionary<string, string[]> d = new Dictionary<string, string[]>();

            foreach (var i in _edges)
            {
                string[] s = i.Value.Split(',');
                d.Add(i.Key, s);
            }

            foreach (var i in d)
            {
                    foreach (string s in d[i.Key])
                    {
                        counter = 0;
                        checkForLoops(i.Key, s, d);
                    }          
            }

        }
        
        private void checkForLoops(string current, string next, Dictionary<string, string[]> d)
        {
            if (!d.ContainsKey(next))
            {
                //dealing with a probe which is not present as key in the edges dictionary but this also means this path has an end and doesnt loop forever
                return;
            }

            foreach (string s in d[next])
            {
                //when counter is +- 9000 StackOverFlowException will be thrown. This is prevented by ending the loop early and throwing our own exception
                counter++;
                if (s.Equals(current) || counter > 5000)
                {
                    Console.WriteLine("Infinite loop in file");
                    Console.ReadLine();
                    System.Environment.Exit(0);
                }
                else
                {
                    checkForLoops(current, s, d);
                }
            }                 
        }
     
        public void validateFile()
        {
            int inputCount = 0;
            int probeCount = 0;
            foreach(var node in _nodes){
                if (node.Value == "INPUT_HIGH" || node.Value == "INPUT_LOW")
                {
                    inputCount++;
                }
                else if(node.Value == "PROBE")
                {
                    probeCount++;
                }
            }
            if(inputCount != 3 || probeCount != 2){
                Console.WriteLine("Error in file");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public Dictionary<string,string> getEdges()
        {
            return _edges;
        }
        public void chooseFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _filecontents = File.ReadAllText(dialog.FileName);
            }
        }
        private void createNodesAndEdges()
        {
            string[] allLines = _filecontents.Split('\n');
            foreach (string line in allLines)
            {
                string i = line.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(";", "");
                if (i != string.Empty)
                {
                    if (i.ToCharArray()[0] != '#')
                    {
                            string[] s = i.Split(':');
                            if (_nodes.ContainsKey(s[0]))
                            {
                                //dealing with an edge
                                _edges.Add(s[0], s[1]);
                            }
                            else
                            {
                                //dealing with a node
                                _nodes.Add(s[0], s[1]);
                            }
                        
                    }
                }
            }
        }
        public Dictionary<string, INode> setInputs(Dictionary<string, INode> _nodeDictionary)
        {
            //This method gives the user the possibility to change the default inputs set in the file
            foreach (var node in _nodeDictionary)
            {
                if (node.Value.GetType().ToString() == "Full_Adder.Nodes.INPUT")
                {
                    string _val = "";
                    Console.Write("Enter input(default " + node.Value.input.First() + "): ");
                    ConsoleKeyInfo key = new ConsoleKeyInfo();

                    while (key.Key != ConsoleKey.Enter){
                        key = Console.ReadKey(true);
                        if (key.KeyChar.ToString() == "1" || key.KeyChar.ToString() == "0")
                        {
                            double val = 0;
                            bool _x = double.TryParse(key.KeyChar.ToString(), out val);
                            if (_x && _val.Length < 1)
                            {
                                _val += key.KeyChar;
                                Console.Write(key.KeyChar);
                            }
                        }
                        else
                        {
                            if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                            {
                                _val = _val.Substring(0, (_val.Length - 1));
                                Console.Write("\b \b");
                            }
                        }
                    }
                  

                    Console.WriteLine();
                    if (_val.Length > 0)
                    {
                        List<int> inputVal = new List<int>();
                        inputVal.Add(int.Parse(_val));
                        node.Value.input = inputVal;
                        Console.WriteLine("Input set to: " + _val);
                    }
                    else
                    {
                        Console.WriteLine("Input unchanged");
                    }
                    Console.WriteLine();
                }
            }
            return _nodeDictionary;
        }
    }
}

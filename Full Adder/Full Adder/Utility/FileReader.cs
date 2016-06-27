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
        string _filecontents;
        Dictionary<string, string> _nodes = new Dictionary<string, string>();
        Dictionary<string, string> _edges = new Dictionary<string, string>();

        public void readFile()
        {
            try
            {
                _nodes.Clear();
                _edges.Clear();
                createNodesAndEdges();
                validateFile();

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
                                //te maken met een edge
                                _edges.Add(s[0], s[1]);
                            }
                            else
                            {
                                //te maken met een node
                                _nodes.Add(s[0], s[1]);
                            }
                        
                    }
                }
            }
        }
        public Dictionary<string, INode> getInput(Dictionary<string, INode> _nodeDictionary)
        {
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
                    // Stops Receving Keys Once Enter is Pressed
                  

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

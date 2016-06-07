using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Full_Adder
{
    class FileReader
    {
        string _filecontents;
        Dictionary<string, string> _nodes = new Dictionary<string, string>();
        Dictionary<string, string> _edges = new Dictionary<string, string>();

        public FileReader()
        {
            readFile();
            createNodesAndEdges();
            try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Fout in file");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public Dictionary<string, string> getNodes()
        {
            return _nodes;
        }

        public Dictionary<string,string> getEdges()
        {
            return _edges;
        }
        private void readFile()
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
    }
}

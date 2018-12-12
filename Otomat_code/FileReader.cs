using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Otomat_code
{
    class FileReader
    {
        static public List<string> readline(string line)
        {
            int index = line.IndexOf('#');
            if (index >= 0) line = line.Remove(index);
            string[] temp = line.Trim().Split(' ');
            List<string> y = temp.ToList<string>();
            y.RemoveAll(p => string.IsNullOrEmpty(p));
            return y;
        }
        static public DFA readDFA(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            int[,] _transitionsTable = new int[0, 0];
            List<string> _language = new List<string>();
            int _n_states = 0;
            List<int> _final_states = new List<int>();
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                line = streamReader.ReadLine();
                if (line != null)
                {
                    //doc bang chu cai
                    _language = readline(line).ToList();
                    //doc so ham trang thai
                    line = streamReader.ReadLine();
                    _n_states = Int32.Parse(readline(line)[0]);
                    //doc cac ham trang thai ket thuc
                    line = streamReader.ReadLine();
                    _final_states = readline(line).Select(int.Parse).ToList();
                    //doc cac ham trang thai
                    List<string> q;
                    int states_start;
                    string char_get;
                    int index_char;
                    int states_end;
                    _transitionsTable = new int[_language.Count, _n_states];
                    for (int i = 0; i < _language.Count; i++)
                    {
                        for (int j = 0; j < _n_states; j++)
                        {
                            _transitionsTable[i, j] = -1;
                        }
                    }
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        q = readline(line);
                        if (q != null && q.Count >= 3)
                        {
                            states_start = Int32.Parse(q[0]);
                            char_get = q[1];
                            index_char = _language.IndexOf(char_get);
                            states_end = Int32.Parse(q[2]);
                            _transitionsTable[index_char, states_start] = states_end;
                        }
                    }
                }

            }
            return new DFA(_n_states, _language, _transitionsTable, _final_states);
        }
        static public NFA readNFA(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            List<int>[,] _transitionsTable = new List<int>[0, 0];
            List<string> _language = new List<string>();
            int _n_states = 0;
            List<int> _final_states = new List<int>();
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                line = streamReader.ReadLine();
                if (line != null)
                {
                    //doc bang chu cai
                    _language = readline(line).ToList();
                    //doc so ham trang thai
                    line = streamReader.ReadLine();
                    _n_states = Int32.Parse(readline(line)[0]);
                    //doc cac ham trang thai ket thuc
                    line = streamReader.ReadLine();
                    _final_states = readline(line).Select(int.Parse).ToList();
                    //doc cac ham trang thai
                    List<string> q;
                    int states_start;
                    string char_get;
                    int index_char;
                    _transitionsTable = new List<int>[_language.Count, _n_states];
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        q = readline(line);
                        if (q != null && q.Count >= 3)
                        {
                            states_start = Int32.Parse(q[0]);
                            q.RemoveAt(0);
                            char_get = q[0];
                            index_char = _language.IndexOf(char_get);
                            q.RemoveAt(0);
                            _transitionsTable[index_char, states_start] = q.Select(int.Parse).ToList();
                        }
                    }
                }

            }
            return new NFA(_n_states, _language, _transitionsTable, _final_states);
        }
    }

}

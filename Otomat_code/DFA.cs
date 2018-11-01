using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Otomat_code
{
    class DFA
    {
        public DFA()
        {

        }
        public DFA(int n_states, List<string> language, int[,] transitionsTable, List<int> final_states, int start_states = 0)
        {
            this._n_states = n_states;
            this._language = language;
            this._transitionsTable = transitionsTable;
            this.start_states = start_states;
            this._final_states = final_states;
        }
        int[,] _transitionsTable;
        List<string> _language;
        int _n_states;
        public List<int> states
        {
            get
            {
                return Enumerable.Range(0, _n_states).ToList();
            }
        }
        List<int> _final_states;
        int _start_states;
        public int start_states
        {
            set
            {
                _start_states = value;
            }
            get
            {
                return 0;
            }
        }
        public int[,] transitionsTable
        {
            get
            {
                return _transitionsTable;
            }
        }
        public List<string> language
        {
            set
            {
                _language = value;
            }
            get
            {
                return _language;
            }
        }
        public int n_states
        {
            get
            {
                return _n_states;
            }
        }
        public List<int> final_states
        {
            get
            {
                return _final_states;
            }
        }
        public int n_symbols
        {
            get
            {
                return _language.Count;
            }
        }
        public void readDFA(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                //doc bang chu cai
                line = streamReader.ReadLine();
                _language = readline(line).ToList();
                //doc so ham trang thai
                line = streamReader.ReadLine();
                _n_states = Int32.Parse(readline(line)[0]);
                //doc cac ham trang thai ket thuc
                line = streamReader.ReadLine();
                _final_states = Array.ConvertAll(readline(line), int.Parse).ToList();
                //doc cac ham trang thai
                string[] q;
                int states_start;
                string char_get;
                int index_char;
                int states_end;
                _transitionsTable = new int[_language.Count, _n_states];
                while ((line = streamReader.ReadLine()) != null)
                {
                    q = readline(line);
                    if (q != null && q.Length >= 3)
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
        public string[] readline(string line)
        {
            int index = line.IndexOf('#');
            if (index >= 0) line = line.Remove(index);
            string[] temp = line.Trim().Split(' ');
            List<string> y = temp.ToList<string>();
            y.RemoveAll(p => string.IsNullOrEmpty(p));
            temp = y.ToArray();
            return temp;
        }
        public void writerDFA()
        {
            foreach (string temp in _language)
            {
                Console.Write(temp);
            }
            Console.Write("\n");
            Console.WriteLine(_n_states.ToString());
            foreach (int temp in _final_states)
            {
                Console.Write(temp);
            }

            Console.Write("\n  ");
            for (int i = 0; i < _language.Count; i++)
            {
                Console.Write(_language[i] + " ");
            }
            Console.Write("\n");
            for (int j = 0; j < _n_states; j++)
            {
                Console.Write(j + " ");
                for (int i = 0; i < _language.Count; i++)
                {
                    Console.Write(_transitionsTable[i, j] + " ");
                }
                Console.Write("\n");
            }
        }
        public List<int> getReachable_states()
        {
            int index_char;
            List<int> reachable_states = new List<int>();
            reachable_states.Add(start_states);
            List<int> new_states = new List<int>();
            new_states.Add(start_states);
            List<int> temp = new List<int>();
            do
            {
                //temp := the empty set;
                temp.Clear();
                foreach (int q in new_states)
                {
                    foreach (string c in _language)
                    {
                        index_char = _language.IndexOf(c);
                        //temp:= temp ∪ { p such that p = δ(q, c)};
                        int p = _transitionsTable[index_char, q];
                        if (!temp.Contains(p))
                            temp.Add(p);
                    }
                }
                // new_states:= temp \ reachable_states;
                new_states = temp.Except(reachable_states).ToList();
                //reachable_states:= reachable_states ∪ new_states;
                foreach (var item in new_states)
                {
                    if (!reachable_states.Contains(item))
                    {
                        reachable_states.Add(item);
                    }
                }
                if (new_states.Count == 0)
                    break;
            }
            while (true);
            return reachable_states;
        }

        public DFA minimizationDFA()
        {
            int index_char;
            List<int> IntersectSet = new List<int>();
            List<int> ExceptSet = new List<int>();
            //loai bo cac trang thai gieng(cac trang thai chi vao ko co ra, trang thai khong ket thuc)
            List<int> reachable_states = getReachable_states();
            //tao cac cap trang thai tuong duong
            List<List<int>> W = new List<List<int>>() { final_states };
            List<List<int>> P = new List<List<int>>() {
                final_states,
                reachable_states.Except(final_states).ToList()
            };
            List<int> X = new List<int>();
            List<int> A = new List<int>();
            List<int> Y;
            int p;
            while (W.Count > 0)
            {
                A = W[0];
                W.RemoveAt(0);
                foreach (var c in language)
                {
                    index_char = _language.IndexOf(c);
                    for (int i = 0; i < _n_states; i++)
                    {
                        p = _transitionsTable[index_char, i];
                        if (A.Contains(p))
                        {
                            X.Add(i);
                        }
                    }
                    if (X.Count == 0)
                    {
                        continue;
                    }
                    for (int i = 0; i < P.Count; i++)
                    {
                        Y = P[i];
                        IntersectSet = X.Intersect(Y).ToList();
                        ExceptSet = Y.Except(X).ToList();
                        if (IntersectSet.Count() > 0 && ExceptSet.Count() > 0)
                        {
                            P.Remove(Y);
                            P.Add(IntersectSet);
                            P.Add(ExceptSet);
                            if (W.Contains(Y))
                            {
                                W.Remove(Y);
                                W.Add(IntersectSet);
                                W.Add(ExceptSet);
                            }
                            else
                            {
                                if (IntersectSet.Count <= ExceptSet.Count)
                                {
                                    W.Add(IntersectSet);
                                }
                                else
                                {
                                    W.Add(ExceptSet);
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in P)
            {
                item.Sort();
            }
            P = P.OrderBy(lst => lst[0]).ToList();
            int new_n_states = P.Count;
            List<int> newfinal_states = new List<int>();
            for (int j = 0; j < new_n_states; j++)
            {
                if (ListCheck(P[j], final_states))
                {
                    if (!newfinal_states.Contains(j))
                    {
                        newfinal_states.Add(j);
                    }
                }
            }

            int[,] newtransitionsTable = new int[language.Count, new_n_states];
            for (int i = 0; i < language.Count; i++)
            {
                for (int j = 0; j < new_n_states; j++)
                {
                    newtransitionsTable[i, j] = -1;
                }
            }

            for (int i = 0; i < new_n_states; i++)
            {
                List<int> item = P[i];
                foreach (var states in item)
                {
                    foreach (var c in language)
                    {
                        index_char = _language.IndexOf(c);
                        int q = transitionsTable[index_char, states];
                        for (int j = 0; j < new_n_states; j++)
                        {
                            if (P[j].Contains(q))
                            {
                                newtransitionsTable[index_char, i] = j;
                                break;
                            }
                        }
                    }
                }
            }

            return new DFA(new_n_states, language, newtransitionsTable, newfinal_states);
        }
        private bool ListCheck<T>(IEnumerable<T> l1, IEnumerable<T> l2)
        {
            // TODO: Null parm checks
            if (l1.Intersect(l2).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

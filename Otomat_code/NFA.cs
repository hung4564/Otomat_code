using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG = System.Collections.Generic;


namespace Otomat_code
{
    class NFA
    {
        List<int>[,] _transitionsTable;
        List<string> _language;
        int _n_states;
        public int n_states
        {
            get
            {
                return this._n_states;
            }
        }
        List<int> _final_states;
        int _start_states;
        public List<int> states
        {
            get
            {
                return Enumerable.Range(0, _n_states).ToList();
            }
        }
        public int start_states
        {
            get
            {
                return this._start_states;
            }
        }
        public NFA()
        {
            this._n_states = 0;
            this._language = new List<string>();
            this._transitionsTable = new List<int>[0, 0];
            this._start_states = 0;
            this._final_states = new List<int>();
        }
        public NFA(int n_states, List<string> language, List<int>[,] transitionsTable, List<int> final_states, int start_states)
        {
            this._n_states = n_states;
            this._language = language;
            this._transitionsTable = transitionsTable;
            this._start_states = start_states;
            this._final_states = final_states;
        }
        public NFA(NFA nfa)
        {
            this._start_states = nfa._start_states;
            this._final_states = nfa._final_states;
            this._transitionsTable = nfa._transitionsTable;
            this._n_states = nfa._n_states;
            this._language = nfa._language;
        }

        public NFA(int n_states, int start_state, int final_state, List<string> language)
        {
            this._start_states = start_state;
            this._final_states = new List<int>() { final_state };
            this._n_states = n_states;
            IsLegalState(start_state);
            IsLegalState(final_state);
            this._language = language;
            this._transitionsTable = new List<int>[_language.Count, n_states];
        }
        public void writerNFA()
        {

            string temp = string.Join(",", _language.ToArray());
            Console.WriteLine(temp);
            Console.WriteLine(_n_states.ToString());
            temp = string.Join(",", _final_states.ToArray());
            Console.WriteLine(temp);
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
                    if (_transitionsTable[i, j] != null)
                    {
                        string combindedString = string.Join(",", _transitionsTable[i, j].ToArray());
                        Console.Write(combindedString + " ");
                    }
                    else
                    {
                        Console.Write("e ");
                    }
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }
        public DFA convertToDFA()
        {
            var this_NFA = this;
            int new_n_states = 0;
            List<string> new_language = this_NFA._language;
            int[,] newtransitionsTable;
            List<int> newfinal_states = new List<int>();
            //tìm trạng thái và bảng trạng thái mới
            List<List<int>> temp_new_states = new List<List<int>>();
            List<int[]> temp_new_transitionsTable = new List<int[]>();
            //bắt đầu bởi trạng thái bắt đầu
            temp_new_states.Add(new List<int>() { start_states });
            for (int i = 0; i < temp_new_states.Count; i++)
            {
                // lấy ra các trạng thái mới chưa kiểm tra và hàm chuyển mới
                List<int> state_check = temp_new_states[i];
                List<int> new_state = new List<int>();
                // hàm chuyển tương ứng với trạng thái đang kiểm tra, khởi tạo bằng -1 => trỏ tới trạng thái giếng
                int[] temp_line = new int[new_language.Count];
                for (int j = 0; j < new_language.Count; j++)
                {
                    temp_line[j] = -1;
                }
                foreach (var key in new_language)
                {
                    new_state = new List<int>();
                    int index_char = _language.IndexOf(key);
                    foreach (var item in state_check)
                    {
                        List<int> temp = _transitionsTable[index_char, item];
                        if (temp != null)
                        {
                            List<int> diff = temp.Except(new_state).ToList();
                            if (diff.Count > 0)
                            {
                                new_state.AddRange(diff);
                            }
                        }

                    }
                    new_state.Sort();
                    var index_state = -1;
                    //kiểm tra xem new_state đã tồn tại chưa
                    for (int j = 0; j < temp_new_states.Count; j++)
                    {
                        if (temp_new_states[j].Count == new_state.Count)
                        {
                            if (temp_new_states[j].SequenceEqual(new_state))
                            {
                                index_state = j;
                                break;
                            }
                        }
                    }
                    if (index_state < 0)
                    {
                        temp_new_states.Add(new_state);
                        index_state = temp_new_states.Count - 1;
                    }
                    temp_line[index_char] = index_state;
                }
                temp_new_transitionsTable.Add(temp_line);
            }
            // gán lại giá trị cho DFA
            new_n_states = temp_new_states.Count;
            // chuyển bảng trạng lại cho DFA
            newtransitionsTable = new int[new_language.Count, new_n_states];
            for (int i = 0; i < new_language.Count; i++)
            {
                for (int j = 0; j < new_n_states; j++)
                {
                    newtransitionsTable[i, j] = temp_new_transitionsTable[j][i];
                }
            }
            //Kiếm tra các trạng thái kết thúc mới, là các trạng thái chứa trạng thái kết thúc của NFA
            for (int i = 0; i < temp_new_states.Count; i++)
            {
                if (temp_new_states[i].Intersect(this_NFA._final_states).Any())
                {
                    newfinal_states.Add(i);
                }
            }

            return new DFA(new_n_states, new_language, newtransitionsTable, newfinal_states);
        }
        public Grammar conventToGrammar()
        {
            var temp = new Grammar();
            //tap trang thai ket thuc la bang ngon ngu ben NFA
            temp.Vt = this._language;
            //tap trang thai bien la cac trang thai ben NFA, do la day so nen can chuyen sang chuoi.
            temp.Vn = this.states.ConvertAll<string>(x => Convert.ToChar(x + 65).ToString());
            temp.S = Convert.ToChar(this.start_states + 65).ToString();
            for (int j = 0; j < _n_states; j++)
            {
                for (int i = 0; i < _language.Count; i++)
                {
                    if (_transitionsTable[i, j] != null)
                    {
                        string combindedString = string.Join(",", _transitionsTable[i, j].ToArray());
                        foreach (var item in _transitionsTable[i, j])
                        {
                            var vecto = new Vector();
                            vecto.start_states = Convert.ToChar(j + 65).ToString();
                            vecto.parameter = new List<string>() { this._language[i] };
                            vecto.end_states = Convert.ToChar(item + 65).ToString();
                            temp.P.Add(vecto);
                        }
                    }
                }
            }
            return temp;
        }
        public static NFA TreeToNFA(ParseTree tree, List<string> language)
        {
            switch (tree.type)
            {
                case ParseTree.NodeType.Chr:
                    return BuildNFABasic(tree.data.Value.ToString(), language);
                case ParseTree.NodeType.Alter:
                    return BuildNFAAlter(TreeToNFA(tree.left, language), TreeToNFA(tree.right, language));
                case ParseTree.NodeType.Concat:
                    return BuildNFAConcat(TreeToNFA(tree.left, language), TreeToNFA(tree.right, language));
                case ParseTree.NodeType.Star:
                    return BuildNFAStar(TreeToNFA(tree.left, language));
                case ParseTree.NodeType.Question:
                    return BuildNFAAlter(TreeToNFA(tree.left, language), BuildNFABasic(Constants.Epsilon.ToString(), language));
                default:
                    return null;
            }
        }
        public static NFA BuildNFABasic(string @in, List<string> language)
        {
            NFA basic = new NFA(2, 0, 1, language);

            basic.AddTrans(0, 1, @in);

            return basic;
        }
        public static NFA BuildNFAAlter(NFA nfa1, NFA nfa2)
        {
            // How this is done: the new nfa must contain all the states in
            // nfa1 and nfa2, plus a new initial and final states. 
            // First will come the new initial state, then nfa1's states, then
            // nfa2's states, then the new final state

            // make room for the new initial state
            nfa1.ShiftStates(1);

            // make room for nfa1
            nfa2.ShiftStates(nfa1._n_states);

            // create a new nfa and initialize it with (the shifted) nfa2
            NFA newNFA = new NFA(nfa2);

            // nfa1's states take their places in new_nfa
            newNFA.FillStates(nfa1);

            // Set new initial state and the transitions from it
            newNFA.AddTrans(0, nfa1._start_states, Constants.Epsilon.ToString());
            newNFA.AddTrans(0, nfa2._start_states, Constants.Epsilon.ToString());

            newNFA._start_states = 0;

            // Make up space for the new final state
            newNFA.AppendEmptyState();

            // Set new final state
            newNFA._final_states = new List<int>() { newNFA._n_states - 1 };

            newNFA.AddTrans(nfa1._final_states[0], newNFA._final_states[0], Constants.Epsilon.ToString());
            newNFA.AddTrans(nfa2._final_states[0], newNFA._final_states[0], Constants.Epsilon.ToString());

            return newNFA;
        }
        /// <summary>
        /// Builds an alternation of nfa1 and nfa2 (nfa1|nfa2)
        /// </summary>
        /// <param name="nfa1"></param>
        /// <param name="nfa2"></param>
        /// <returns></returns>
        public static NFA BuildNFAConcat(NFA nfa1, NFA nfa2)
        {
            // How this is done: First will come nfa1, then nfa2 (its initial state replaced
            // with nfa1's final state)
            nfa2.ShiftStates(nfa1._n_states - 1);

            // Creates a new NFA and initialize it with (the shifted) nfa2
            NFA newNFA = new NFA(nfa2);

            // nfa1's states take their places in newNFA
            // note: nfa1's final state overwrites nfa2's initial state,
            // thus we get the desired merge automatically (the transition
            // from nfa2's initial state now transits from nfa1's final state)
            newNFA.FillStates(nfa1);

            // Sets the new initial state (the final state stays nfa2's final state,
            // and was already copied)
            newNFA._start_states = nfa1._start_states;

            return newNFA;
        }

        /// <summary>
        /// Builds a star (kleene closure) of nfa (nfa*)
        /// How this is done: First will come the new initial state, then NFA, then the new final state
        /// </summary>
        /// <param name="nfa"></param>
        /// <returns></returns>
        public static NFA BuildNFAStar(NFA nfa)
        {
            // Makes room for the new initial state
            nfa.ShiftStates(1);

            // Makes room for the new final state
            nfa.AppendEmptyState();

            // Adds new transitions
            nfa.AddTrans(nfa._final_states[0], nfa._start_states, Constants.Epsilon.ToString());
            nfa.AddTrans(0, nfa._start_states, Constants.Epsilon.ToString());
            nfa.AddTrans(nfa._final_states[0], nfa._n_states - 1, Constants.Epsilon.ToString());
            nfa.AddTrans(0, nfa._n_states - 1, Constants.Epsilon.ToString());

            nfa._start_states = 0;
            nfa._final_states = new List<int>() { nfa._n_states - 1 };

            return nfa;
        }
        /// <summary>
        /// Provides default values for epsilon and none
        /// </summary>
        public enum Constants
        {
            Epsilon = 'ε',
            None = '\0'
        }


        public bool IsLegalState(int s)
        {
            // We have 'size' states, numbered 0 to size-1
            if (s < 0 || s >= _n_states)
                return false;

            return true;
        }
        public void AddTrans(int from, int to, string @in)
        {
            IsLegalState(from);
            IsLegalState(to);
            if (!_language.Where(x => x.Contains(@in)).Any())
            {
                _language.Add(@in);
                _transitionsTable = Resize(_transitionsTable, _language, _n_states);
            }
            var index_char = _language.FindIndex(x => x.Contains(@in));
            if (_transitionsTable[index_char, from] != null)
            {
                _transitionsTable[index_char, from].Add(to);
            }
            else
            {
                _transitionsTable[index_char, from] = new List<int>() { to };
            }
        }
        /// <summary>
        /// Renames all the NFA's states. For each nfa state: number += shift.
        /// Functionally, this doesn't affect the NFA, it only makes it larger and renames
        /// its states.
        /// </summary>
        /// <param name="shift"></param>
        public void ShiftStates(int shift)
        {
            int newSize = this._n_states + shift;

            if (shift < 1)
                return;
            var newTransTable = new List<int>[_language.Count, newSize];

            // Copies all the transitions to the new table, at their new locations.
            for (int i = 0; i < _language.Count; ++i)
                for (int j = 0; j < this._n_states; ++j)
                {
                    if (_transitionsTable[i, j] != null)
                    {
                        newTransTable[i, j + shift] = _transitionsTable[i, j].Select(x => x + shift).ToList();
                    }
                }

            // Updates the NFA members.
            _n_states = newSize;
            _start_states += shift;
            _final_states = _final_states.Select(x => x + shift).ToList();
            _transitionsTable = newTransTable;
        }
        public void FillStates(NFA other)
        {
            for (int i = 0; i < other._language.Count; ++i)
                for (int j = 0; j < other._n_states; ++j)
                {
                    if (other._transitionsTable[i, j] != null)
                        if (_transitionsTable[i, j] != null)
                        {
                            _transitionsTable[i, j].AddRange(other._transitionsTable[i, j]);
                        }
                        else
                        {
                            _transitionsTable[i, j] = other._transitionsTable[i, j];
                        }
                }
        }
        public void AppendEmptyState()
        {
            _transitionsTable = Resize(_transitionsTable, _language, _n_states + 1);

            _n_states += 1;
        }
        private static List<int>[,] Resize(List<int>[,] transTable, List<string> language, int newSize)
        {
            List<int>[,] newTransTable = new List<int>[language.Count, newSize];
            for (int i = 0; i < transTable.GetLength(0); i++)
                for (int j = 0; j < transTable.GetLength(1); j++)
                {
                    if (transTable[i, j] != null)
                    {
                        newTransTable[i, j] = transTable[i, j];
                    }
                }

            return newTransTable;
        }
    }
}

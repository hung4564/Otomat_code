using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Otomat_code
{
    class NFA
    {
        List<int>[,] _transitionsTable;
        List<string> _language;
        int _n_states;
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
        public NFA(int n_states, List<string> language, List<int>[,] transitionsTable, List<int> final_states, int start_states = 0)
        {
            this._n_states = n_states;
            this._language = language;
            this._transitionsTable = transitionsTable;
            this._start_states = start_states;
            this._final_states = final_states;
        }

        public void writerNFA()
        {
            foreach (string temp in _language)
            {
                Console.Write(temp);
            }
            Console.Write("\n");
            Console.WriteLine(_n_states.ToString());
            foreach (int temp in _final_states)
            {
                Console.Write(temp + " ");
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
                    if (_transitionsTable[i, j] != null)
                    {
                        string combindedString = string.Join(",", _transitionsTable[i, j].ToArray());
                        Console.Write(combindedString + " ");
                    }
                    else
                    {
                        Console.Write("e");
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
                Console.Write(j + " ");
                for (int i = 0; i < _language.Count; i++)
                {
                    if (_transitionsTable[i, j] != null)
                    {
                        string combindedString = string.Join(",", _transitionsTable[i, j].ToArray());
                        foreach (var item in _transitionsTable[i, j])
                        {
                            var vecto = new Vector();
                            vecto.start_states = Convert.ToChar(j + 65).ToString();
                            vecto.parameter = this._language[i];
                            vecto.end_states = Convert.ToChar(item + 65).ToString();
                            temp.P.Add(vecto);
                        }
                    }
                }
            }
            return temp;
        }
    }
}

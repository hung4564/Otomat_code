using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Otomat_code
{
  class Grammar
  {
    const string epsilon = "ep";
    // tap cac bien la V hay N
    public List<string> Vn;
    //tap cac bien ket thuc la T
    public List<string> Vt;
    //tap cac luat sinh
    public List<Vector> P;
    //trang thai bat dau
    public string S;
    public Grammar()
    {
      Vn = new List<string>();
      Vt = new List<string>();
      P = new List<Vector>();
    }
    public void wrireGrammar()
    {
      Console.WriteLine("Grammar");

      string temp = string.Join(",", Vn.ToArray());
      Console.WriteLine(temp);
      temp = string.Join(",", Vt.ToArray());
      Console.WriteLine(temp);
      Console.Write("\n");
      Console.WriteLine(S + " ");
      foreach (Vector t in P)
      {
        Console.WriteLine(t.ToString());
      }
    }
    public NFA conventToNFA()
    {
      var new_start_states = 0;
      // danh sach trang thai voi nhan la ky tu thuoc tap ket thuc
      List<string> new_language = new List<string>();
      new_language.AddRange(this.Vt);
      new_language.Add(epsilon);
      List<int>[,] new_transitionsTable;
      int count_new_state = 0;
      List<int> new_final_state = new List<int>();

      List<string> temp_states = new List<string>();
      List<string> temp_new_final_states = new List<string>();
      temp_states.Add(this.S);
      temp_states.AddRange(this.Vn.Where(v => !v.Equals(this.S)).ToList());
      //chuyen tap sinh sang bang trang thai cua NFA
      List<Vector> temp_vect = new List<Vector>();
      if (temp_new_final_states.IndexOf(epsilon) < 0)
      {
        temp_states.Add(epsilon);
        temp_new_final_states.Add(epsilon);
      }
      foreach (Vector item in this.P)
      {
        Vector temp = new Vector();
        temp.start_states = item.start_states;
        if (new_language.Where(stringToCheck => stringToCheck.Contains(item.end_states)).Any())
        {
          string t = item.end_states.Clone().ToString();
          temp.parameter = item.parameter;
          temp.parameter.Add(t);
          temp.end_states = epsilon;
        }
        else
        {
          temp.parameter = item.parameter.Count > 0 ? item.parameter : new List<string>() { epsilon };
          temp.end_states = item.end_states;
        }
        List<Vector> temp_list = changeVecto(temp);
        if (temp_list.Count > 1)
        {
          foreach (var v in temp_list)
          {
            if (!temp_states.Where(stringToCheck => stringToCheck.Contains(v.end_states)).Any())
            {
              temp_states.Add(v.end_states);
            }
          }
        }
        temp_vect.AddRange(temp_list);
      }
      count_new_state = temp_states.Count;
      // chuyen ve bang trang thai
      new_transitionsTable = new List<int>[new_language.Count, count_new_state];
      int index_char;
      int index_state_p;
      int index_state_q;

      index_state_p = temp_states.FindIndex(x => x.Contains(epsilon));

      for (var i = 0; i < new_language.Count; i++)
      {
        new_transitionsTable[i, index_state_p] = new List<int>() { index_state_p };
      }
      //den day bang sinh con co dang s -> a A
      foreach (var item in temp_vect)
      {
        // tu Ep tro roi Ep => trang thai ket thuc
        if (item.parameter[0].Contains(epsilon) && item.end_states.Contains(epsilon))
        {
          temp_new_final_states.Add(item.start_states);
        }
        else
        {
          index_char = new_language.IndexOf(item.parameter[0]);
          index_state_p = temp_states.FindIndex(x => x.Contains(item.start_states));
          index_state_q = temp_states.FindIndex(x => x.Contains(item.end_states));
          if (new_transitionsTable[index_char, index_state_p] == null)
          {
            new_transitionsTable[index_char, index_state_p] = new List<int>() { index_state_q };
          }
          else
          {
            new_transitionsTable[index_char, index_state_p].Add(index_state_q);
          }

        }
      }
      //chuyen lai nhan trang thai ve so
      foreach (var item in temp_new_final_states)
      {
        index_state_p = temp_states.FindIndex(x => x.Contains(item));
        new_final_state.Add(index_state_p);
      }
      new_start_states = temp_states.FindIndex(x => x.Contains(this.S));
      return new NFA(count_new_state, new_language, new_transitionsTable, new_final_state, new_start_states);
    }

    private List<Vector> changeVecto(Vector vector)
    {
      List<Vector> result = new List<Vector>();
      if (vector.parameter.Count > 1 || Vt.Where(stringToCheck => stringToCheck.Contains(vector.end_states)).Any())
      {
        // nut dau
        Vector temp = new Vector();
        List<string> temp_parameter = new List<string>(vector.parameter);
        temp.start_states = vector.start_states;
        temp.parameter = new List<string>() { vector.parameter[0] };
        temp.end_states = string.Join(",", vector.parameter.ToArray()) + "," + vector.end_states;
        temp_parameter.RemoveAt(0);
        result.Add(temp);
        var temp_string = temp.end_states;
        while (temp_parameter.Count > 1)
        {
          temp = new Vector();
          temp.start_states = temp_string;
          temp.parameter = new List<string>() { temp_parameter[0] };
          temp_parameter.RemoveAt(0);
          temp_string = string.Join(",", temp_parameter.ToArray()) + "," + vector.end_states;
          temp.end_states = temp_string;
          result.Add(temp);
        }
        // nut cuoi
        temp = new Vector();
        var last = vector.parameter.Count;
        temp.start_states = vector.parameter[last - 2] + "," + vector.parameter[last - 1] + "," + vector.end_states;
        temp.parameter = new List<string>() { vector.parameter[vector.parameter.Count - 1] };
        temp.end_states = vector.end_states;
        result.Add(temp);
      }
      else
      {
        result = new List<Vector>() { vector };
      }
      return result;
    }
  }
}

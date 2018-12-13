using System;
using System.Collections.Generic;
using System.Text;

namespace Otomat_code
{
    class Grammar
    {
        // tap cac bien 
        public List<string> Vn;
        //tap cac bien ket thuc
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
            foreach (string temp in Vn)
            {
                Console.Write(temp + " ");
            }
            Console.Write("\n");
            foreach (string temp in Vt)
            {
                Console.Write(temp + " ");
            }
            Console.Write("\n");
            Console.WriteLine(S + " ");
            foreach (Vector temp in P)
            {
                Console.WriteLine(temp.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Otomat_code
{
    class Program
    {

        static void Main(string[] args)
        {
            var dfa = new DFA();
            dfa.readDFA("data/TextFile1.txt");
            dfa.minimizationDFA().writerDFA();
            Console.ReadKey();
        }
    }
}

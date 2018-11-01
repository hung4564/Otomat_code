using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Otomat_code
{
    class Program
    {

        static void Main(string[] args)
        {
            var dfa = new DFA();
            string filePath = "TextFile1.txt";
            dfa.readDFA(filePath);
            dfa.minimizationDFA().writerDFA();
            Console.ReadKey();
        }
    }
}

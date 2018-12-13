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
            string filePath = "TextFile1.txt";
            //var dfa = FileReader.readDFA(filePath);
            //dfa.minimizationDFA().writerDFA();
            filePath = "NFA_1.txt";
            var nfa = FileReader.readNFA(filePath);
            nfa.writerNFA();
            nfa.conventToGrammar().wrireGrammar();
            Console.ReadKey();
        }
    }
}

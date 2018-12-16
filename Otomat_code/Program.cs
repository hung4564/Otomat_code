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
            //var nfa = FileReader.readNFA(filePath);
            //nfa.writerNFA();
            //nfa.conventToGrammar().wrireGrammar();
            filePath = "Gp_1.txt";
            //var grammar = FileReader.readGrammar(filePath);
            //grammar.wrireGrammar();
            //Console.WriteLine("convent");
            //grammar.conventToNFA().writerNFA();
            string reg = "01";
            RegexParser myRegexParser = new RegexParser();
            myRegexParser.Init(reg);
            // Creating a parse tree with the preprocessed regex
            ParseTree parseTree = myRegexParser.Expr();

            // Checking for a string termination character after
            // parsing the regex
            if (myRegexParser.Peek() != '\0')
            {
                Console.WriteLine("Parse error: unexpected char, got {0} at #{1}", myRegexParser.Peek(), myRegexParser.GetPos());
            }
            RegexParser.PrintTree(parseTree, 1);
            var nFA = NFA.TreeToNFA(parseTree, myRegexParser.language);
            nFA.writerNFA();
            nFA.convertToDFA().writerDFA();
            Console.ReadKey();
        }
    }
}

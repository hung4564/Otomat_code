using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Otomat_code
{
    class Program
    {
        int[,] dfa;
        string[] language;
        int n_status;
        int[] final_status;

        public void readDFA()
        {
            var fileStream = new FileStream("data/TextFile1.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                //doc bang chu cai
                line = streamReader.ReadLine();
                language = readline(line);
                //doc so ham trang thai
                line = streamReader.ReadLine();
                n_status = Int32.Parse(readline(line)[0]);
                //doc cac ham trang thai ket thuc
                line = streamReader.ReadLine();
                final_status = Array.ConvertAll(readline(line), int.Parse);
                //doc cac ham trang thai
                string[] q;
                int status_start;
                string char_get;
                int index_char;
                int status_end;
                dfa = new int[language.Length, n_status];
                while ((line = streamReader.ReadLine()) != null)
                {
                    q = readline(line);
                    if (q != null && q.Length >= 3)
                    {
                        status_start = Int32.Parse(q[0]);
                        char_get = q[1];
                        index_char = Array.IndexOf(language, char_get);
                        status_end = Int32.Parse(q[2]);
                        dfa[index_char, status_start] = status_end;
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
        public void writerdfa()
        {
            foreach (string temp in language)
            {
                Console.Write(temp);
            }
            Console.Write("\n");
            Console.WriteLine(n_status.ToString());
            foreach (int temp in final_status)
            {
                Console.Write(temp);
            }

            Console.Write("\n  ");
            for (int i = 0; i < language.Length; i++)
            {
                Console.Write(language[i] + " ");
            }
            Console.Write("\n");
            for (int j = 0; j < n_status; j++)
            {
                Console.Write(j+" ");
                for (int i = 0; i < language.Length; i++)
                {
                    Console.Write(dfa[i, j] + " ");
                }
                Console.Write("\n");
            }
        }
        static void Main(string[] args)
        {
            var p = new Program();
            p.readDFA();
            p.writerdfa();
            Console.ReadKey();
        }
    }
}

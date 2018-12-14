using System;
using System.Collections.Generic;
using System.Text;

namespace Otomat_code
{
    class Vector
    {
        public string start_states;
        public List<string> parameter;
        public string end_states;
        public override string ToString()
        {
            string combindedString = string.Join("", parameter.ToArray());
            return start_states + " -> " + combindedString + " " + end_states;
        }
        public Vector()
        {
            this.parameter = new List<string>();
        }
    }
}

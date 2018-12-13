using System;
using System.Collections.Generic;
using System.Text;

namespace Otomat_code
{
    class Vector
    {
        public string start_states;
        public string end_states;
        public string parameter;
        public override string ToString()
        {
            return start_states + " ->" + parameter + "" + end_states;
        }
    }
}

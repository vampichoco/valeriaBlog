using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace valeria2
{
    public class post
    {
        private string _title;
        private string _text; 

        public string title
        {
            get { return _title; }
            set { _title = value; }
        } 

        public string text
        {
            get { return _text; }
            set { _text = value; }
        }

    }
}

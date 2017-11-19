using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaOfGreed.SOGLang
{
    public class ParserTypes
    {
        public class FName
        {
            public FName(string name)
            {
                this.name = name;
            }

            public string name;
        }

        public class FArg
        {
            public FArg(object value)
            {
                this.value = value;
            }

            public object value;
        }

        public class Comparator
        {
            public Comparator(string name)
            {
                this.name = name;
            }
            public string name;
        }

        public class Extender
        {
            public Extender(string name)
            {
                this.name = name;
            }

            public string name;
        }

            
    }
}


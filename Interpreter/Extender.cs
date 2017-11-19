using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaOfGreed.SOGLang
{
    public static class Extender
    {
        public static bool extend(object obj1, object obj2, string oper)
        {
            bool val1 = false;
            bool val2 = false;

            if(obj1 is Vector3)
            {
                val1 = !((Vector3)obj1 == new Vector3(0, 0, 0)) ;
            }

            else if(obj1 is float)
            {
                val1 = !(Convert.ToSingle(obj1) == 0F);
            }

            else if(obj1 is string)
            {
                val1 = !(Convert.ToString(obj1) == "");
            }

            else if(obj1 is bool)
            {
                val1 = Convert.ToBoolean(obj1);
            }

            else if(obj1 == null)
            {
                val1 = false;
            }

            if (obj2 is Vector3)
            {
                val2 = !((Vector3)obj2 == new Vector3(0, 0, 0));
            }

            else if (obj2 is float)
            {
                val2 = !(Convert.ToSingle(obj2) == 0F);
            }

            else if (obj2 is string)
            {
                val2 = !(Convert.ToString(obj2) == "");
            }

            else if (obj2 is bool)
            {
                val2 = Convert.ToBoolean(obj2);
            }

            else if (obj2 == null)
            {
                val2 = false;
            }


            switch (oper)
            {
                case "&&":
                    return Convert.ToBoolean(val1) && Convert.ToBoolean(val2);

                case "||":
                    return Convert.ToBoolean(val1) || Convert.ToBoolean(val2);

                default:
                    throw new ArgumentException("This is not supposed to happen.");

            }
        }
    }
}
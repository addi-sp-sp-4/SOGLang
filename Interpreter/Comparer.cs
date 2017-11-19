using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaOfGreed.SOGLang
{
    public static class Comparer
    {

        public static bool compare(object obj1, object obj2, string comparator)
        {
            if(comparator == "==")
            {
                return obj1.Equals(obj2);
            }

            if(comparator == "!=")
            {
                return !obj1.Equals(obj2);
            }

            if( (obj1 is Vector3 || obj2 is float) == false)
            {
                throw new ArgumentException("Can't use comparator " + comparator + "with those 2 types");
                
            }

            if(obj1.GetType() != obj2.GetType())
            {
                throw new ArgumentException("Cant compare different types with comparator " + comparator);
            }

            // Due to previous checks we know both of the objects are vector3s if one is
            if(obj1 is Vector3)
            {
                Vector3 v1 = (Vector3)obj1;
                Vector3 v2 = (Vector3)obj2;

                switch(comparator)
                {
                    case "<":
                        return ( v1[0] < v2[0]) && (v1[1] < v2[1]) && (v1[2] < v2[2]);

                    case ">":
                        return (v1[0] > v2[0]) && (v1[1] > v2[1]) && (v1[2] > v2[2]);

                    case "<=":
                        return (v1[0] <= v2[0]) && (v1[1] <= v2[1]) && (v1[2] <= v2[2]);

                    case ">=":
                        return (v1[0] >= v2[0]) && (v1[1] >= v2[1]) && (v1[2] >= v2[2]);

                    default:
                        throw new ArgumentException("This isn't supposed to happen.");
                }
            }

            // They are both a float 
            else
            {
                float f1 = Convert.ToSingle(obj1);
                float f2 = Convert.ToSingle(obj2);

                switch(comparator)
                {
                    case "<":
                        return f1 < f2;

                    case ">":
                        return f1 > f2;

                    case "<=":
                        return f1 <= f2;

                    case ">=":
                        return f1 >= f2;

                    default:
                        throw new ArgumentException("This isn't supposed to happen.");
                }
            }
        }

    }
}
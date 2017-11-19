using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeaOfGreed.SOGLang
{
    public static class Calculator
    {
        private static List<string> operators = new List<string>(
        new string[] 
            {
                "+",
                "-",
                "*",
                "/",
                "%",
                "^"
            }
        );


        private static Dictionary<string, int> precedences = new Dictionary<string, int>()
        {
            { "+", 0},
            { "-", 0},

            { "*", 1},
            { "/", 1},
            { "%", 1},

            { "^", 2}
        };

        public static object calculate(List<object> input)
        {
            return evaluatePostFix(toPostFix(input));
        }

        public static object evaluatePostFix(List<object> postfix)
        {
            string oper = "";

            object obj1 = new object();
            object obj2 = new object();

            Stack<object> values = new Stack<object>();


            foreach(object val in postfix)
            {
                if(val is string)
                {
                    oper = Convert.ToString(val);

                    obj1 = values.Pop();
                    obj2 = values.Pop();

                    values.Push(evaluateNumbers(obj2, obj1, oper));
                }

                else
                {
                    values.Push(val);
                }
            }
            return values.Pop();
        }

        private static object evaluateNumbers(object obj1, object obj2, string oper)
        {
            // Prepare yourself
            // Please for the love of god let there be a better way

            bool obj1IsV3 = false;
            bool obj2IsV3 = false;

            if(obj1 is Vector3)
            {
                obj1IsV3 = true;
            }

            if(obj2 is Vector3)
            {
                obj2IsV3 = true;
            }

            if(!obj1IsV3 && !obj2IsV3)
            {
                float float1 = Convert.ToSingle(obj1);
                float float2 = Convert.ToSingle(obj2);

                switch(oper)
                {
                    
                    case "+":
                        return float1 + float2;
                       
                    case "-":
                        return float1 - float2;

                    case "*":
                        return float1 * float2;

                    case "/":
                        return float1 / float2;

                    case "%":
                        return float1 % float2;

                    case "^":
                        return Mathf.Pow(float1, float2);

                    default:
                        throw new ArgumentException("Invalid operator");
                }
            }

            else if(obj1IsV3 && obj2IsV3)
            {
                Vector3 vector1 = (Vector3)obj1;
                Vector3 vector2 = (Vector3)obj2;

                switch (oper)
                {
                    case "+":
                        return vector1 + vector2;

                    case "-":
                        return vector1 - vector2;

                    case "*":
                       
                        return new Vector3(vector1[0] * vector2[0], vector1[1] * vector2[1], vector1[2] * vector2[2]);

                    case "/":
                        
                        return new Vector3(vector1[0] / vector2[0], vector1[1] / vector2[1], vector1[2] / vector2[2]);

                    case "%":
                        return new Vector3(vector1[0] % vector2[0], vector1[1] % vector2[1], vector1[2] % vector2[2]);

                    case "^":
                        return new Vector3(Mathf.Pow(vector1[0], vector2[0]), Mathf.Pow(vector1[1], vector2[1]), Mathf.Pow(vector1[2], vector2[2]));

                    default:
                        throw new ArgumentException("Invalid operator");
                }
            }

            // If one of the two is a vector3
            else
            {
                // means designated vector and designated float
                Vector3 dv;
                float df;

                int theChosenOne;

                if(obj1IsV3)
                {
                    dv = (Vector3)obj1;
                    df = Convert.ToSingle(obj2);
                    theChosenOne = 1;
                }
                else
                {
                    dv = (Vector3)obj2;
                    df = Convert.ToSingle(obj1);
                    theChosenOne = 2;
                }

                switch(oper)
                {
                    case "+":

                        if(theChosenOne == 1)
                        {
                            return new Vector3(dv[0] + df, dv[1] + df, dv[2] + df);
                        }
                        throw new ArgumentException("Unable to add with a single on the left side");

                    case "-":
                        if(theChosenOne == 1)
                        {
                            return new Vector3(dv[0] - df, dv[1] - df, dv[2] - df);
                        }
                        throw new ArgumentException("Unable to subtract with a single on the left side");

                    case "*":
                        return dv * df;

                    case "/":
                        if (theChosenOne == 1)
                        {
                            return dv / df;
                        }
                        throw new ArgumentException("Unable to divide with a single on the left side");

                    case "%":
                        if (theChosenOne == 1)
                        {
                            return new Vector3(dv[0] % df, dv[1] % df, dv[2] % df);
                        }
                        throw new ArgumentException("Unable to apply modulus operation with a single on the left side");


                    case "^":
                        if (theChosenOne == 1)
                        {
                            return new Vector3(Mathf.Pow(dv[0], df), Mathf.Pow(dv[1], df), Mathf.Pow(dv[2], df));
                        }
                        throw new ArgumentException("Unable to raise to a power with the single on the left side");

                    default:
                        throw new ArgumentException("Invalid operator");

                }

            }

        }

        private static List<object> toPostFix(List<object> input)
        {
            Stack<string> opers = new Stack<string>();

            List<object> postfix = new List<object>();

            foreach(object val in input)
            {
                
                if(val is string)
                {
                    string oper = Convert.ToString(val);
                    

                    // Really MS? Just call it top like cpp
                    if(opers.Count == 0 || (precedences[opers.Peek()] <= precedences[oper]) )
                    {
                        opers.Push(oper);
                    }
                    else
                    {
                        while( opers.Count != 0 && precedences[opers.Peek()] > precedences[oper])
                        {

                            postfix.Add(opers.Pop());
                        }
                        opers.Push(oper);
                    }

                }
                else
                {
                    postfix.Add(val);
                }
            }

            while (opers.Count != 0)
            {
                postfix.Add(opers.Pop());

            }

            return postfix;
        }
    }
}
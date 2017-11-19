using System.Collections.Generic;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using UnityEngine;

using SeaOfGreed.SOGLang;
using System;
using System.Linq;

class CommandListener : LanguageBaseListener
{
    private Stack<object> stack;
    private FunctionExecuter executer = new FunctionExecuter();

	public object result = new object();

    private enum enter {

        EnterMath,
        EnterVector3,
        EnterList,
        EnterDict,
        EnterFunction,
        EnterComparison,
        EnterExtension

    };

    public CommandListener()
    {
        stack = new Stack<object>();
    }

    //******************************************************************************************************

    public override void EnterSingle([NotNull] LanguageParser.SingleContext context)
    {
        stack.Push(float.Parse(context.SINGLE().ToString()));
    }

    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************

    public override void EnterString ([NotNull] LanguageParser.StringContext context)
    {
        stack.Push(context.STRING().ToString().Trim('"'));
    }

    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //*******************************************************************************************************

    public override void EnterNullval([NotNull] LanguageParser.NullvalContext context)
    
    {
        stack.Push(null);      
    }

    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************

    public override void EnterBool([NotNull] LanguageParser.BoolContext context)
    {
        string boolstring = context.BOOL().ToString();

        stack.Push(boolstring == "true");
    }
    
    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************
    

    public override void EnterVector3([NotNull] LanguageParser.Vector3Context context)
    {
        stack.Push(enter.EnterVector3);
    }

    
    public override void ExitVector3([NotNull] LanguageParser.Vector3Context context)
    {
        List<float> elements = new List<float>();
        object currentElement = stack.Pop();

       
        // Add all three floats of the vector3 to the list
        while(!currentElement.Equals(enter.EnterVector3))
        {
            elements.Add(Convert.ToSingle(currentElement));

            currentElement = stack.Pop();
            
        }

        // Push the vector3 to the stack
        // In reverse because we get them in reverse
        stack.Push(new Vector3(elements[2], elements[1], elements[0]));

    }

    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************
    public override void EnterMath([NotNull] LanguageParser.MathContext context)
    {
        stack.Push(enter.EnterMath);
    }

    public override void EnterOperator([NotNull] LanguageParser.OperatorContext context)
    {
        stack.Push(context.OPERATOR().ToString());
    }

    public override void ExitMath([NotNull] LanguageParser.MathContext context)
    {
        List<object> values = new List<object>();

        object currentElement = stack.Pop();

        while(!currentElement.Equals(enter.EnterMath))
        {
            values.Add(currentElement);
            currentElement = stack.Pop();
        }

        values.Reverse();

        stack.Push(Calculator.calculate(values));
    }
    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************

    public override void EnterList(LanguageParser.ListContext context)
    {
        stack.Push(enter.EnterList);
    }

    public override void ExitList(LanguageParser.ListContext context)
    {
        List<object> values = new List<object>();

        object currentElement = stack.Pop();

        while(!currentElement.Equals(enter.EnterList))
        {
            values.Add(currentElement);
            currentElement = stack.Pop();
        }

        values.Reverse();

        stack.Push(values);
    }
    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************

    public override void EnterDict(LanguageParser.DictContext context)
    {
        stack.Push(enter.EnterDict);
    }

    public override void ExitDict(LanguageParser.DictContext context)
    {
        Dictionary<object, object> dict = new Dictionary<object, object>();

        object currentElement = stack.Pop();
        
        while(!currentElement.Equals(enter.EnterDict))
        {


            dict[stack.Pop()] = currentElement;

            currentElement = stack.Pop();
        }

        dict.Reverse();
        
        stack.Push(dict);
    }

    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************
    
    public override void EnterFunction([NotNull] LanguageParser.FunctionContext context)
    {
        stack.Push(enter.EnterFunction);
    }

    public override void ExitFunction([NotNull] LanguageParser.FunctionContext context)
    {
        List<object> values = new List<object>();

        object currentElement = stack.Pop();

        while (!currentElement.Equals(enter.EnterFunction))
        {
            values.Add(currentElement);
            currentElement = stack.Pop();
        }

        values.Reverse();

        // First element
        ParserTypes.FName fname = new ParserTypes.FName(((ParserTypes.FName)values[0]).name);

        List<ParserTypes.FArg> args = new List<ParserTypes.FArg>();

        // All but the first one
        for(int i = 1; i < values.Count; i++)
        {
            args.Add(new ParserTypes.FArg(values[i]));
        }

        stack.Push(executer.execute(fname, args));

    }

    public override void EnterFname([NotNull] LanguageParser.FnameContext context)
    {
        string fname = context.FNAME().ToString();

        stack.Push(new ParserTypes.FName(fname));
    }

    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************

    public override void EnterComparison([NotNull] LanguageParser.ComparisonContext context)
    {
        stack.Push(enter.EnterComparison);
    }

    public override void ExitComparison([NotNull] LanguageParser.ComparisonContext context)
    {
        List<object> values = new List<object>();

        object currentElement = stack.Pop();

        while (!currentElement.Equals(enter.EnterComparison))
        {
            values.Add(currentElement);
            currentElement = stack.Pop();
        }

        string comparator = ((ParserTypes.Comparator)values[1]).name;

        stack.Push(Comparer.compare(values[2], values[0], comparator));
    }

    public override void EnterComparator([NotNull] LanguageParser.ComparatorContext context)
    {
        string comparator = context.COMPARATOR().ToString();

        stack.Push(new ParserTypes.Comparator(comparator));
    }

    //******************************************************************************************************

    //--------------------------------------------------------------------------------------------------------------------

    //******************************************************************************************************

    public override void EnterExtension([NotNull] LanguageParser.ExtensionContext context)
    {
        stack.Push(enter.EnterExtension);
    }

    public override void ExitExtension([NotNull] LanguageParser.ExtensionContext context)
    {
        List<object> values = new List<object>();

        object currentElement = stack.Pop();

        while (!currentElement.Equals(enter.EnterExtension))
        {
            values.Add(currentElement);
            currentElement = stack.Pop();
        }

        stack.Push(Extender.extend(values[2], values[0], ((ParserTypes.Extender)values[1]).name));
    }

    public override void EnterExtender([NotNull] LanguageParser.ExtenderContext context)
    {
        string extender = context.EXTENDER().ToString();

        stack.Push(new ParserTypes.Extender(extender));
    }

    public override void ExitCommand([NotNull] LanguageParser.CommandContext context)
    {
		this.result = stack.Pop();
    }

    //******************************************************************************************************
}


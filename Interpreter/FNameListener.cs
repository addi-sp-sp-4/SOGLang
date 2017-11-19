using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using UnityEngine;

public class FNameListener : LanguageBaseListener
{
    public List<string> fnames;

    public FNameListener()
    {
        fnames = new List<string>(); 
    }

    public override void EnterFname([NotNull] LanguageParser.FnameContext context)
    {
        fnames.Add(context.FNAME().ToString());
    }
}
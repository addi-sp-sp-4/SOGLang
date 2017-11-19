using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SeaOfGreed.SOGLang
{
    public static class Interpreter
    {
        // So we do not have to parse reocurring commands again
        // This should improve performance
        public static Dictionary<string, IParseTree> parseTrees = new Dictionary<string, IParseTree>();

        public static object interpret(string query)
        {
            IParseTree tree;
            string querynospaces = query.Replace(" ", "");

            if (parseTrees.ContainsKey(querynospaces))
            {
                tree = parseTrees[querynospaces];
            }
            else
            {

                AntlrInputStream inputStream = new AntlrInputStream(query);
                LanguageLexer lexer = new LanguageLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                LanguageParser parser = new LanguageParser(commonTokenStream);
                parser.BuildParseTree = true;

                parser.AddErrorListener(new CommandErrorListener());

                tree = parser.command();
                parseTrees[querynospaces] = tree;
            }

            ParseTreeWalker walker = new ParseTreeWalker();

			CommandListener listener = new CommandListener();
		    

            walker.Walk(listener, tree);

			return listener.result;

        }

        public static List<string> getFnames(string query)
        {
            IParseTree tree;
            string querynospaces = query.Replace(" ", "");

            if (parseTrees.ContainsKey(querynospaces))
            {
                tree = parseTrees[querynospaces];
            }
            else
            {

                AntlrInputStream inputStream = new AntlrInputStream(query);
                LanguageLexer lexer = new LanguageLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                LanguageParser parser = new LanguageParser(commonTokenStream);
                parser.BuildParseTree = true;

                parser.AddErrorListener(new CommandErrorListener());

                tree = parser.command();
                parseTrees[querynospaces] = tree;
            }

            ParseTreeWalker walker = new ParseTreeWalker();

            FNameListener listener = new FNameListener();


            walker.Walk(listener, tree);

            return listener.fnames;
        }

        
    }

}
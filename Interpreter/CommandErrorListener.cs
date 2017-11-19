using System;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;


namespace SeaOfGreed.SOGLang
{

    public class CommandErrorListener :  BaseErrorListener
    {
        public override void SyntaxError(System.IO.TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new InvalidOperationException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class TinyParser
    {
        private List<Token> tokens;
        private int index = 0;
        private Token currentToken;

        public TinyParser(List<Token> tokens)
        {
            this.tokens = tokens;
            if (tokens.Count > 0)
                currentToken = tokens[index];
        }
        private void Match(string expected)
        {
            if (index < tokens.Count && (currentToken.Value == expected || currentToken.Type == expected))
            {
                index++;
                if (index < tokens.Count)
                    currentToken = tokens[index];

            }
            else
            {
                throw new Exception($"Syntax error: expected {expected} but found {currentToken?.Type}");
            }
        }
        public void ParseProgram()
        {
            ParseStatements();
        }
        private void ParseStatements()
        {
            ParseStatement();
            if (index < tokens.Count && currentToken.Value == ";")
            {
                Match(";");
                if (index < tokens.Count && currentToken.Value != "}" && currentToken.Value != ";")
                {
                    ParseStatements();
                }
            }
        }
        private void ParseStatement()
        {
            if (index >= tokens.Count) return;
            if (currentToken.Value == "num" || currentToken.Value == "text")
            {
                ParseDecleration();
            }
            else if (currentToken.Type == "Identifier")
            {
                ParseAssignment();
            }
            else if (currentToken.Value == "check")
            {
                ParseCheck();
            }
            else if (currentToken.Value == "repeat")
            {
                ParseRepeat();
            }
            else if (currentToken.Value == "{")
            {
                ParseBlock();
            }
            else
            {
                throw new Exception($"Syntax error: unexpected token {currentToken.Value}");
            }
        }
        private void ParseDecleration()
        {
            if (currentToken.Value == "num") Match("num");
            else Match("text");

            ParseAssignment();

        }
        private void ParseAssignment()
        {
            if (currentToken.Type != "Identifier")
            {
                throw new Exception($"Syntax error: expected identifier but found {currentToken.Type}");
            }
            Match("Identifier");
            Match(":=");
            ParseExpression();
        }
        private void ParseCheck()
        {
            Match("check");
            Match("(");
            ParseCondition();
            Match(")");
            ParseBlock();

            if (index < tokens.Count && currentToken.Value == "otherwise")
            {
                Match("otherwise");
                ParseBlock();
            }
        }
        private void ParseRepeat()
        {
            Match("repeat");
            ParseBlock();
            Match("until");
            Match("(");
            ParseCondition();
            Match(")");
        }
        private void ParseBlock()
        {
            if (currentToken.Value == "{")
            {
                Match("{");
                ParseStatements();
                Match("}");
            }
            else
            {
                ParseStatement();
            }
        }
        private void ParseCondition()
        {
            ParseExpression();
            if (currentToken.Value == "<" || currentToken.Value == ">" || currentToken.Value == "==")
                Match(currentToken.Value);
            else
                throw new Exception("expected Relation operator (<,>,==<!=)");
            ParseExpression();     
        }
        private void ParseExpression()
        {
            ParseTerm();
            while (index < tokens.Count && (currentToken.Value == "+" || currentToken.Value == "-"))
            {
                Match(currentToken.Value);
                ParseTerm();
            }
        }
        private void ParseTerm()
        {
            ParseFactor();
            while (index < tokens.Count && (currentToken.Value == "*" || currentToken.Value == "/"))
            {
                Match(currentToken.Value);
                ParseFactor();
            }
        }
        private void ParseFactor()
        {
            if (currentToken.Type == "Identifier") Match("Identifier");
            else if (currentToken.Type == "number") Match("number");
            else if (currentToken.Value == "(")
            {
                Match("(");
                ParseExpression();
                Match(")");
            }
            else
            {
                throw new Exception($"Syntax error: unexpected token {currentToken.Value}");
            }
        }

    } 
}
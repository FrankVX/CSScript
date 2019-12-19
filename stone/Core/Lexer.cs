using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Stone
{
    public class Lexer
    {
        const string pattern = "\\s*((//.*)|" +
            "([0-9]+([.]{1}[0-9]+){0,1})|" +
            "(\"((\\\\\"|\\\\\\\\|\\\\n|[^\"])*)\")|" +
            "([A-Z_a-z][A-Z_a-z0-9]*)|" +
            "\\-=|\\+=|==|<=|>=|&&|\\|\\||\\p{P}|[=+-<>])?";

        Regex regex = new Regex(pattern);

        List<Token> tokens = new List<Token>();

        public Token Next()
        {
            var t = tokens[0];
            tokens.RemoveAt(0);
            return t;
        }

        public Token Peek(int n = 0)
        {
            var t = tokens[n];
            return t;
        }

        public void Read(string str)
        {
            var match = regex.Match(str);
            while (match.Success)
            {
                var gs = match.Groups;
                if (gs[1].Value != "" && gs[2].Value == "")//不是注释
                {
                    Token token;
                    if (gs[3].Value != "") //数字
                    {
                        token = new Token(Token.TokenType.Number, gs[3].Value);
                    }
                    else if (gs[6].Value != "")
                    {
                        token = new Token(Token.TokenType.String, gs[6].Value);
                    }
                    else if (gs[8].Value != "")
                    {
                        token = new Token(Token.TokenType.Identifier, gs[8].Value);
                    }
                    else
                    {
                        token = new Token(Token.TokenType.Other, gs[1].Value);
                    }
                    tokens.Add(token);
                }
                match = match.NextMatch();
            }
            tokens.Add(new Token(Token.TokenType.EOF, "EOF"));
        }
    }

    public class Token
    {
        public enum TokenType
        {
            Number,
            String,
            Identifier,
            Other,
            EOF,
        }
        public TokenType type;
        public string value;

        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public override string ToString()
        {
            return $"type:{type} value:{value}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    class ASTLeaf : ASTNode
    {
        internal Token token { get; private set; }
        public ASTLeaf(Token t)
        {
            token = t;
        }

        internal string StrValue => token.value;
        internal long IntValue => long.Parse(token.value);
        internal double FloatValue => double.Parse(token.value);

        internal override object eval(IEnviroment env)
        {
            switch (token.type)
            {
                case Token.TokenType.Number:
                    if (token.value.Contains('.'))
                        return double.Parse(token.value);
                    else
                        return int.Parse(token.value);
                case Token.TokenType.String:
                    return token.value;
                case Token.TokenType.Identifier:
                    return env.Get(token.value);
                case Token.TokenType.Other:
                case Token.TokenType.EOF:
                    return null;
            }
            return base.eval(env);
        }
    }
}

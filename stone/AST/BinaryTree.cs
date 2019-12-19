using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    class BinaryTree : ASTTree
    {
        const int TRUE = 1, FALSE = 0;
        internal override object eval(IEnviroment env)
        {
            var op = ((ASTLeaf)childs[1]).StrValue;
            var right = childs[2].eval(env);

            if (op == "=")
            {
                if (childs[0] is ASTLeaf name && name.token.type == Token.TokenType.Identifier)
                {
                    env.Set(name.StrValue, right);
                    return right;
                }
                throw new Exception("can only assignment to var");
            }

            var left = childs[0].eval(env);

            if (op == "+=")
            {
                if (childs[0] is ASTLeaf name && name.token.type == Token.TokenType.Identifier)
                {
                    var v = computeOp(left, right, "+");
                    env.Set(name.StrValue, v);
                    return v;
                }
                throw new Exception("can only assignment to var");
            }
            if (op == "-=")
            {
                if (childs[0] is ASTLeaf name && name.token.type == Token.TokenType.Identifier)
                {
                    var v = computeOp(left, right, "-");
                    env.Set(name.StrValue, v);
                    return v;
                }
                throw new Exception("can only assignment to var");
            }
            return computeOp(left, right, op);
        }

        object computeOp(object left, object right, string op)
        {
            if (left is int il && right is int ir)
                return computeNumber(il, ir, op);
            else if (left is string || right is string)
                return left.ToString() + right.ToString();
            else if (left is double || right is double)
                return computeNumber(Convert.ToDouble(left), Convert.ToDouble(right), op);
            throw new Exception($"can not operator between {left} an {right}");
        }


        object computeNumber(int a, int b, string op)
        {
            if (op.Equals("+"))
                return a + b;
            else if (op.Equals("-"))
                return a - b;
            else if (op.Equals("*"))
                return a * b;
            else if (op.Equals("/"))
                return a / b;
            else if (op.Equals("%"))
                return a % b;
            else if (op.Equals("=="))
                return a == b ? TRUE : FALSE;
            else if (op.Equals(">"))
                return a > b ? TRUE : FALSE;
            else if (op.Equals("<"))
                return a < b ? TRUE : FALSE;
            else
                throw new Exception("bad operator");
        }

        object computeNumber(double a, double b, string op)
        {
            if (op.Equals("+"))
                return a + b;
            else if (op.Equals("-"))
                return a - b;
            else if (op.Equals("*"))
                return a * b;
            else if (op.Equals("/"))
                return a / b;
            else if (op.Equals("%"))
                return a % b;
            else if (op.Equals("=="))
                return a == b ? TRUE : FALSE;
            else if (op.Equals(">"))
                return a > b ? TRUE : FALSE;
            else if (op.Equals("<"))
                return a < b ? TRUE : FALSE;
            else
                throw new Exception("bad operator");
        }
    }
}

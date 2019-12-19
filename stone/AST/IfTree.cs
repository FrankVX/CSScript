using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.AST
{
    internal class IfTree : ASTTree
    {
        ASTNode condition => childs[0];
        ASTNode ifBlock => childs[1];
        ASTNode elseBlock => childs[2];
        bool hasElse => childs.Count == 3;

        internal override object eval(IEnviroment env)
        {
            var state = (int)condition.eval(env);
            if (state > 0)
                return ifBlock.eval(env);
            else if (hasElse)
                return elseBlock.eval(env);
            return state;
        }
    }
}

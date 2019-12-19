using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.AST
{
    internal class WhileTree : ASTTree
    {
        ASTNode condition => childs[0];
        ASTNode block => childs[1];
        internal override object eval(IEnviroment env)
        {
            object r = null;
            while ((int)condition.eval(env) > 0)
            {
                r = block.eval(env);
            }
            return r;
        }
    }
}

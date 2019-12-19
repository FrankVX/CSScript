using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.AST
{
    internal class PrimaryTree : ASTTree
    {
        ASTNode id => childs[0];
        ASTNode postfix => childs[1];
        internal override object eval(IEnviroment env)
        {
            var value = id.eval(env);
            if (value is NativeFunction nfunc)
            {
                if (nfunc.numOfParameters > 0)
                {
                    object[] parms = new object[nfunc.numOfParameters];
                    if (postfix.Name == "postfix")
                    {
                        for (int i = 0; i < parms.Length; i++)
                        {
                            parms[i] = (postfix as ASTTree).childs[i].eval(env);
                        }
                    }
                    else
                    {
                        parms[0] = postfix.eval(env);
                    }
                    return nfunc.invoke(parms);

                }
                return nfunc.invoke(null);
            }
            return value;
        }
    }
}

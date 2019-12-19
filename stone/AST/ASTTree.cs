using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    class ASTTree : ASTNode
    {
        internal List<ASTNode> childs { get; private set; }

        internal virtual void SetChilds(List<ASTNode> nodes)
        {
            childs = nodes;
        }

        internal override object eval(IEnviroment env)
        {
            object value = null;
            foreach (var c in childs)
            {
                if (c is ASTLeaf aSTLeaf && aSTLeaf.token.type == Token.TokenType.EOF)
                    continue;
                value = c.eval(env);
            }
            return value;
        }

    }
}

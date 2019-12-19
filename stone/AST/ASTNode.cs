using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    internal class ASTNode
    {
        internal string Name { get; set; }

        internal virtual object eval(IEnviroment env)
        {
            throw new Exception("error!");
        }
    }
}

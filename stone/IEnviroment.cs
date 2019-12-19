using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    public interface IEnviroment
    {
        object Get(string name);
        void Set(string name, object value);
    }
}

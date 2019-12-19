using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    public class BaseEnv : IEnviroment
    {
        Dictionary<string, object> env = new Dictionary<string, object>();
        public object Get(string name)
        {
            if (env.ContainsKey(name))
                return env[name];
            return 0;
        }
        public void Set(string name, object obj)
        {
            env[name] = obj;
        }
    }
}

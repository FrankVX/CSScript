using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Stone
{
    class Function
    {

    }

    class NativeFunction
    {
        MethodInfo method;
        ParameterInfo[] parameters;
        public  int numOfParameters => parameters.Length;

        public NativeFunction(MethodInfo method)
        {
            this.method = method;
            parameters = method.GetParameters();
        }
        internal object invoke(object[] args)
        {
            try
            {
                return method.Invoke(null, args);
            }
            catch (Exception e)
            {
                throw new Exception("bad native function call: " + method.Name);
            }
        }
    }
}

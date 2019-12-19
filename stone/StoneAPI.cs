using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    public static class StoneAPI
    {

        public static IEnviroment CreatEnv()
        {
            var e = new BaseEnv();

            var ms = typeof(NativeInterface).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var m in ms)
            {
                var f = new NativeFunction(m);
                e.Set(m.Name, f);
            }
            return e;
        }

        public static object DoScript(IEnviroment env, string str)
        {
            var lexer = new Lexer();
            var p = new BaseParser();
            lexer.Read(str);
            var tree = p.Parse(lexer);
            var value = tree.eval(env);
            return value;
        }
    }
}

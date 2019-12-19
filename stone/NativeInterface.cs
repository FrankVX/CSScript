using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    class NativeInterface
    {
        public static void print(object arg)
        {
            Console.WriteLine(arg.ToString());
        }

        public static int add(int a,int b)
        {
            return a + b;
        }
    }
}

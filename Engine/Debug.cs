using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.Engine
{
    public static class Debug
    {
        static public void Log(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }
    }
}

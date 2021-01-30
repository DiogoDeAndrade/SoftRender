using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender
{
    static class Time
    {
        public static float deltaTime
        {
            get { return Application.deltaTime; }
        }
        public static float time
        {
            get { return Application.currentTime; }
        }
    }
}

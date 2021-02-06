using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.Engine
{
    public static class Time
    {
        public static float deltaTime
        {
            get { return Application.current.deltaTime; }
        }
        public static float time
        {
            get { return Application.current.currentTime; }
        }

        public static double tickCount
        {
            get { return DateTime.Now.Ticks; }
        }
    }
}

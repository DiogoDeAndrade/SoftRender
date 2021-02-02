using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.UnityApp
{
    public class Behaviour : Component
    {
        public bool enabled = true;

        public bool isActiveAndEnabled
        {
            get
            {
                if (!enabled) return false;
                if (!gameObject.activeInHierarchy) return false;

                return true;
            }
        }
    }
}

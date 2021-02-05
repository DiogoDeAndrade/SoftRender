using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.UnityApp
{
    public class Behaviour : Component
    {
        bool _enabled = true;
        protected MethodInfo onEnableFunction;
        protected MethodInfo onDisableFunction;

        public bool enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled == value) return;
                _enabled = value;

                if (_enabled)
                {
                    onEnableFunction.Invoke(this, null);
                }
                else
                {
                    onDisableFunction.Invoke(this, null);
                }
            }
        }

        public bool isActiveAndEnabled
        {
            get
            {
                if (!enabled) return false;
                if (!gameObject.activeInHierarchy) return false;

                return true;
            }
        }

        public override void InitUnityComponent(bool waitLoad = false)
        {
            base.InitUnityComponent(waitLoad);

            var type = GetType();
            onEnableFunction = GetMethod(type, "OnEnable");
            onDisableFunction = GetMethod(type, "OnDisable");
        }

        protected MethodInfo GetMethod(System.Type type, string methodName)
        {
            return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

    }
}

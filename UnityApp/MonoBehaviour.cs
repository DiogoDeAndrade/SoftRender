using System.Collections.Generic;
using System.Reflection;

namespace SoftRender.UnityApp
{
    public class MonoBehaviour : Component
    {
        struct Callable
        {
            public Callable(MethodInfo m, object o ) { method = m; obj = o; }

            public MethodInfo  method;
            public object      obj;
        };
        static List<Callable> updateBehaviours = new List<Callable>();

        static public void RunUpdate()
        {
            foreach (var toUpdate in updateBehaviours)
            {
                object[] parameters = null;
                toUpdate.method.Invoke(toUpdate.obj, parameters);
            }
        }

        public MonoBehaviour()
        {
        }

        public override void InitUnityComponent()
        {
            base.InitUnityComponent();

            var type = GetType();
            var updateMethod = type.GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (updateMethod != null)
            {
                updateBehaviours.Add(new Callable(updateMethod, this));
            }
        }
    }
}

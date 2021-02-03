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
        static List<Callable> postRenderBehaviours = new List<Callable>();

        static public void RunUpdate()
        {
            RunMethod(updateBehaviours);
        }

        static public void RunOnPostUpdate()
        {
            RunMethod(postRenderBehaviours);
        }

        private static void RunMethod(List<Callable> behaviourList)
        {
            foreach (var toUpdate in behaviourList)
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
            AddMethodToList(type, "Update", updateBehaviours);
            AddMethodToList(type, "OnPostRender", postRenderBehaviours);
        }

        void AddMethodToList(System.Type type, string methodName, List<Callable> behaviourList)
        {
            var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                behaviourList.Add(new Callable(method, this));
            }
        }
    }
}

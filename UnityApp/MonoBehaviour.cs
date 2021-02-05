using System.Collections.Generic;
using System.Reflection;

namespace SoftRender.UnityApp
{
    public class MonoBehaviour : Behaviour
    {
#region Static behaviour
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
                toUpdate.method.Invoke(toUpdate.obj, null);
            }
        }
#endregion

        public MonoBehaviour()
        {
        }

        public override void InitUnityComponent(bool waitLoad = false)
        {
            base.InitUnityComponent();

            var type = GetType();
            AddMethodToList(type, "Update", updateBehaviours);
            AddMethodToList(type, "OnPostRender", postRenderBehaviours);

            // waitLoad specifies that Awake, etc will be done at a later time (for future scene loading)
            if (waitLoad) return;

            var method = GetMethod(type, "Awake");
            if (method != null)
            {
                object[] parameters = null;
                method.Invoke(this, parameters);
            }

            if (enabled)
            {
                onEnableFunction?.Invoke(this, null);
            }

            method = GetMethod(type, "Start");
            if (method != null)
            {
                method.Invoke(this, null);
            }
        }

        void AddMethodToList(System.Type type, string methodName, List<Callable> behaviourList)
        {
            var method = GetMethod(type, methodName);
            if (method != null)
            {
                behaviourList.Add(new Callable(method, this));
            }
        }
    }
}

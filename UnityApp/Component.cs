using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.UnityApp
{
    public class Component : Object
    {
        public GameObject gameObject;

        public Transform  transform { get => gameObject.transform; }

        public Component() : base("")
        {

        }

        public virtual void InitUnityComponent(bool waitLoad = false)
        {

        }

        public T GetComponent<T>() where T : Component => gameObject.GetComponent<T>();
        public T GetComponentInParent<T>() where T : Component => gameObject.GetComponentInParent<T>();
        public T GetComponentInChildren<T>() where T : Component => gameObject.GetComponentInChildren<T>();
        public T[] GetComponents<T>() where T : Component => gameObject.GetComponents<T>();
        public T[] GetComponentsInChildren<T>() where T : Component => gameObject.GetComponentsInChildren<T>();
    }
}

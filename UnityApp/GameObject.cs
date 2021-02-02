using System.Collections.Generic;
using SoftRender.UnityApp.SceneManagement;

namespace SoftRender.UnityApp
{
    public class GameObject : Object
    {
        public Transform transform;
        public Scene     scene;
        public int       layer = 0;

        bool            activerotation = true;
        List<Component> components;

        public bool activeInHierarchy
        {
            get { return activerotation; }
        }

        public GameObject(string name = "") : base(name)
        {
            components = new List<Component>();

            transform = new Transform();
            components.Add(transform);

            SceneManager.GetActiveScene().AddObject(this);
        }

        ~GameObject()
        {
            scene.RemoveObject(this);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T newComponent = new T();

            newComponent.InitUnityComponent();

            newComponent.gameObject = this;

            components.Add(newComponent);

            return newComponent;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (var component in components)
            {
                if (component is T) return component as T;
            }

            return null;
        }
        public T GetComponentInChildren<T>() where T : Component
        {
            foreach (var component in components)
            {
                if (component is T) return component as T;
            }
            foreach (var child in transform.children)
            {
                var ret = child.gameObject.GetComponentInChildren<T>();

                if (ret != null) return ret;
            }

            return null;
        }
        public T GetComponentInParent<T>() where T : Component
        {
            foreach (var component in components)
            {
                if (component is T) return component as T;
            }

            var parent = transform.GetParent();
            return (parent == null)?(null):(parent.gameObject.GetComponentInParent<T>());
        }

        public T[] GetComponents<T>() where T : Component
        {
            List<T> ret = new List<T>();

            foreach (var component in components)
            {
                if (component is T) ret.Add(component as T);
            }

            return ret.ToArray();
        }

        public void GetComponents<T>(List<T> ret) where T : Component
        {
            foreach (var component in components)
            {
                if (component is T) ret.Add(component as T);
            }
        }

        public T[] GetComponentsInChildren<T>() where T : Component
        {
            List<T> ret = new List<T>();

            GetComponents<T>(ret);
            foreach (var child in transform.children)
            {
                child.gameObject.GetComponentsInChildren(ret);
            }

            return ret.ToArray();
        }

        public void GetComponentsInChildren<T>(List<T> ret) where T : Component
        {
            GetComponents<T>(ret);
            foreach (var child in transform.children)
            {
                child.gameObject.GetComponentsInChildren(ret);
            }
        }
    }
}

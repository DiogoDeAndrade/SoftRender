using System.Collections.Generic;
using SoftRender.Engine;

namespace SoftRender.UnityApp.SceneManagement
{
    public class Scene : Object
    {
        public Color ambientLight = Color.black;
        public float fogStart = 5.0f;
        public float fogEnd = 20.0f;
        public Color fogColor = Color.black;

        List<GameObject> objects = new List<GameObject>();

        public Scene(string name) : base(name)
        {

        }

        public void AddObject(GameObject go)
        {
            if (go.scene == this) return;

            if ((go.scene != this) && (go.scene != null))
            {
                go.scene.RemoveObject(go);
            }
            go.scene = this;

            objects.Add(go);
        }

        public void RemoveObject(GameObject go)
        {
            go.scene = null;
            objects.Remove(go);
        }
    }
}

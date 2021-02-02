using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.UnityApp.SceneManagement
{
    public class Scene : Object
    {
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

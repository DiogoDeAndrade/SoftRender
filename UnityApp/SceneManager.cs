using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.UnityApp.SceneManagement
{
    public static class SceneManager
    {
        static Scene        activeScene = null;
        static List<Scene>  loadedScenes;

        public static int sceneCount { get { return (loadedScenes == null) ? (0) : (loadedScenes.Count); } }

        static public   Scene GetActiveScene() { return activeScene; }
        static public   Scene CreateScene(string name)
        {
            var scene = new Scene(name);

            if (activeScene == null)
            {
                activeScene = scene;
            }
            if (loadedScenes == null)
            {
                loadedScenes = new List<Scene>();
            }
            loadedScenes.Add(scene);

            return scene;
        }
    }
}

using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; //the best boi
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.System_Stuff
{
    public class SceneManager
    {
        private Game _game;
        private Scene currentScene;
        public int NextScene = 0;
        private IEnumerable<Scene> _scenes;

        public SceneManager(Game game)
        {
            this._game = game;
            IEnumerable<Scene> scenes = ReflectiveEnumerator.GetEnumerableOfType<Scene>();
            foreach (Scene testt in scenes)
            {
                Console.WriteLine(testt.name);
            }
            this._scenes = scenes;
        }

        public void LoadScene(int index)
        {
            NextScene = index;
            _game.Running = false;

        }

        public List<GameObject> initScene()
        {
            Console.WriteLine(this.NextScene);
            if (this.currentScene != null)
            {
                //end the scene

            }
            currentScene = _scenes.Where(x => x.index == this.NextScene).First();
            //Console.WriteLine($"{currentScene.index} is the current index and {nextScene} is the goal");
            currentScene.Start();
            return currentScene.initialObjs;

        }



        public static class ReflectiveEnumerator
        {
            static ReflectiveEnumerator() { }

            public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable
            {
                List<T> objects = new List<T>();
                foreach (Type type in
                    Assembly.GetAssembly(typeof(T)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
                {
                    objects.Add((T)Activator.CreateInstance(type, constructorArgs));
                }
                objects.Sort();
                return objects;
            }
        }

    }
}

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
        private State state = State.Test;
        private Game game;
        private Scene currentScene;
        public int nextScene = 0;
        public IEnumerable<Scene> scenes;

        public SceneManager(Game gamee)
        {
            this.game = gamee;
            IEnumerable<Scene> test = ReflectiveEnumerator.GetEnumerableOfType<Scene>();
            foreach (Scene testt in test)
            {
                Console.WriteLine(testt.name);
            }
            scenes = test;
        }

        public void loadScene(int index)
        {
            nextScene = index;
            game.running = false;

        }

        public List<GameObject> initScene()
        {
            Console.WriteLine(this.nextScene);
            if (this.currentScene != null)
            {
                //end the scene

            }
            currentScene = scenes.Where(x => x.index == this.nextScene).First();
            //Console.WriteLine($"{currentScene.index} is the current index and {nextScene} is the goal");
            currentScene.Start();
            return currentScene.initialObjs;

        }
        public enum State { Test, Menu, Game, Score }



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

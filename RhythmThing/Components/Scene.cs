using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Components
{
    public abstract class Scene : IComparable
    {
        //list of objects to be parsed by the game
        public List<GameObject> initialObjs;
        public string name;
        public int index;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Scene otherScene = obj as Scene;
            if (otherScene != null)
            {
                return this.index.CompareTo(otherScene.index);

            }
            else
            {

                throw new ArgumentException("Object is not scene");
            }

        }

        //where said objects are created and aded
        public abstract void Start();

        //dunno if this is worth it.
        public abstract void Update();

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.System_Stuff
{
    public enum objType { visual, nonvisual };
    //simplified implementatino of my engine
    public abstract class GameObject
    {
        public objType GameObjectType;
        public bool alive = true;
        public List<Component> Components = new List<Component>();
        public abstract void Update(double time, Game game);
        public abstract void Start(Game game);
        public abstract void End();
    }
    public abstract class Component
    {
        //component is used to interface with system stuff with the exception of input
        public string ComponentType;
        public bool Active;


        public abstract void Update(double time);
    }
}

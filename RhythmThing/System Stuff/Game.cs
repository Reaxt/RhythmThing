using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using RhythmThing.Objects;

namespace RhythmThing.System_Stuff
{
    public class Game
    {
        //these three variables are for various scoring elements
        public static float approachSpeed = 5300;
        public static float scoringTime = 100;
        public static float missTime = 250;
        private int frames = 0;
        private float timePassed = 0;
        //a ref to the game instance JUST in case
        public static Game mainInstance;
        //the current "State" of the game loop
        public bool running = true;
        //whether or not the game loop is running
        public static bool gameLoopLives = true;
        //the dimensions of the console window. Used to be for setting, now just for various calculations
        public int screenX, screenY;
        //a variable that represents the time since the last update, used for making frame independant things
        public double deltaTime;
        //the display class, calculates objects into coords
        public Display display;
        //the class to manage "scenes"
        public SceneManager sceneManager;
        //object to handle input. This will make it easy for rebinds and the likes.
        public Input input;
        //various game object lists for the loop
        private List<GameObject> gameObjects;
        private List<GameObject> gameObjectsToAdd;
        private List<GameObject> addBuffer;
        private List<GameObject> toRemove;

        //audio manager
        public AudioManager audioManager;
        public bool exitViaEsc = false;
        //these are for storing information between scenes.
        public string ChartToLoad;
        public int totalNotes = 100;
        public int notesHit = 100;
        public string songHash = "1tb56k0XFKSw49gL2xm0lA==";
        public string songName = "Test song name";
        //private List<GameObject> toRemove;
        //for our good ol friend deltatime
        Stopwatch stopwatch = new Stopwatch();
        public Game(int screenX, int screenY)
        {
            this.screenX = screenX;
            this.screenY = screenY;
            mainInstance = this;
            gameObjects = new List<GameObject>();
            gameObjectsToAdd = new List<GameObject>();
            toRemove = new List<GameObject>();
            audioManager = new AudioManager();
            /*
            Console.WriteLine("Loading console handle thingy");
            Console.WriteLine("Totally a real loading message and not just me taking time to seem cool");
            Console.WriteLine("ok thats enough heres the game");
            Thread.Sleep(1000);
            Console.WriteLine("\n\n Welcome to rhythm thing!! This game is going to be continued after with more feautures likely. For now, the controls are the arrow keys and enter. Please make sure to select \"lessons by dj\" first! ");
            Console.WriteLine("This bit is just for the assignment build. Please enjoy!! Press any key to load the actual game.");
            Console.ReadKey();
            */
            display = new Display();
            addBuffer = new List<GameObject>();
            input = Input.Instance;
            deltaTime = 0;
            sceneManager = new SceneManager(this);
            //entry point

            sceneManager.loadScene(0);

            //debug scene
            //sceneManager.loadScene(5);

            while (gameLoopLives)
            {


                this.Loop();

            }

            //YER DONE
            //TODO: Find out why this takes so long.
            Environment.Exit(0);
        }

        public void addGameObject(GameObject gameObject)
        {
            addBuffer.Add(gameObject);
        }
        public void removeGameObject(GameObject gameObject)
        {
            //DEPRACATED: CHECK IF OBJ IS ALIVE
        }

        private void Start()
        {
            //init shit goes here kind of
            //Console.SetWindowSize(screenX, screenY);



            //put in test object
            //mainInstance.addGameObject(new LeftArrow());
            // mainInstance.addGameObject(new RightArrow());
            //mainInstance.addGameObject(new DownArrow());
            //mainInstance.addGameObject(new UpArrow());
            //mainInstance.addGameObject(new Chart("Metronome80"));

            //Loop();
        }
        void Loop()
        {
            //kinda nothing yet

            //changing scene stuff
            foreach (GameObject obj in gameObjects)
            {
                obj.alive = false;
                obj.End();
            }

            addBuffer = new List<GameObject>(sceneManager.initScene());
            //mainInstance.addGameObject(new Chart("Nisemono"));
            running = true;
            while (running)
            {
                stopwatch.Start();
                gameObjectsToAdd = new List<GameObject>(addBuffer);
                addBuffer.Clear();
                foreach (GameObject obj in gameObjectsToAdd)
                {
                    obj.Start(this);
                    gameObjects.Add(obj);

                    if (obj.type == objType.visual)
                    {
                        display.AddObject(obj);
                    }
                }
                gameObjectsToAdd.Clear();
                //calculate objects that exist
                toRemove.Clear();
                foreach (GameObject obj in gameObjects)
                {
                    if (obj.alive)
                    {
                        //run the components first or last?
                        foreach (Component comp in obj.components)
                        {
                            if (comp.active) comp.Update(deltaTime);
                        }

                        obj.Update(deltaTime, this);
                    }
                    else
                    {
                        toRemove.Add(obj);
                        obj.End();
                    }
                }
                foreach (GameObject obj in toRemove)
                {
                    gameObjects.Remove(obj);
                    display.RemoveObject(obj);
                }


                display.DrawFrame(deltaTime);



                input.UpdateInput();
                // your code

                Thread.Sleep(1); //just in case
                stopwatch.Stop();
                deltaTime = stopwatch.ElapsedMilliseconds * 0.001;

                //calculate framerate
                frames++;
                if (timePassed > 1000)
                {
                    Console.Title = $"FPS: {frames}";
                    frames = 0;
                    timePassed = 0;
                }
                timePassed += stopwatch.ElapsedMilliseconds;

                stopwatch.Reset();
                //Console.WriteLine("frame");
            }
        }
    }
}

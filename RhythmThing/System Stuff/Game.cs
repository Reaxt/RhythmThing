using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RhythmThing.System_Stuff
{
    public class Game
    {
        //these three variables are for various scoring elements
        public static float ApproachSpeed = 5300;
        public static float ScoringTime = 100;
        public static float MissTime = 250;
        private int _frames = 0;
        private float _timePassed = 0;
        //a ref to the game instance JUST in case
        public static Game MainInstance;
        //the current "State" of the game loop
        public bool Running = true;
        //whether or not the game loop is running
        public static bool GameLoopLives = true;
        //the dimensions of the console window. Used to be for setting, now just for various calculations
        public int ScreenX, ScreenY;
        //a variable that represents the time since the last update, used for making frame independant things
        private double _deltaTime;
        //the display class, calculates objects into coords
        public Display DisplayInstance;
        //the class to manage "scenes"
        public SceneManager SceneManagerInstance;
        //object to handle input. This will make it easy for rebinds and the likes.
        public Input InputInstance;
        //various game object lists for the loop
        private List<GameObject> _gameObjects;
        private List<GameObject> _gameObjectsToAdd;
        private List<GameObject> _addBuffer;
        private List<GameObject> _toRemove;

        //audio manager
        public AudioManager AudioManagerInstance;
        public bool ExitViaEsc = false;
        //these are for storing information between scenes.
        public string ChartToLoad;
        public int TotalNotes = 100;
        public int NotesHit = 100;
        public string SongHash = "1tb56k0XFKSw49gL2xm0lA==";
        public string SongName = "Test song name";
        //private List<GameObject> toRemove;
        //for our good ol friend deltatime
        private Stopwatch _stopwatch = new Stopwatch();

        

        public Game(int screenX, int screenY)
        {
            this.ScreenX = screenX;
            this.ScreenY = screenY;
            MainInstance = this;
            _gameObjects = new List<GameObject>();
            _gameObjectsToAdd = new List<GameObject>();
            _toRemove = new List<GameObject>();
            AudioManagerInstance = new AudioManager();
            /*
            Console.WriteLine("Loading console handle thingy");
            Console.WriteLine("Totally a real loading message and not just me taking time to seem cool");
            Console.WriteLine("ok thats enough heres the game");
            Thread.Sleep(1000);
            Console.WriteLine("\n\n Welcome to rhythm thing!! This game is going to be continued after with more feautures likely. For now, the controls are the arrow keys and enter. Please make sure to select \"lessons by dj\" first! ");
            Console.WriteLine("This bit is just for the assignment build. Please enjoy!! Press any key to load the actual game.");
            Console.ReadKey();
            */
            DisplayInstance = new Display();
            _addBuffer = new List<GameObject>();
            InputInstance = Input.Instance;
            _deltaTime = 0;
            SceneManagerInstance = new SceneManager(this);
            //entry point

            SceneManagerInstance.LoadScene(0);

            //debug scene
            //sceneManager.loadScene(5);

            while (GameLoopLives)
            {


                this.Loop();

            }

            //YER DONE
            //TODO: Find out why this takes so long.
            Environment.Exit(0);
        }

        public void addGameObject(GameObject gameObject)
        {
            _addBuffer.Add(gameObject);
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
            foreach (GameObject obj in _gameObjects)
            {
                obj.alive = false;
                obj.End();
            }
            //get rid of any shaders
            DisplayInstance.DisableFilter();
            //kill any slaves
            SlaveManager.CloseAll();
            _addBuffer = new List<GameObject>(SceneManagerInstance.initScene());
            //mainInstance.addGameObject(new Chart("Nisemono"));
            Running = true;
            while (Running)
            {
                _stopwatch.Start();
                _gameObjectsToAdd = new List<GameObject>(_addBuffer);
                _addBuffer.Clear();
                foreach (GameObject obj in _gameObjectsToAdd)
                {
                    obj.Start(this);
                    _gameObjects.Add(obj);

                    if (obj.GameObjectType == objType.visual)
                    {
                        DisplayInstance.AddObject(obj);
                    }
                }
                _gameObjectsToAdd.Clear();
                //calculate objects that exist
                _toRemove.Clear();
                foreach (GameObject obj in _gameObjects)
                {
                    if (obj.alive)
                    {
                        //run the components first or last?
                        foreach (Component comp in obj.Components)
                        {
                            if (comp.Active) comp.Update(_deltaTime);
                        }

                        obj.Update(_deltaTime, this);
                    }
                    else
                    {
                        _toRemove.Add(obj);
                        obj.End();
                    }
                }
                foreach (GameObject obj in _toRemove)
                {
                    _gameObjects.Remove(obj);
                    DisplayInstance.RemoveObject(obj);
                }


                DisplayInstance.DrawFrame(_deltaTime);



                InputInstance.UpdateInput();
                // your code

                Thread.Sleep(1); //just in case
                _stopwatch.Stop();
                _deltaTime = _stopwatch.ElapsedMilliseconds * 0.001;

                //calculate framerate
                _frames++;
                if (_timePassed > 1000)
                {
                    Console.Title = $"FPS: {_frames}";
                    _frames = 0;
                    _timePassed = 0;
                }
                _timePassed += _stopwatch.ElapsedMilliseconds;

                _stopwatch.Reset();
                //Console.WriteLine("frame");
            }
        }
    }
}

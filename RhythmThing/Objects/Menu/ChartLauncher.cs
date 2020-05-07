using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;

namespace RhythmThing.Objects.Menu
{
    class ChartLauncher : GameObject
    {
        private Chart chart;
        public override void End()
        {


        }

        public override void Start(Game game)
        {
            //Console.WriteLine("ok");
            components = new List<Component>();
            chart = new Chart(game.ChartToLoad);
            
            game.addGameObject(chart);


        }

        public override void Update(double time, Game game)
        {

        }
    }
}

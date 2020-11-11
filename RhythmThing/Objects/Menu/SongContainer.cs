using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;
using RhythmThing.Components;

namespace RhythmThing.Objects.Menu
{
    public class SongContainer : GameObject
    {
        public string chartName;
        public Visual visual;
        public Chart chart;
        public int pos;
        public SongContainer(string chartName, int pos)
        {
            this.chartName = chartName;
            chart = new Chart(chartName);
            this.pos = pos;
            this.type = objType.visual;

            //sorry for moving this here "('w')

            components = new List<Component>();

        }
        public override void End()
        {
            //throw new NotImplementedException();
        }




        public override void Start(Game game)
        {


        }

        public override void Update(double time, Game game)
        {
        }
    }
}

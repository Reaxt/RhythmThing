using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.System_Stuff
{
    public interface SongScript
    {
        string Name { get; }
        string Description { get; }
        //will be ran ONCE on chart start
        void runScript();
        //will be ran in the chart update function
        void mainScript();

    }
}

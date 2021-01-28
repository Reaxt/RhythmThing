﻿using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Objects;

namespace RhythmThing.System_Stuff
{
    public interface SongScript
    {
        string Name { get; }
        string Description { get; }
        //will be ran ONCE on chart start
        void RunScript(Chart chart, Game game);
        //will be ran in the chart update function
        void MainScript(Chart chart, Game game, double time);
        //ran at the end if you wanna unload shit
        void EndScript(Chart chart, Game game);

    }
}

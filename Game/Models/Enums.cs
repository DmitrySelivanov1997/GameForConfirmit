﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Models
{

    public enum TypesOfObject
    {
        FreeSpace,
        Food,
        UnitWhite,
        UnitBlack,
        BaseWhite,
        BaseBlack,
        Brick,
        Border,
        FogOfWar
    }
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        Stay
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace CommonInterfaceLibrary
{
    interface IItem
    {
        Color Color { get; set; }
        int X { get; }
        int Y { get; }
    }
}

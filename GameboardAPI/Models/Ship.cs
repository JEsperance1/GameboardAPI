using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    public class Ship
    {
        public string shipType;
        public int spaces;
        public bool isHorizontal;
        public (int, int) startCoordinate;
        public Ship(string ShipType, int num_spaces)
            
        {
            this.shipType = ShipType;
            this.spaces = num_spaces;
            this.isHorizontal = true;
        }
    }
}

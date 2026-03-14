using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    public class Ship
    {
        public string ShipType;
        public int spaces;
        public bool isHorizontal;
        public (int, int) startCoordinate;
        public int[] hitsArray;
        public Ship(string ShipType, int num_spaces)
            
        {
            this.ShipType = ShipType;
            this.spaces = num_spaces;
            this.isHorizontal = true;
            this.hitsArray = new int[spaces];
        }
        public void createHitsArray()
        {
            Array.Resize(ref this.hitsArray, this.spaces);
        }
        public bool IsSunk()
        {
            return hitsArray.Sum() == spaces;
        }
    }
}

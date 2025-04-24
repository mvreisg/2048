using UnityEngine;

namespace Game.Classes
{
    public class Tile
    {
        public Vector2Int Coordinates { get; set; }

        public int Value { get; set; }

        public GameObject GameObject { get; set; }
    }
}
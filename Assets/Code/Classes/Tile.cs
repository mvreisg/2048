using UnityEngine;

namespace Game.Classes
{
    public class Tile
    {
        public Tile(Vector2Int coordinates, int value, GameObject gameObject)
        {
            Coordinates = coordinates;
            Value = value;
            GameObject = gameObject;
        }

        public GameObject GameObject { get; set; }

        public Vector2Int Coordinates { get; set; }

        public int Value { get; set; }
    }
}
using UnityEngine;

namespace Game.Classes
{
    public class Slot
    {
        public Slot(Vector2Int coordinates, GameObject gameObject)
        {
            Coordinates = coordinates;
            GameObject = gameObject;
        }

        public GameObject GameObject { get; set; }

        public Tile Tile { get; set; }

        public Vector2Int Coordinates { get; private set; }

        public bool IsOccupied { 
            get
            {
                return Tile != null;
            }
        }
    }
}
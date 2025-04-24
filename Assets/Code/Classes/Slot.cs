using UnityEngine;
using Game.Scripts;

namespace Game.Classes
{
    public class Slot
    {
        public Slot(GameObject gameObject, Vector2Int coordinates)
        {
            this.GameObject = gameObject;
            this.Coordinates = coordinates;
        }

        public GameObject GameObject { get; private set; }

        public Tile Tile { get; set; }

        public Vector2Int Coordinates { get; private set; }

        public bool IsOccupied
        {
            get
            {
                return Tile != null;
            }
        }
    }
}
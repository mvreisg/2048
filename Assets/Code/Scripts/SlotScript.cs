using Game.Classes;
using UnityEngine;

namespace Game.Scripts
{
    public class SlotScript : MonoBehaviour
    {
        public Slot Slot { get; set; }

        [SerializeField]
        private Vector2Int coordinates;

        private void Awake()
        {
            Slot = new Slot(coordinates, gameObject);
        }
    }
}
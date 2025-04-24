using Game.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class TileScript : MonoBehaviour
    {    
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI textBox;

        public Tile Tile { get; set; }

        private void Awake()
        {
            Tile = new Tile();            
        }

        private void Start()
        {
            image.color = Color.blue;
            textBox.text = Tile.Value.ToString();
        }
    }
}
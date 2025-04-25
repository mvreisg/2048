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

        [SerializeField]
        private Color twoColor;

        [SerializeField]
        private Color fourColor;

        [SerializeField]
        private Color eightColor;

        [SerializeField]
        private Color sixteenColor;

        [SerializeField]
        private Color thirtyTwoColor;

        [SerializeField]
        private Color sixtyFourColor;

        [SerializeField]
        private Color oneHundredTwentyEightColor;

        [SerializeField]
        private Color twoHundredFiftySixColor;

        [SerializeField]
        private Color fiveHundredTwelveColor;

        [SerializeField]
        private Color oneThousandTwentyFourColor;

        [SerializeField]
        private Color twoThousandFortyEightColor;

        public Tile Tile { get; set; }

        private void Awake()
        {
            Tile = new Tile();            
        }

        private void Start()
        {            
            UpdateText();
            UpdateColor();
        }

        public void UpdateText()
        {
            textBox.text = Tile.Value.ToString();
        }

        public void UpdateColor()
        {
            if (Tile.Value < 8)
            {
                textBox.color = Color.black;
            }
            else
            {
                textBox.color = Color.white;
            }

            switch (Tile.Value)
            {
                case 2:

                    image.color = twoColor;
                    break;
                case 4:
                    image.color = fourColor;
                    break;
                case 8:
                    image.color = eightColor;
                    break;
                case 16:
                    image.color = sixteenColor;
                    break;
                case 32:
                    image.color = thirtyTwoColor;
                    break;
                case 64:
                    image.color = sixtyFourColor;
                    break;
                case 128:
                    image.color = oneHundredTwentyEightColor;
                    break;
                case 256:
                    image.color = twoHundredFiftySixColor;
                    break;
                case 512:
                    image.color = fiveHundredTwelveColor;
                    break;
                case 1024:
                    image.color = oneThousandTwentyFourColor;
                    break;
                case 2048:
                    image.color = twoThousandFortyEightColor;
                    break;
            }
        }
    }
}
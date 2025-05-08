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

        private Color initialColor;

        public Color InitialColor
        {
            set { initialColor = value; }
        }

        private Color finalColor;

        public Color FinalColor
        {
            set { finalColor = value; }
        }
        
        public Tile Tile { get; set; }

        private void Start()
        {            
            UpdateText();
            UpdateColor();

            GameObject.Find("InitialColorButton").GetComponent<ColorPickerButtonScript>().OnPointerClickEvent += UpdateInitialColor;
            GameObject.Find("FinalColorButton").GetComponent<ColorPickerButtonScript>().OnPointerClickEvent += UpdateFinalColor;
        }

        private void UpdateInitialColor(Color color)
        {
            initialColor = color;
            UpdateColor();
        }

        private void UpdateFinalColor(Color color)
        {
            finalColor = color;
            UpdateColor();
        }

        public void UpdateText()
        {
            textBox.text = Tile.Value.ToString();
        }

        public void UpdateColor()
        {
            textBox.color = Color.white;

            float value = Mathf.Log(Tile.Value, 2);
            float maxValue = 11;
            if (value > maxValue)
            {
                image.color = finalColor;
                return;
            }
            image.color = Color.Lerp(initialColor, finalColor, value / maxValue);
        }

        public void OnDestroy()
        {
            GameObject.Find("InitialColorButton").GetComponent<ColorPickerButtonScript>().OnPointerClickEvent -= UpdateInitialColor;
            GameObject.Find("FinalColorButton").GetComponent<ColorPickerButtonScript>().OnPointerClickEvent -= UpdateFinalColor;
        }
    }
}
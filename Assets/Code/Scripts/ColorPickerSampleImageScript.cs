using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class ColorPickerSampleImageScript : MonoBehaviour
    {
        [SerializeField]
        private ColorPickerButtonScript colorPickerButtonScript;

        private Color actualColor;

        private void Awake()
        {
            actualColor = colorPickerButtonScript.SelectedColor;
            colorPickerButtonScript.OnPointerClickEvent += SetColor;
            Paint();
        }

        private void SetColor(Color color)
        {
            actualColor = color;
            Paint();
        }

        private void Paint()
        {
            GetComponent<RawImage>().color = actualColor;
        }
    }
}
using Game.Classes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class ColorPickerButtonScript : MonoBehaviour, IPointerClickHandler
    {
        public delegate void OnPointerClickHandler(Color newColor);

        public event OnPointerClickHandler OnPointerClickEvent;

        private RawImage image;
        private Texture2D texture;

        [SerializeField]
        private IntegerDimensions textureDimensions;

        [SerializeField]
        private Color selectedColor;

        public Color SelectedColor => selectedColor;

        private void Awake()
        {
            image = GetComponent<RawImage>();
            texture = new Texture2D(textureDimensions.width, textureDimensions.height, TextureFormat.RGB24, false);
            texture.filterMode = FilterMode.Point;

            for (int x = 0; x < textureDimensions.width; x++)
            {
                float hue = (float)x / textureDimensions.width;
                for (int y = 0; y < textureDimensions.height; y++)
                {
                    float value = 1f - (float)y / textureDimensions.height;
                    Color color = Color.HSVToRGB(hue, 1f, value);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            image.texture = texture;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,  // importante para o Canvas em modo World ou Camera
                out localPoint
            );

            // Transformar ponto local (centrado em 0,0) para relativo ao canto inferior esquerdo
            Vector2 rectSize = rectTransform.rect.size;
            Vector2 pivot = rectTransform.pivot;
            Vector2 relativePoint = localPoint + (rectSize * pivot);

            RawImage image = GetComponent<RawImage>();
            Texture2D texture = image.texture as Texture2D;
            selectedColor = texture.GetPixel((int)relativePoint.x, (int)relativePoint.y);

            OnPointerClickEvent?.Invoke(selectedColor);
        }
    }
}
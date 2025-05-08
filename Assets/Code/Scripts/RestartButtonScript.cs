using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class RestartButtonScript : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        public bool IsInteractable
        {
            get
            {
                return button.interactable;
            }
        }

        public void Enable()
        {
            button.interactable = true;
        }

        public void Disable()
        {
            button.interactable = false;
        }
    }
}
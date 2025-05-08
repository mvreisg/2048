using System;
using UnityEngine;

namespace Game.Classes
{
    [Serializable]
    public class IntegerDimensions
    {
        [SerializeField]
        public int width;

        [SerializeField]
        public int height;

        public IntegerDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }        
    }
}
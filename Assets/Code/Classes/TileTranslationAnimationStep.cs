using System;
using UnityEngine;

namespace Game.Classes
{
    public class TileTranslationAnimationStep
    {
        public GameObject GameObject { get; set; }

        public Vector3 StartPoint { get; set; }

        public Vector3 EndPoint { get; set; }

        public float ElapsedTime { get; set; }

        public float TotalTime { get; set; }
        
        public Action FinishCallback {  get; set; }

        public bool CanContinue
        {
            get
            {
                return ElapsedTime <= TotalTime;
            }
        }

        public void Update()
        {            
            Vector3 actualPoint = StartPoint;
            if (CanContinue)
            {
                actualPoint = Vector3.Lerp(StartPoint, EndPoint, ElapsedTime / TotalTime);
                ElapsedTime += Time.deltaTime;
            }
            else
            {                
                actualPoint = EndPoint;                
            }
            GameObject.transform.position = actualPoint;
        }
    }
}
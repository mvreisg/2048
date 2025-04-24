using Game.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class PanelScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject slot_0_0;

        [SerializeField]
        private GameObject slot_0_1;

        [SerializeField]
        private GameObject slot_0_2;

        [SerializeField]
        private GameObject slot_0_3;

        [SerializeField]
        private GameObject slot_1_0;

        [SerializeField]
        private GameObject slot_1_1;

        [SerializeField]
        private GameObject slot_1_2;

        [SerializeField]
        private GameObject slot_1_3;

        [SerializeField]
        private GameObject slot_2_0;

        [SerializeField]
        private GameObject slot_2_1;

        [SerializeField]
        private GameObject slot_2_2;

        [SerializeField]
        private GameObject slot_2_3;

        [SerializeField]
        private GameObject slot_3_0;

        [SerializeField]
        private GameObject slot_3_1;

        [SerializeField]
        private GameObject slot_3_2;

        [SerializeField]
        private GameObject slot_3_3;

        [SerializeField]
        private GameObject TilePrefab;

        private List<AnimationStep> animationStepsList = new List<AnimationStep>();

        private Panel panel;

        private void Awake()
        {
            panel = new Panel(4, 4);
            panel.Slots[0, 0] = new Slot(slot_0_0, new Vector2Int(0, 0));
            panel.Slots[0, 1] = new Slot(slot_0_1, new Vector2Int(0, 1));
            panel.Slots[0, 2] = new Slot(slot_0_2, new Vector2Int(0, 2));
            panel.Slots[0, 3] = new Slot(slot_0_3, new Vector2Int(0, 3));
            panel.Slots[1, 0] = new Slot(slot_1_0, new Vector2Int(1, 0));
            panel.Slots[1, 1] = new Slot(slot_1_1, new Vector2Int(1, 1));
            panel.Slots[1, 2] = new Slot(slot_1_2, new Vector2Int(1, 2));
            panel.Slots[1, 3] = new Slot(slot_1_3, new Vector2Int(1, 3));
            panel.Slots[2, 0] = new Slot(slot_2_0, new Vector2Int(2, 0));
            panel.Slots[2, 1] = new Slot(slot_2_1, new Vector2Int(2, 1));
            panel.Slots[2, 2] = new Slot(slot_2_2, new Vector2Int(2, 2));
            panel.Slots[2, 3] = new Slot(slot_2_3, new Vector2Int(2, 3));
            panel.Slots[3, 0] = new Slot(slot_3_0, new Vector2Int(3, 0));
            panel.Slots[3, 1] = new Slot(slot_3_1, new Vector2Int(3, 1));
            panel.Slots[3, 2] = new Slot(slot_3_2, new Vector2Int(3, 2));
            panel.Slots[3, 3] = new Slot(slot_3_3, new Vector2Int(3, 3));
        }

        private void Start()
        {
            Vector2Int firstSlotToInstantiate = GetRandomSlotNumber();
            Vector2Int secondSlotToInstantiate = GetRandomSlotNumber();
            while (firstSlotToInstantiate == secondSlotToInstantiate)
            {
                secondSlotToInstantiate = GetRandomSlotNumber();
            }

            GameObject firstTileInstantiated = InstantiateTile(firstSlotToInstantiate.x, firstSlotToInstantiate.y);
            GameObject secondTileInstantiated = InstantiateTile(secondSlotToInstantiate.x, secondSlotToInstantiate.y);
        }

        private bool isAnimationsHappening;
        private bool isToLockInput;

        private void Update()
        {
            animationStepsList.ForEach((item) => {
                item.Update();
            });

            animationStepsList.RemoveAll((item) => {
                return item.CanContinue == false;
            });

            isAnimationsHappening = animationStepsList.Count > 0;

            if (isAnimationsHappening)
            {
                Debug.Log("animaçoes acontecendo");
                return;
            }
            else
            {
                isToLockInput = false;
            }

            bool left = Input.GetKeyDown(KeyCode.A);
            if (left && isToLockInput == false)
            {
                Debug.Log("para a esquerda");
                StartMovingAnimationToTheLeft();
                isToLockInput = true;
                return;
            }
            Debug.Log("fim do update");
        }

        private Vector2Int GetRandomSlotNumber()
        {
            int x = Random.Range(0, 3);
            int y = Random.Range(0, 3);
            return new Vector2Int(x, y);
        }

        private GameObject InstantiateTile(int x, int y)
        {
            GameObject gameObject = Instantiate<GameObject>(
                TilePrefab,
                panel.Slots[x, y].GameObject.transform.position,
                Quaternion.Euler(Vector3.zero),
                this.transform
            );

            TileScript script = gameObject.GetComponent<TileScript>();
            
            script.Tile.GameObject = script.gameObject;
            script.Tile.Value = 2;
            script.Tile.Coordinates = new Vector2Int(x, y);

            panel.Slots[x, y].Tile = script.Tile;

            return gameObject;
        }

        private void StartMovingAnimationToTheLeft()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Slot slot = panel.Slots[x, y];

                    if (slot.IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = IterateToLeft(x, y);

                    if (actualCoordinate == finalCoordinate)
                    {
                        continue;
                    }
                    
                    Slot targetSlot = panel.Slots[finalCoordinate.x, finalCoordinate.y];

                    AnimationStep animationStep = new AnimationStep();
                    animationStep.GameObject = slot.Tile.GameObject;
                    animationStep.StartPoint = slot.GameObject.transform.position;
                    animationStep.EndPoint = targetSlot.GameObject.transform.position;
                    animationStep.TotalTime = 1f;
                    animationStepsList.Add(animationStep);

                    panel.Slots[finalCoordinate.x, finalCoordinate.y].Tile = slot.Tile;
                    panel.Slots[actualCoordinate.x, actualCoordinate.y].Tile = null;
                }
            }
        }

        private Vector2Int IterateToLeft(int startX, int y)
        {
            Vector2Int vector = new Vector2Int(startX, y);
            for (int x = startX; x > 0; x--)
            {
                if (TryMoveToLeft(x, y) == false)
                    break;

                vector = new Vector2Int(x - 1, y);
            }
            return vector;
        }

        private bool TryMoveToLeft(int x, int y)
        {
            if (x - 1 < 0)
                return false;

            Slot slot = panel.Slots[x - 1, y];

            if (slot.IsOccupied)
                return false;

            return true;
        }
    }
}
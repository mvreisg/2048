using Game.Classes;
using Game.Enums;
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

        private bool isAnimationsHappening;

        private bool isToLockInput;

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
            Vector2Int firstSlotToInstantiate = new Vector2Int(0, 0);// GetRandomSlotNumber();
            Vector2Int secondSlotToInstantiate = new Vector2Int(1, 0); // GetRandomSlotNumber();
            while (firstSlotToInstantiate == secondSlotToInstantiate)
            {
                secondSlotToInstantiate = GetRandomSlotNumber();
            }

            InstantiateTile(firstSlotToInstantiate.x, firstSlotToInstantiate.y);
            InstantiateTile(secondSlotToInstantiate.x, secondSlotToInstantiate.y);
        }

        private void Update()
        {
            animationStepsList.ForEach((item) => {
                item.Update();
            });

            animationStepsList.ForEach((item) => {
                if (item.CanContinue == false)
                {
                    item.FinishCallback?.Invoke();
                }                
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
            int x = Random.Range(0, 4);
            int y = Random.Range(0, 4);
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
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int xx = x; xx > 0; xx--)
                    {
                        operation = TryMoveToLeft(xx, y);
                        if (operation == SlotOperations.Stop)
                            break;

                        finalCoordinate = new Vector2Int(xx - 1, y);

                        if (operation == SlotOperations.Fuse)
                            break;
                    }

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

                    GameObject gameObjectToChange = slot.Tile.GameObject;
                    GameObject gameObjectToDelete = targetSlot.Tile.GameObject;
                    if (operation == SlotOperations.Fuse)
                    {                        
                        animationStep.FinishCallback = () =>
                        {
                            gameObjectToChange.GetComponent<TileScript>().Tile.Value += gameObjectToChange.GetComponent<TileScript>().Tile.Value;
                            gameObjectToChange.GetComponent<TileScript>().UpdateText();
                            Destroy(gameObjectToDelete);
                        };
                    }
                    animationStepsList.Add(animationStep);

                    panel.Slots[finalCoordinate.x, finalCoordinate.y].Tile = slot.Tile;
                    panel.Slots[actualCoordinate.x, actualCoordinate.y].Tile = null;
                }
            }
        }

        private SlotOperations TryMoveToLeft(int x, int y)
        {
            if (x - 1 < 0)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot leftSlot = panel.Slots[x - 1, y];

            if (leftSlot.IsOccupied && leftSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (leftSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }
    }
}
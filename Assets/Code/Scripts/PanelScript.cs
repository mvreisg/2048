using Game.Classes;
using Game.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class PanelScript : MonoBehaviour
    {
        [SerializeField]
        private SlotScript slot_0_0;

        [SerializeField]
        private SlotScript slot_0_1;

        [SerializeField]
        private SlotScript slot_0_2;

        [SerializeField]
        private SlotScript slot_0_3;

        [SerializeField]
        private SlotScript slot_1_0;

        [SerializeField]
        private SlotScript slot_1_1;

        [SerializeField]
        private SlotScript slot_1_2;

        [SerializeField]
        private SlotScript slot_1_3;

        [SerializeField]
        private SlotScript slot_2_0;

        [SerializeField]
        private SlotScript slot_2_1;

        [SerializeField]
        private SlotScript slot_2_2;

        [SerializeField]
        private SlotScript slot_2_3;

        [SerializeField]
        private SlotScript slot_3_0;

        [SerializeField]
        private SlotScript slot_3_1;

        [SerializeField]
        private SlotScript slot_3_2;

        [SerializeField]
        private SlotScript slot_3_3;

        [SerializeField]
        private GameObject TilePrefab;

        private Queue<AnimationStep>[] animationStepsQueueArray = new Queue<AnimationStep>[4];

        private AnimationStep[] animationSteps = new AnimationStep[4];

        private Panel panel;

        private bool isAnimationsHappening;

        private bool isToLockInput;

        private void Awake()
        {
            panel = new Panel(4, 4);
        }

        private void Start()
        {            
            panel.Slots[0, 0] = slot_0_0.Slot;
            panel.Slots[0, 1] = slot_0_1.Slot;
            panel.Slots[0, 2] = slot_0_2.Slot;
            panel.Slots[0, 3] = slot_0_3.Slot;
            panel.Slots[1, 0] = slot_1_0.Slot;
            panel.Slots[1, 1] = slot_1_1.Slot;
            panel.Slots[1, 2] = slot_1_2.Slot;
            panel.Slots[1, 3] = slot_1_3.Slot;
            panel.Slots[2, 0] = slot_2_0.Slot;
            panel.Slots[2, 1] = slot_2_1.Slot;
            panel.Slots[2, 2] = slot_2_2.Slot;
            panel.Slots[2, 3] = slot_2_3.Slot;
            panel.Slots[3, 0] = slot_3_0.Slot;
            panel.Slots[3, 1] = slot_3_1.Slot;
            panel.Slots[3, 2] = slot_3_2.Slot;
            panel.Slots[3, 3] = slot_3_3.Slot;

            for (int i = 0; i < 2; i++)
            {
                GenerateNewTile();
            }            
        }

        private void Update()
        {
            for (int i = 0; i < animationStepsQueueArray.Length; i++)
            {
                if (animationStepsQueueArray[i] == null)
                    continue;

                if (animationSteps[i] == null && animationStepsQueueArray[i].Count > 0)
                {
                    animationSteps[i] = animationStepsQueueArray[i].Dequeue();
                }
            }
            
            for (int i = 0; i < animationSteps.Length; i++)
            {
                AnimationStep animationStep = animationSteps[i];
                if (animationStep == null)
                    continue;

                animationStep.Update();
                if (animationStep.CanContinue == false)
                {
                    animationStep.Update();
                    animationStep.FinishCallback?.Invoke();
                    animationSteps[i] = null;
                }                
            }

            isAnimationsHappening = false;
            for (int i = 0; i < animationStepsQueueArray.Length; i++)
            {
                if (animationStepsQueueArray[i] == null)
                    continue;

                if (animationStepsQueueArray[i].Count > 0)
                {
                    isAnimationsHappening = true;
                    break;
                }
            }

            if (isAnimationsHappening)
            {
                return;
            }
            else if (isAnimationsHappening == false && isToLockInput)
            {
                isToLockInput = false;
                GenerateNewTile();
                return;
            }

            bool left = Input.GetKeyDown(KeyCode.A);
            bool right = Input.GetKeyDown(KeyCode.D);
            bool top = Input.GetKeyDown(KeyCode.W);
            bool bottom = Input.GetKeyDown(KeyCode.S);
            if (left && right == false && top == false && bottom == false && isToLockInput == false)
            {
                Debug.Log("para a esquerda");
                StartMovingAnimationToTheLeft();
                isToLockInput = true;
                return;
            }

            if (left == false && right && top == false && bottom == false && isToLockInput == false)
            {
                Debug.Log("para a direita");
                StartMovingAnimationToTheRight();
                isToLockInput = true;
                return;
            }

            if (left == false && right == false && top && bottom == false && isToLockInput == false)
            {
                Debug.Log("para cima");
                StartMovingAnimationToTheTop();
                isToLockInput = true;
                return;
            }

            if (left == false && right == false && top == false && bottom && isToLockInput == false)
            {
                Debug.Log("para baixo");
                StartMovingAnimationToTheBottom();
                isToLockInput = true;
                return;
            }
        }

        private Vector2Int GetRandomSlotNumber()
        {
            int x = Random.Range(0, 4);
            int y = Random.Range(0, 4);
            return new Vector2Int(x, y);
        }

        private void InstantiateTile(int x, int y)
        {
            if (panel.HasEmptySlots == false)
                return;

            GameObject gameObject = Instantiate(
                TilePrefab,
                panel.Slots[x, y].GameObject.transform.position,
                Quaternion.Euler(Vector3.zero),
                transform
            );

            TileScript tileScript = gameObject.GetComponent<TileScript>();

            tileScript.Tile = new Tile(new Vector2Int(x, y), 2, tileScript.gameObject);

            panel.Slots[x, y].Tile = tileScript.Tile;

            panel.IncrementAmountOfFilledSlots(1);
        }

        private void GenerateNewTile()
        {
            if (panel.HasEmptySlots == false)
                return;

            Vector2Int coordinate = GetRandomSlotNumber();
            while (CheckIfSlotIsOccupied(coordinate.x, coordinate.y))
            {
                coordinate = GetRandomSlotNumber();
            }
            InstantiateTile(coordinate.x, coordinate.y);
        }

        private bool CheckIfSlotIsOccupied(int x, int y)
        {
            return panel.Slots[x, y].IsOccupied;
        }

        private void MakeAnimationLogic (
            Queue<AnimationStep> animationStepsQueue, 
            SlotOperations operation, 
            Vector2Int actualCoordinate, 
            Vector2Int finalCoordinate) 
        {
            Slot actualSlot = panel.Slots[actualCoordinate.x, actualCoordinate.y];
            Slot finalSlot = panel.Slots[finalCoordinate.x, finalCoordinate.y];

            Tile actualTile = actualSlot.Tile;
            Tile finalTile = finalSlot.Tile;

            GameObject gameObjectToChange = actualTile.GameObject;

            AnimationStep animationStep = new AnimationStep();
            animationStep.GameObject = gameObjectToChange;
            animationStep.StartPoint = actualSlot.GameObject.transform.position;
            animationStep.EndPoint = finalSlot.GameObject.transform.position;
            animationStep.TotalTime = 0.1f;
            actualTile.Coordinates = finalCoordinate;

            if (operation == SlotOperations.Fuse)
            {
                GameObject gameObjectToDelete = finalTile.GameObject;

                actualTile.Value += finalTile.Value;

                animationStep.FinishCallback = () => {
                    TileScript tileScript = gameObjectToChange.GetComponent<TileScript>();
                    tileScript.Tile = actualTile;
                    tileScript.UpdateText();
                    tileScript.UpdateColor();
                    Destroy(gameObjectToDelete);
                    panel.DecrementAmountOfFilledSlots(1);
                };
            }

            panel.Slots[actualCoordinate.x, actualCoordinate.y].Tile = null;
            panel.Slots[finalCoordinate.x, finalCoordinate.y].Tile = actualTile;

            animationStepsQueue.Enqueue(animationStep);
        }

        private void StartMovingAnimationToTheLeft()
        {            
            for (int y = 0; y < 4; y++)
            {
                Queue<AnimationStep> animationStepsQueue = new Queue<AnimationStep>();
                for (int x = 0; x < 4; x++)
                {
                    if (panel.Slots[x, y].IsOccupied == false)
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

                        actualCoordinate = new Vector2Int(xx, y);
                        finalCoordinate = new Vector2Int(xx - 1, y);

                        MakeAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }                                        
                }
                animationStepsQueueArray[y] = animationStepsQueue;
            }
        }

        private void StartMovingAnimationToTheRight()
        {
            for (int y = 0; y < 4; y++)
            {
                Queue<AnimationStep> animationStepsQueue = new Queue<AnimationStep>();
                for (int x = 3; x >= 0; x--)
                {
                    if (panel.Slots[x, y].IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int xx = x; xx < 4; xx++)
                    {
                        operation = TryMoveToRight(xx, y);
                        if (operation == SlotOperations.Stop)
                            break;

                        actualCoordinate = new Vector2Int(xx, y);
                        finalCoordinate = new Vector2Int(xx + 1, y);

                        MakeAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }
                }
                animationStepsQueueArray[y] = animationStepsQueue;
            }            
        }

        private void StartMovingAnimationToTheTop()
        {
            for (int x = 0; x < 4; x++)
            {
                Queue<AnimationStep> animationStepsQueue = new Queue<AnimationStep>();
                for (int y = 0; y < 4; y++)
                {
                    if (panel.Slots[x, y].IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int yy = y; y >= 0; yy--)
                    {
                        operation = TryMoveToTop(x, yy);
                        if (operation == SlotOperations.Stop)
                            break;

                        actualCoordinate = new Vector2Int(x, yy);
                        finalCoordinate = new Vector2Int(x, yy - 1);

                        MakeAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }
                }
                animationStepsQueueArray[x] = animationStepsQueue;
            }            
        }

        private void StartMovingAnimationToTheBottom()
        {            
            for (int x = 0; x < 4; x++)
            {
                Queue<AnimationStep> animationStepsQueue = new Queue<AnimationStep>();
                for (int y = 3; y >= 0; y--)
                {
                    if (panel.Slots[x, y].IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int yy = y; y < 4; yy++)
                    {
                        operation = TryMoveToBottom(x, yy);
                        if (operation == SlotOperations.Stop)
                            break;

                        actualCoordinate = new Vector2Int(x, yy);
                        finalCoordinate = new Vector2Int(x, yy + 1);

                        MakeAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }
                }
                animationStepsQueueArray[x] = animationStepsQueue;
            }
        }

        private SlotOperations TryMoveToLeft(int x, int y)
        {
            if (x - 1 < 0)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot leftSlot = panel.Slots[x - 1, y];
                
            if (leftSlot.IsOccupied && leftSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (leftSlot.IsOccupied && leftSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (leftSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }

        private SlotOperations TryMoveToRight(int x, int y)
        {
            if (x + 1 >= 4)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot rightSlot = panel.Slots[x + 1, y];

            if (rightSlot.IsOccupied && rightSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (rightSlot.IsOccupied && rightSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (rightSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }

        private SlotOperations TryMoveToTop(int x, int y)
        {
            if (y - 1 < 0)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot topSlot = panel.Slots[x, y - 1];

            if (topSlot.IsOccupied && topSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (topSlot.IsOccupied && topSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (topSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }

        private SlotOperations TryMoveToBottom(int x, int y)
        {
            if (y + 1 >= 4)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot bottomSlot = panel.Slots[x, y + 1];

            if (bottomSlot.IsOccupied && bottomSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (bottomSlot.IsOccupied && bottomSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (bottomSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }
    }
}
using Game.Classes;
using Game.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class GameControllerScript : MonoBehaviour
    {
        [SerializeField]
        private PanelScript panelScript;

        [SerializeField]
        private StartGameScript startGameScript;

        [SerializeField]
        private GameOverScript gameOverScript;

        [SerializeField]
        private ScoreScript scoreScript;

        private Queue<AnimationStep>[] animationStepsQueueArray = new Queue<AnimationStep>[4];

        private AnimationStep[] animationSteps = new AnimationStep[4];

        private GameController gameController;

        private bool isAnimationsHappening;

        private bool isToLockInput;        

        private void Awake()
        {
            gameController = new GameController();
        }

        private void Update()
        {
            float submit = Input.GetAxis("Submit");
            if (panelScript.PanelStarted == false)
                return;

            if (gameController.HasFirstStarted == false && submit > 0f)
            {
                startGameScript.HideStartGamePanel();
                gameController.HasFirstStarted = true;
            }
            
            if (gameController.HasFirstStarted == false)
                return;

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

            
            if (gameController.IsGameOver && submit > 0f)
            {
                gameOverScript.HideGameOverPanel();
                StartGame();
            }

            if (gameController.IsGameOver == false && submit > 0f)
            {
                gameOverScript.HideGameOverPanel();
                StartGame();
            }

            if (gameController.IsGameOver)
                return;

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

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal < 0f && isToLockInput == false)
            {
                StartMovingAnimationToTheLeft();
                isToLockInput = true;
                return;
            }

            if (horizontal > 0f && isToLockInput == false)
            {
                StartMovingAnimationToTheRight();
                isToLockInput = true;
                return;
            }

            if (vertical < 0f && isToLockInput == false)
            {
                StartMovingAnimationToTheBottom();
                isToLockInput = true;
                return;
            }

            if (vertical > 0f && isToLockInput == false)
            {
                StartMovingAnimationToTheTop();
                isToLockInput = true;
                return;
            }
        }

        public void StartGame()
        {
            gameController.HasFirstStarted = true;

            scoreScript.ResetScore();

            animationStepsQueueArray = new Queue<AnimationStep>[4];
            animationSteps = new AnimationStep[4];

            gameController.IsGameOver = false;

            panelScript.Panel.ClearAllSlots();

            TileScript[] tileScripts = FindObjectsByType<TileScript>(FindObjectsSortMode.InstanceID);
            if (tileScripts.Length > 0)
            {
                for (int i = 0; i < tileScripts.Length; i++)
                {
                    TileScript script = tileScripts[i];
                    Destroy(script.gameObject);
                }
            }

            for (int i = 0; i < 2; i++)
            {
                GenerateNewTile();
            }
        }

        private void GenerateNewTile()
        {
            if (gameController.IsGameOver)
            {
                return;
            }

            if (panelScript.Panel.HasEmptySlots == false && panelScript.CheckIfMovementIsAllowed() == false)
            {
                gameController.IsGameOver = true;
                gameOverScript.ShowGameOverPanel();
                return;
            }

            if (panelScript.Panel.HasEmptySlots)
            {
                Vector2Int coordinate = panelScript.GetRandomSlotNumber();
                while (panelScript.CheckIfSlotIsOccupied(coordinate.x, coordinate.y))
                {
                    coordinate = panelScript.GetRandomSlotNumber();
                }
                panelScript.InstantiateTile(coordinate.x, coordinate.y);
            }
        }

        private void MakeAnimationLogic(Queue<AnimationStep> animationStepsQueue, SlotOperations operation, Vector2Int actualCoordinate, Vector2Int finalCoordinate)
        {
            Slot actualSlot = panelScript.Panel.Slots[actualCoordinate.x, actualCoordinate.y];
            Slot finalSlot = panelScript.Panel.Slots[finalCoordinate.x, finalCoordinate.y];

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
                scoreScript.IncreaseScoreBy(actualTile.Value);

                animationStep.FinishCallback = () => {
                    TileScript tileScript = gameObjectToChange.GetComponent<TileScript>();
                    tileScript.Tile = actualTile;
                    tileScript.UpdateText();
                    tileScript.UpdateColor();
                    Destroy(gameObjectToDelete);
                };
                panelScript.Panel.DecrementAmountOfFilledSlots(1);
            }

            panelScript.Panel.Slots[actualCoordinate.x, actualCoordinate.y].Tile = null;
            panelScript.Panel.Slots[finalCoordinate.x, finalCoordinate.y].Tile = actualTile;

            animationStepsQueue.Enqueue(animationStep);
        }

        private void StartMovingAnimationToTheLeft()
        {
            for (int y = 0; y < 4; y++)
            {
                Queue<AnimationStep> animationStepsQueue = new Queue<AnimationStep>();
                for (int x = 0; x < 4; x++)
                {
                    if (panelScript.Panel.Slots[x, y].IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int xx = x; xx > 0; xx--)
                    {
                        operation = panelScript.TryMoveToLeft(xx, y);

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
                    if (panelScript.Panel.Slots[x, y].IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int xx = x; xx < 4; xx++)
                    {
                        operation = panelScript.TryMoveToRight(xx, y);
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
                    if (panelScript.Panel.Slots[x, y].IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int yy = y; y >= 0; yy--)
                    {
                        operation = panelScript.TryMoveToTop(x, yy);
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
                    if (panelScript.Panel.Slots[x, y].IsOccupied == false)
                    {
                        continue;
                    }

                    Vector2Int actualCoordinate = new Vector2Int(x, y);
                    Vector2Int finalCoordinate = new Vector2Int(x, y);

                    SlotOperations operation = SlotOperations.Stop;
                    for (int yy = y; y < 4; yy++)
                    {
                        operation = panelScript.TryMoveToBottom(x, yy);
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
    }
}
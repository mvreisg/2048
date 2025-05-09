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
        private VictoryScript victoryScript;

        [SerializeField]
        private ScoreScript scoreScript;

        private GameController gameController;

        private void Awake()
        {
            gameController = new GameController();
        }

        private void Update()
        {
            if (panelScript.PanelStarted == false)
                return;

            bool submitPressed = Input.GetButtonDown("Submit");

            if (gameController.HasFirstStarted == false && submitPressed)
            {
                startGameScript.HideStartGamePanel();
                StartGame();
                return;
            }            

            if (gameController.IsGameOver && submitPressed)
            {
                gameOverScript.HideGameOverPanel();
                RestartGame();
                return;
            }

            if (gameController.IsEndless && submitPressed)
            {
                RestartGame();
                return;
            }

            if (gameController.HasWon && gameController.IsEndless == false && submitPressed)
            {
                victoryScript.HideVictoryPanel();
                StartEndless();
                return;
            }

            if (gameController.HasFirstStarted == false && gameController.HasRestarted == false)
                return;

            if (gameController.IsPaused)
                return;

            for (int i = 0; i < gameController.TileTranslationAnimationStepsQueueArray.Length; i++)
            {
                if (gameController.TileTranslationAnimationStepsQueueArray[i] == null)
                    continue;

                if (gameController.TileTranslationAnimationStepsArray[i] == null && gameController.TileTranslationAnimationStepsQueueArray[i].Count > 0)
                {
                    gameController.TileTranslationAnimationStepsArray[i] = gameController.TileTranslationAnimationStepsQueueArray[i].Dequeue();
                }
            }

            for (int i = 0; i < gameController.TileTranslationAnimationStepsArray.Length; i++)
            {
                TileTranslationAnimationStep animationStep = gameController.TileTranslationAnimationStepsArray[i];
                if (animationStep == null)
                    continue;

                animationStep.Update();
                if (animationStep.CanContinue == false)
                {
                    animationStep.Update();
                    animationStep.FinishCallback?.Invoke();
                    gameController.TileTranslationAnimationStepsArray[i] = null;
                }
            }

            for (int i = 0; i < gameController.TileScalingAnimationStepsList.Count; i++)
            {
                TileScalingAnimationStep animationStep = gameController.TileScalingAnimationStepsList[i];
                if (animationStep == null)
                    continue;

                animationStep.Update();
                if (animationStep.CanContinue == false)
                {
                    animationStep.Update();
                    animationStep.FinishCallback?.Invoke();
                    gameController.TileScalingAnimationStepsList.Remove(animationStep);
                }
            }

            gameController.SetAnimationsHappening(false);
            for (int i = 0; i < gameController.TileTranslationAnimationStepsQueueArray.Length; i++)
            {
                if (gameController.TileTranslationAnimationStepsQueueArray[i] == null)
                    continue;

                if (gameController.TileTranslationAnimationStepsQueueArray[i].Count > 0)
                {
                    gameController.SetAnimationsHappening(true);
                    break;
                }
            }

            if (gameController.TileScalingAnimationStepsList.Count > 0)
            {
                gameController.SetAnimationsHappening(true);                
            }

            if (gameController.IsAnimationsHappening)
                return;

            if (gameController.HasWon == false)
            {
                foreach (Slot slot in panelScript.Panel.Slots)
                {
                    if (slot.Tile == null)
                        continue;

                    Tile tile = slot.Tile;
                    int value = tile.Value;
                    if (scoreScript.CheckVictory(value))
                    {
                        gameController.StartVictory();
                        victoryScript.ShowVictoryPanel();
                        return;
                    }
                }
            }                        
            
            if (gameController.IsAnimationsHappening == false && gameController.IsToSpawnNewTile)
            {
                gameController.SetIsToSpawnNewTile(false);
                GenerateNewTile();
                return;
            }

            if (gameController.IsAnimationsHappening == false && gameController.IsToLockInputs)
            {
                gameController.SetIsToLockInputs(false);
                return;
            }
            
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool left = false;
            bool right = false;
            bool up = false;
            bool down = false;

            if (horizontal < 0f)
            {                
                left = true;
                right = false;
            }

            if (horizontal > 0f)
            {
                left = false;
                right = true;
            }

            if (vertical < 0f)
            {
                up = false;
                down = true;
            }

            if (vertical > 0f)
            {
                up = true;
                down = false;
            }

            if (left && gameController.IsToLockInputs == false)
            {
                StartMovingAnimationToTheLeft();
                gameController.SetIsToLockInputs(true);
                gameController.SetIsToSpawnNewTile(true);                
                return;
            }

            if (right && gameController.IsToLockInputs == false)
            {
                StartMovingAnimationToTheRight();
                gameController.SetIsToLockInputs(true);
                gameController.SetIsToSpawnNewTile(true);
                return;
            }

            if (down && gameController.IsToLockInputs == false)
            {
                StartMovingAnimationToTheBottom();
                gameController.SetIsToLockInputs(true);
                gameController.SetIsToSpawnNewTile(true);
                return;
            }

            if (up && gameController.IsToLockInputs == false)
            {
                StartMovingAnimationToTheTop();
                gameController.SetIsToLockInputs(true);
                gameController.SetIsToSpawnNewTile(true);
                return;
            }
        }

        public void PauseGame()
        {
            gameController.PauseGame();
        }

        public void ResumeGame()
        {
            gameController.ResumeGame();
        }

        private void BootGame()
        {
            scoreScript.ResetScore();

            gameController.ResetTileTranslationAnimationStepsArray();
            gameController.ResetTileTranslationAnimationStepsQueueArray();

            for (int i = 0; i < gameController.TileScalingAnimationStepsList.Count; i++)
            {
                GameObject gameObject = gameController.TileScalingAnimationStepsList[i].GameObject;
                Destroy(gameObject);
            }

            gameController.TileScalingAnimationStepsList.Clear();

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

        public void StartGame()
        {
            gameController.StartGame();                        
            BootGame();
        }

        public void RestartGame()
        {
            gameController.RestartGame();            
            BootGame();
        }

        public void StartEndless()
        {
            victoryScript.HideVictoryPanel();
            gameController.StartEndless();            
        }

        private void GenerateNewTile()
        {
            if (gameController.IsGameOver)
            {
                return;
            }

            if (panelScript.Panel.HasEmptySlots == false && panelScript.CheckIfMovementIsAllowed() == false)
            {
                gameController.StartGameOver();
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
                Tile instantiatedTile = panelScript.InstantiateTile(coordinate.x, coordinate.y);

                if (instantiatedTile == null)
                {
                    return;
                }

                TileScalingAnimationStep step = new TileScalingAnimationStep();
                step.StartPoint = Vector3.zero;
                step.EndPoint = Vector3.one;
                step.TotalTime = 0.1f;
                step.GameObject = instantiatedTile.GameObject;
                    
                gameController.TileScalingAnimationStepsList.Add(step);
            }
        }

        private void MakeTranslationAnimationLogic(Queue<TileTranslationAnimationStep> animationStepsQueue, SlotOperations operation, Vector2Int actualCoordinate, Vector2Int finalCoordinate)
        {
            Slot actualSlot = panelScript.Panel.Slots[actualCoordinate.x, actualCoordinate.y];
            Slot finalSlot = panelScript.Panel.Slots[finalCoordinate.x, finalCoordinate.y];

            Tile actualTile = actualSlot.Tile;
            Tile finalTile = finalSlot.Tile;

            GameObject gameObjectToChange = actualTile.GameObject;

            TileTranslationAnimationStep animationStep = new TileTranslationAnimationStep();
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
                Queue<TileTranslationAnimationStep> animationStepsQueue = new Queue<TileTranslationAnimationStep>();
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

                        MakeTranslationAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }
                }
                gameController.TileTranslationAnimationStepsQueueArray[y] = animationStepsQueue;
            }
        }

        private void StartMovingAnimationToTheRight()
        {
            for (int y = 0; y < 4; y++)
            {
                Queue<TileTranslationAnimationStep> animationStepsQueue = new Queue<TileTranslationAnimationStep>();
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

                        MakeTranslationAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }
                }
                gameController.TileTranslationAnimationStepsQueueArray[y] = animationStepsQueue;
            }
        }

        private void StartMovingAnimationToTheTop()
        {
            for (int x = 0; x < 4; x++)
            {
                Queue<TileTranslationAnimationStep> animationStepsQueue = new Queue<TileTranslationAnimationStep>();
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

                        MakeTranslationAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }
                }
                gameController.TileTranslationAnimationStepsQueueArray[x] = animationStepsQueue;
            }
        }

        private void StartMovingAnimationToTheBottom()
        {
            for (int x = 0; x < 4; x++)
            {
                Queue<TileTranslationAnimationStep> animationStepsQueue = new Queue<TileTranslationAnimationStep>();
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

                        MakeTranslationAnimationLogic(animationStepsQueue, operation, actualCoordinate, finalCoordinate);
                    }
                }
                gameController.TileTranslationAnimationStepsQueueArray[x] = animationStepsQueue;
            }
        }        
    }
}
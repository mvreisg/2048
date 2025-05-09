using System;
using System.Collections.Generic;

namespace Game.Classes
{
    public class GameController
    {
        public bool HasFirstStarted { get; private set; }

        public bool IsGameOver { get; private set; }

        public bool HasWon {  get; private set; }

        public bool IsEndless { get; private set; }

        public bool HasRestarted { get; private set; }

        public bool IsPaused { get; private set; }

        public bool IsAnimationsHappening { get; private set; }

        public bool IsToLockInputs { get; private set; }

        public bool IsToSpawnNewTile { get; private set; }

        public Queue<TileTranslationAnimationStep>[] TileTranslationAnimationStepsQueueArray { get; private set; }

        public TileTranslationAnimationStep[] TileTranslationAnimationStepsArray { get; private set; }

        public List<TileScalingAnimationStep> TileScalingAnimationStepsList { get; private set; }

        public GameController()
        {
            TileTranslationAnimationStepsQueueArray = new Queue<TileTranslationAnimationStep>[4];
            TileTranslationAnimationStepsArray = new TileTranslationAnimationStep[4];
            TileScalingAnimationStepsList = new List<TileScalingAnimationStep>();
        }

        public void ResetTileTranslationAnimationStepsQueueArray()
        {
            TileTranslationAnimationStepsQueueArray = new Queue<TileTranslationAnimationStep>[4];
        }

        public void ResetTileTranslationAnimationStepsArray()
        {
            TileTranslationAnimationStepsArray = new TileTranslationAnimationStep[4];
        }

        public void StartGame()
        {
            IsGameOver = false;
            HasWon = false;
            HasFirstStarted = true;
            IsEndless = false;
            HasRestarted = false;
            ResumeGame();
        }

        public void RestartGame()
        {
            IsGameOver = false;
            HasWon = false;
            IsEndless = false;
            HasRestarted = true;
            ResumeGame();
        }

        public void StartEndless()
        {
            IsEndless = true;
            ResumeGame();
        }

        public void StartVictory()
        {
            HasWon = true;
            PauseGame();
        }

        public void StartGameOver()
        {
            IsGameOver = true;
            PauseGame();
        }

        public void PauseGame()
        {
            IsPaused = true;
        }

        public void ResumeGame()
        {
            IsPaused = false;
        }

        public void SetAnimationsHappening(bool value)
        {
            IsAnimationsHappening = value;
        }

        public void SetIsToSpawnNewTile(bool value)
        {
            IsToSpawnNewTile = value;
        }

        public void SetIsToLockInputs(bool value)
        {
            IsToLockInputs = value;
        }
    }
}
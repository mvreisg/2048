namespace Game.Classes
{
    public class Score
    {
        private readonly int winningScore;

        public Score(int winningScore)
        {
            this.winningScore = winningScore;
        }

        public int ActualValue { get; private set; }   
        
        public int MaxValue { get; private set; }

        public void ResetScore()
        {
            ActualValue = 0;
            TryToUpdateMaxValue();
        }

        public void IncreaseScoreBy(int value)
        {
            ActualValue += value;
            TryToUpdateMaxValue();
        }

        private void TryToUpdateMaxValue()
        {
            if (ActualValue < MaxValue)
                return;

            MaxValue = ActualValue;
        }

        public bool CheckVictory(int value)
        {
            return value >= winningScore;                
        }
    }
}
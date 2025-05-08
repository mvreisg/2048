namespace Game.Classes
{
    public class Score
    {
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

        public bool CheckVictory()
        {
            return ActualValue >= 2048;                
        }
    }
}
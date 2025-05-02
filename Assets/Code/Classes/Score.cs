namespace Game.Classes
{
    public class Score
    {
        public int ActualValue { get; private set; }        

        public void ResetScore()
        {
            ActualValue = 0;
        }

        public void IncreaseScoreBy(int value)
        {
            ActualValue += value;
        }
    }
}
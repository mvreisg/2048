namespace Game.Classes
{
    public class Score
    {
        public int ActualValue { get; private set; }        

        public void IncreaseScoreBy(int value)
        {
            ActualValue += value;
        }
    }
}
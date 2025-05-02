namespace Game.Classes
{
    public class GameOverConditionChecker
    {
        public bool CheckIfCanMove(Slot actual, Slot target)
        {
            return actual.Tile.Value == target.Tile.Value;
        }
    }
}
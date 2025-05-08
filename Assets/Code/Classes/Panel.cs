namespace Game.Classes
{
    public class Panel
    {
        private Slot[,] slots;
        private int width;
        private int length;
        private int amountOfFilledSlots;

        public Panel(int width, int length)
        {
            this.width = width;
            this.length = length;
            slots = new Slot[width, length];
        }

        public Slot[,] Slots
        {
            get
            {
                return slots;
            }
        }

        public bool HasEmptySlots
        {
            get
            {
                return amountOfFilledSlots < width * length;
            }
        }

        public int AmountOfFilledSlots
        {
            get
            {
                return amountOfFilledSlots;
            }
        }

        public void IncrementAmountOfFilledSlots(int amount)
        {
            amountOfFilledSlots += amount;
        }

        public void DecrementAmountOfFilledSlots(int amount)
        {
            amountOfFilledSlots -= amount;
        }

        public void ClearAllSlots()
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Slots[x, y].Tile = null;
                }
            }
            amountOfFilledSlots = 0;
        }
    }
}
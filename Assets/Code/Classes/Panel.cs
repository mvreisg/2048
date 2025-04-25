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
            this.slots = new Slot[width, length];
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
    }
}
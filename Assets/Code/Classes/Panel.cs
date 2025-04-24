namespace Game.Classes
{
    public class Panel
    {
        private Slot[,] slots;
        private int width;
        private int length;

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
    }
}
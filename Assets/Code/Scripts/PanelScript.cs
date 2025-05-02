using Game.Classes;
using Game.Enums;
using UnityEngine;

namespace Game.Scripts
{
    public class PanelScript : MonoBehaviour
    {
        [SerializeField]
        private SlotScript slot_0_0;

        [SerializeField]
        private SlotScript slot_0_1;

        [SerializeField]
        private SlotScript slot_0_2;

        [SerializeField]
        private SlotScript slot_0_3;

        [SerializeField]
        private SlotScript slot_1_0;

        [SerializeField]
        private SlotScript slot_1_1;

        [SerializeField]
        private SlotScript slot_1_2;

        [SerializeField]
        private SlotScript slot_1_3;

        [SerializeField]
        private SlotScript slot_2_0;

        [SerializeField]
        private SlotScript slot_2_1;

        [SerializeField]
        private SlotScript slot_2_2;

        [SerializeField]
        private SlotScript slot_2_3;

        [SerializeField]
        private SlotScript slot_3_0;

        [SerializeField]
        private SlotScript slot_3_1;

        [SerializeField]
        private SlotScript slot_3_2;

        [SerializeField]
        private SlotScript slot_3_3;

        [SerializeField]
        private GameObject TilePrefab;     

        private Panel panel;

        public Panel Panel => panel;

        public bool PanelStarted { get; private set; }

        private void Awake()
        {
            panel = new Panel(4, 4);
        }

        private void Start()
        {            
            panel.Slots[0, 0] = slot_0_0.Slot;
            panel.Slots[0, 1] = slot_0_1.Slot;
            panel.Slots[0, 2] = slot_0_2.Slot;
            panel.Slots[0, 3] = slot_0_3.Slot;
            panel.Slots[1, 0] = slot_1_0.Slot;
            panel.Slots[1, 1] = slot_1_1.Slot;
            panel.Slots[1, 2] = slot_1_2.Slot;
            panel.Slots[1, 3] = slot_1_3.Slot;
            panel.Slots[2, 0] = slot_2_0.Slot;
            panel.Slots[2, 1] = slot_2_1.Slot;
            panel.Slots[2, 2] = slot_2_2.Slot;
            panel.Slots[2, 3] = slot_2_3.Slot;
            panel.Slots[3, 0] = slot_3_0.Slot;
            panel.Slots[3, 1] = slot_3_1.Slot;
            panel.Slots[3, 2] = slot_3_2.Slot;
            panel.Slots[3, 3] = slot_3_3.Slot;
            PanelStarted = true;
        }

        public Vector2Int GetRandomSlotNumber()
        {
            int x = Random.Range(0, 4);
            int y = Random.Range(0, 4);
            return new Vector2Int(x, y);
        }

        public void InstantiateTile(int x, int y)
        {
            if (panel.HasEmptySlots == false)
                return;

            GameObject gameObject = Instantiate(
                TilePrefab,
                panel.Slots[x, y].GameObject.transform.position,
                Quaternion.Euler(Vector3.zero),
                transform
            );

            TileScript tileScript = gameObject.GetComponent<TileScript>();

            tileScript.Tile = new Tile(new Vector2Int(x, y), 2, tileScript.gameObject);

            panel.Slots[x, y].Tile = tileScript.Tile;

            panel.IncrementAmountOfFilledSlots(1);
        }        

        public bool CheckIfMovementIsAllowed()
        {
            GameOverConditionChecker checker = new GameOverConditionChecker();
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Slot actualSlot = panel.Slots[x, y];
                    if (x - 1 >= 0)
                    {
                        Slot leftSlot = panel.Slots[x - 1, y];                        
                        if (checker.CheckIfCanMove(actualSlot, leftSlot))
                            return true;
                    }

                    if (x + 1 <= 3)
                    {
                        Slot rightSlot = panel.Slots[x + 1, y];
                        if (checker.CheckIfCanMove(actualSlot, rightSlot))
                            return true;
                    }

                    if (y - 1 >= 0)
                    {
                        Slot topSlot = panel.Slots[x, y - 1];
                        if (checker.CheckIfCanMove(actualSlot, topSlot))
                            return true;
                    }

                    if (y + 1 <= 3)
                    {
                        Slot bottomSlot = panel.Slots[x, y + 1];
                        if (checker.CheckIfCanMove(actualSlot, bottomSlot))
                            return true;
                    }
                }
            }

            return false;
        }

        public bool CheckIfSlotIsOccupied(int x, int y)
        {
            return panel.Slots[x, y].IsOccupied;
        }

        public SlotOperations TryMoveToLeft(int x, int y)
        {
            if (x - 1 < 0)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot leftSlot = panel.Slots[x - 1, y];

            if (leftSlot.IsOccupied && leftSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (leftSlot.IsOccupied && leftSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (leftSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }

        public SlotOperations TryMoveToRight(int x, int y)
        {
            if (x + 1 >= 4)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot rightSlot = panel.Slots[x + 1, y];

            if (rightSlot.IsOccupied && rightSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (rightSlot.IsOccupied && rightSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (rightSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }

        public SlotOperations TryMoveToTop(int x, int y)
        {
            if (y - 1 < 0)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot topSlot = panel.Slots[x, y - 1];

            if (topSlot.IsOccupied && topSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (topSlot.IsOccupied && topSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (topSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }

        public SlotOperations TryMoveToBottom(int x, int y)
        {
            if (y + 1 >= 4)
                return SlotOperations.Stop;

            Slot actualSlot = panel.Slots[x, y];
            Slot bottomSlot = panel.Slots[x, y + 1];

            if (bottomSlot.IsOccupied && bottomSlot.Tile.Value != actualSlot.Tile.Value)
                return SlotOperations.Stop;

            if (bottomSlot.IsOccupied && bottomSlot.Tile.Value == actualSlot.Tile.Value)
                return SlotOperations.Fuse;

            if (bottomSlot.IsOccupied)
                return SlotOperations.Stop;

            return SlotOperations.Move;
        }
    }
}
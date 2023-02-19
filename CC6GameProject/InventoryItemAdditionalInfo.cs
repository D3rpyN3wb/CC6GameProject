using System;

namespace CC6GameProject
{
    // InventoryItemAdditionalInfo
    // This class gives extra information on the selected inventory item
    // IIAI("Hunger", "100%", ConsoleColor.Green, ConsoleColor.Yellow) creates in the inventory description (green text)Hunger: (yellow text)100%
    class IIAI
    {
        public string label, value;
        public ConsoleColor lColor, vColor;

        public IIAI(string lbl, string val, ConsoleColor lblColor = ConsoleColor.DarkCyan, ConsoleColor vColor = ConsoleColor.White)
        {
            this.label = lbl;
            this.value = val;
            this.lColor = lblColor;
            this.vColor = vColor;
        }
    }
}

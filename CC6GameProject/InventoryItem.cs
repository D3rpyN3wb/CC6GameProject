using CC6GameProject.Spells;
using System;

namespace CC6GameProject
{
    class InventoryItem
    {
        public string name, description;
        public IIAI[] extraInfo;
        public char[,] image;
        public ConsoleColor color;

        public InventoryAction[] actions;

        public InventoryItem() { }
        public InventoryItem(string name, string description, char[,] image, ConsoleColor color,
            IIAI[] addInfo = null, InventoryAction[] invActions = null)
        {
            this.name = name;
            this.description = description;
            this.image = image;
            this.color = color;
            this.extraInfo = addInfo;

            this.actions = invActions == null ? new InventoryAction[] { new InventoryActionDrop() } : invActions;
        }

        // adds more info to this item
        public void AddAdditionalInfo(IIAI info)
        {
            Array.Resize(ref extraInfo, extraInfo.Length + 1);
            extraInfo[extraInfo.Length - 1] = info;
        }
    }

    abstract class InventoryAction
    {
        public abstract string Action { get; }
        public abstract string Description { get; }

        protected int amnt;

        public InventoryAction(int amount = 0)
        {
            this.amnt = amount;
        }

        /// <returns>If the item needs to be destroyed</returns>
        public abstract bool Act(ref Player p, int itemIndex);
    }

    class InventoryActionDrop : InventoryAction
    {
        public override string Action { get { return "Drop"; } }
        public override string Description { get { return "Drop this item"; } }

        public InventoryActionDrop() : base() { }

        public override bool Act(ref Player p, int itemIndex)
        {
            return true;
        }
    }

    class InventoryActionCast : InventoryAction
    {
        public override string Action { get { return "Cast"; } }
        public override string Description { get { return "Cast this spell"; } }

        public InventoryActionCast() : base() { }

        public override bool Act(ref Player p, int itemIndex)
        {
            SpellEffect[] effects = ((EffectItem)p.inventory[itemIndex]).effects;

            Creature creature = (Creature)p;

            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].Apply(ref creature);
            }

            return true;
        }
    }

    class InventoryActionYield : InventoryAction
    {
        public override string Action { get { return "Yield"; } }
        public override string Description { get { return "Hold this weapon in your hands"; } }

        public InventoryActionYield() : base() { }

        public override bool Act(ref Player p, int itemIndex)
        {
            if (((WeaponItem)p.inventory[itemIndex]).weapon is Throwable)
            {
                if (p.rWeaponItem != null) p.addInventoryItem(p.rWeaponItem);
                p.rWeaponItem = (WeaponItem)p.inventory[itemIndex];
            }
            else
            {
                if (p.mWeaponItem != null) p.addInventoryItem(p.mWeaponItem);
                p.mWeaponItem = (WeaponItem)p.inventory[itemIndex];
            }

            Game.msg("now holding " + p.inventory[itemIndex].name + '!');
            return true;
        }
    }
    //rip not implemented 
    /*class InventoryActionHeal : InventoryAction
    {
        public override string Action { get { return "Heal"; } }
        public override string Description { get { return "Heal yourself " + amnt + " with this item"; } }

        public InventoryActionHeal(int amount) : base(amount) { }

        public override bool Act(ref Player p, int itemIndex)
        {
            int health = p.health + amnt;
            if (health > p.maxHealth) health = p.maxHealth;

            health -= p.health;

            p.health += health;

            Game.msg("You healed yourself " + health);
            return true;
        }
    }*/
}

using System;

namespace CC6GameProject
{
    class WeaponItem : InventoryItem
    {
        public Weapon weapon;

        public WeaponItem(Weapon superWeapon, string desc, char[,] img, ConsoleColor clr = ConsoleColor.Gray)
        {
            this.weapon = superWeapon;

            this.name = superWeapon.name;
            this.description = desc;
            this.image = img;
            this.color = clr;

            this.extraInfo = new IIAI[] { new IIAI("Deals", superWeapon.damage.X + "-" + superWeapon.damage.Y) };
            if (superWeapon is Throwable)
            {
                AddAdditionalInfo(new IIAI("Range", ((Throwable)superWeapon).range.ToString()));
            }

            this.actions = new InventoryAction[] { new InventoryActionYield(), new InventoryActionDrop() };
        }
    }

    class Weapon
    {
        public Point damage;
        public string name;

        public override string ToString()
        {
            return name;
        }
    }
    class Throwable : Weapon
    {
        public ushort hitChance;
        public byte range;
    }

    class Dagger : Weapon
    {
        public Dagger()
        {
            damage = new Point(1, 3);
            name = "Dagger";
        }
    }
    class Spear : Throwable
    {
        public Spear()
        {
            hitChance = 780;
            range = 3;
            damage = new Point(3, 7);
            name = "Spear";
        }
    }
    class Sword : Weapon
    {
        public Sword()
        {
            damage = new Point(3, 6);
            name = "Sword";
        }
    }
}

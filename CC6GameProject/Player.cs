using System;

namespace CC6GameProject
{
    class Player : Creature
    {
        public int nInvItems = 0;

        public InventoryItem[] inventory = new InventoryItem[Constants.invCapacity];

        public WeaponItem rWeaponItem;//= Constants.spear;
        public WeaponItem mWeaponItem;// = Constants.dagger;

        // "Holding" weapons
        public Throwable rangedWeapon
        {
            get { return rWeaponItem == null ? null : (Throwable)rWeaponItem.weapon; }
        }
        public Weapon meleeWeapon
        {
            get { return mWeaponItem == null ? null : mWeaponItem.weapon; }
        }

        public string name = Environment.UserName;
        public uint xp = 0;
        public uint reqXp = 1;
        public uint level = 1;

        private byte hMoves = 0;

        public readonly bool[,] circle = Constants.generateCircle(Constants.playerLookRadius);

        public Player()
            : base()
        {
            health = maxHealth = 15;

            tiletype = TileType.Player;
            lastTile = new Tile(TileType.Up);
            drawChar = '☺';
            color = ConsoleColor.Magenta;
            damage = new Point(3, 0); // x is the strength

            hitLikelyness = 400;

            addInventoryItem(Constants.dagger);
        }

        public bool addInventoryItem(InventoryItem item)
        {
            if (nInvItems >= inventory.Length) return false;
            inventory[nInvItems++] = item;
            return true;
        }

        public void removeInventoryItem(int n)
        {

            nInvItems--;
            for (; n < nInvItems;)
            {
                inventory[n] = inventory[++n];
            }
            inventory[nInvItems] = null;
        }

        // unused
        public InventoryItem swapInventoryItem(int n, InventoryItem item)
        {
            InventoryItem ret = inventory[n];
            inventory[n] = item;
            return ret;
        }

        public InventoryItem lastInventoryItem()
        {
            return inventory[nInvItems - 1];
        }

        public void onMove()
        {
            if (health >= maxHealth) return;
            hMoves++;
            if (hMoves >= 4)
            {
                health++;
                hMoves = 0;
            }
        }

        public void levelUp()
        {
            xp -= reqXp;
            level++;

            ////////
            reqXp = reqXp * 2;
            hitLikelyness += (ushort)(75 - hitLikelyness / 100);

            maxHealth += 4;
            damage.X += 1;
            damage.Y += 2;
        }
    }
}

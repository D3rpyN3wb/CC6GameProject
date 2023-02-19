using System;

namespace CC6GameProject
{

    enum CreatureAIMode
    {
        Friendly,
        Neutral,
        Angry
    }

    class Creature : Tile
    {
        public bool processed = false;

        public int maxHealth = 0;
        public int health = 0;
        public int money = 0;
        public ushort searchRange = 0;
        public Point damage = new Point(1, 4);


        public Tile lastTile = new Tile(TileType.Air);

        // Likeliness of hitting something
        public ushort hitLikelyness = 0;

        // Likeliness of getting hit in a form of a penalty
        short? hitPenalty = null; // 1000 = max, 0 = min
        public bool HasHitPenalty
        {
            get { return hitPenalty != null; }
        }
        public short HitPenalty
        {
            get { return HasHitPenalty ? (short)hitPenalty : (short)0; }
            set { this.hitPenalty = value; }
        }


        public Creature()
        {
            walkable = true;
        }

        // Include if (t is Pickupable)
        public void onTileEncounter(ref Tile t)
        {
            if (t.tiletype == TileType.Money)
            {
                money += ((Money)t).money;
                t = new Tile(((Pickupable)t).replaceTile);
            }
        }

        /// Creature DAMAGE
        public bool doDamage(int dmg, ref Tile t)
        {
            health -= dmg;

            if (health <= 0)
            {
                t = new Money(this.money);
                ((Pickupable)t).replaceTile = lastTile.tiletype;
                t.needsToBeDrawn = true;
                return true;
            }
            return false;
        }

        /// Heal Creature
        public int heal(int amnt)
        {
            int h = health + amnt;
            if (h > maxHealth) h = maxHealth;

            h -= health;

            health += h;
            return h;
        }

        public int hit(ref Random ran, ref Player p)
        {
            if (ran.Next(0, 1001) > hitLikelyness - p.HitPenalty)
            {
                return 0;
            }

            int dmg = ran.Next(damage.X, damage.Y + 1);

            Tile t = (Tile)p;
            if (p.doDamage(dmg, ref t)) p = null;

            return dmg;
        }

        public virtual ushort getXp(ref Random ran)
        {
            return 0;
        }

        public void move(Tile old)
        {
            this.lastTile = old;
        }
    }



    class Snake : Creature
    {
        public Snake(ref Random ran, int lvl = 0)
            : base()
        {
            health = maxHealth = ran.Next(6, 10) + lvl;
            money = ran.Next(1, 5);
            tiletype = TileType.Snake;
            drawChar = 'S';
            color = ConsoleColor.DarkGreen;
            damage = new Point(2 + lvl, 4 + lvl);
            searchRange = 5;

            if (lvl <= 10)
            {
                float f = (float)lvl / 2.0f;
                hitLikelyness = (ushort)((95 * f - f * f * 10) * 13 + 301);
            }
            else hitLikelyness = 900;

            hitLikelyness += (ushort)ran.Next(-20, 21);
        }

        public override ushort getXp(ref Random ran)
        {
            return (ushort)ran.Next(0, damage.X / 2 + maxHealth / 5);
        }
    }
    class Rat : Creature
    {
        public Rat(ref Random ran, int lvl = 0)
            : base()
        {
            health = maxHealth = ran.Next(1, 3) + lvl;
            money = ran.Next(1, 2);
            tiletype = TileType.Rat;
            drawChar = 'r';
            color = ConsoleColor.Yellow;
            damage = new Point(1 + lvl, 1 + lvl);
            searchRange = 2;

            if (lvl <= 10)
            {
                float f = (float)lvl / 2.0f;
                hitLikelyness = (ushort)((95 * f - f * f * 10) * 13 + 301);
            }
            else hitLikelyness = 400;

            hitLikelyness += (ushort)ran.Next(-20, 21);
        }

        public override ushort getXp(ref Random ran)
        {
            return (ushort)ran.Next(0, damage.X / 2 + maxHealth / 5);
        }
    }

    class Goblin : Creature
    {
        public Goblin(ref Random ran, int lvl = 0)
            : base()
        {
            money = ran.Next(4, 10);
            health = maxHealth = ran.Next(5, 10) + lvl;
            searchRange = (ushort)(6 + lvl / 3);
            tiletype = TileType.Goblin;
            drawChar = 'G';
            color = ConsoleColor.DarkRed;
            damage = new Point(1 + lvl, 3 + lvl + ran.Next(0, lvl));

            if (lvl <= 8)
            {
                float f = (float)lvl / 2.0f;
                hitLikelyness = (ushort)((120 * f - f * f * 12) * 13 + 300);
            }
            else hitLikelyness = 800;
        }

        public override ushort getXp(ref Random ran)
        {
            return (ushort)(ran.Next(1, 3 + maxHealth / 10) + ran.Next(0, damage.Y - damage.X));
        }
    }
    class Orc : Creature
    {
        public Orc(ref Random ran, int lvl = 0)
            : base()
        {
            money = ran.Next(10, 20);
            health = maxHealth = ran.Next(15, 25) + lvl;
            searchRange = (ushort)(6 + lvl / 3);
            tiletype = TileType.Orc;
            drawChar = 'O';
            color = ConsoleColor.DarkRed;
            damage = new Point(2 + lvl, 6 + lvl + ran.Next(0, lvl));

            if (lvl <= 8)
            {
                float f = (float)lvl / 2.0f;
                hitLikelyness = (ushort)((120 * f - f * f * 12) * 13 + 500);
            }
            else hitLikelyness = 1000;
        }
            public override ushort getXp(ref Random ran)
        {
            return (ushort)(ran.Next(1, 3 + maxHealth / 10) + ran.Next(0, damage.Y - damage.X));
        }
    }
    class Dragon : Creature
    {
        public Dragon(ref Random ran, int lvl = 0)
            : base()
        {
            money = ran.Next(200, 400);
            health = maxHealth = ran.Next(50, 100) + lvl;
            searchRange = (ushort)(6 + lvl / 3);
            tiletype = TileType.Dragon;
            drawChar = 'D';
            color = ConsoleColor.DarkBlue;
            damage = new Point(20 + lvl, 30 + lvl + ran.Next(0, lvl));

            if (lvl <= 8)
            {
                float f = (float)lvl / 2.0f;
                hitLikelyness = (ushort)((120 * f - f * f * 12) * 13 + 300);
            }
            else hitLikelyness = 300;
        }
        public override ushort getXp(ref Random ran)
        {
            return (ushort)(ran.Next(1, 3 + maxHealth / 10) + ran.Next(0, damage.Y - damage.X));
        }
    }

}

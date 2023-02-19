using System;
using System.Text;

namespace CC6GameProject
{
    static class Constants
    {
        public const byte playerLookRadius = 5;//15;

        public const byte invCapacity = 50;
        public const byte invDescriptionWidth = 0x20;

        static Constants()
        {
            yWall = _g(186);
            xWall = _g(205);
            lupWall = _g(201);
            ldownWall = _g(200);
            rupWall = _g(187);
            rdownWall = _g(188);
            lSplitWall = _g(204);
            rSplitWall = _g(185);
            yWallToXWallBothSides = _g(202);
            yWallWithRightXWall = _g(204);
            yWallWithLeftXWall = _g(185);

            chars = new char[] { _g(166), // scrollChar
                _g(30),                   // startChar
                _g(31),                   // exitChar
                _g(147),                  // goblinChar
                _g(16),                   // >ish char
                _g(220),                  // chestChar
            };
        }

        public const ushort SFLAG_NEEDS_CHECK = ushort.MaxValue - 1;

        public const ConsoleColor invItemBorderColor = ConsoleColor.DarkGreen;
        public const ConsoleColor invSelItemBorderColor = ConsoleColor.Yellow;

        public static readonly Encoding enc = Encoding.GetEncoding(437);

        public readonly static char[] chars; // used to display special characters, like a scroll.
        public readonly static char yWall, xWall, lupWall, ldownWall, rupWall, rdownWall, lSplitWall, rSplitWall, yWallToXWallBothSides, yWallWithRightXWall, yWallWithLeftXWall;

        public static readonly string[] dungeonNameParts = new string[] { "Prisons", "Dungeon", "Tunnels", "Chamber", "Tombs", "Cells", "Lair", "Cave", "Catacombs", "Caverns" };
        public readonly static string[] namefwords = new string[] { "Witty", "Ancient", "Evil", "Heartless", "Furious", "Screaming", "Mighty", "Infamous", "Loved", "Isolated", "Withered", "Magic", "Undead", "Mystic", "Dark", "Black", "Creepy" };
        public readonly static string[] nameswords = new string[] { "Orc", "Elf", "Prisoner", "Dragon", "Angel", "Demon", "Giant", "Ghost", "Priest", "Cobra", "Warrior", "Phantom", "Hunter", "Phoenix", "Widow", "Mage", "Warlock" };
        public readonly static string[] namewords = new string[] { "Blindness", "Resurrection", "Power", "Acceptance", "Tears", "Undead", "Lies", "Fear", "Disbelieve", "Anger", "Mistakes", "Judgement" };

        // "Macro"
        static char _g(byte val)
        {
            return enc.GetChars(new byte[] { val })[0];
        }

        // random language characters
        public readonly static char[] rlangChars = new char[] { 'a', 'o', 'e', 'u', 'j', 'k', 'n', 'q', 'p', 'w',
            _g(230), _g(208), _g(244), _g(255), _g(232), _g(209), _g(190), _g(207)  };

        public readonly static InventoryItem[] invItems = {
            new InventoryItem("Empty", "Empty", new char[,] {{'\\',' ',' ',' ',' ',' ','/'},{' ','\\',' ',' ',' ','/',' '},{' ',' ','\\',' ','/',' ',' '},{' ',' ',' ','X',' ',' ',' '},
            { ' ',' ','/',' ','\\',' ',' ' },{ ' ','/',' ',' ',' ','\\',' ' },{ '/',' ',' ',' ',' ',' ','\\'}}, ConsoleColor.DarkRed),};


        public static readonly WeaponItem spear = new WeaponItem(new Spear(),
            "The spear is a weapon with short range you can throw to creatures. It's very effective against big creatures!",
            new char[,] { {' ',' ',' ',' ',' ','/','|' },{ ' ',' ',' ',' ','/',' ','|' },
            { ' ',' ',' ',' ','/','/',' ' },{ ' ',' ',' ','/','/',' ',' ' },{ ' ',' ','/','/',' ',' ',' ' },{ ' ','/','/',' ',' ',' ',' '} });
        public static readonly WeaponItem dagger = new WeaponItem(new Dagger(),
            "A simple knife!", new char[,] { { ' ', '_', '_', '_', '_', '_', '.', '_', '_', ' ' }, { ' ', '`', '-', '-', '-', '-', ';', '=', '=', '\'' } });
        public static readonly WeaponItem sword = new WeaponItem(new Sword(),
            "A mighty cheap Sword!", new char[,] { { ' ', '_', '_', '_', '_', '_', '.', '_', '_', ' ' }, { ' ', '`', '-', '-', '-', '-', ';', '=', '=', '\'' } });

        public const string invFullMsg = "Inventory full!";

        #region methods
        public static bool[,] generateCircle(int radius)
        {
            // http://webstaff.itn.liu.se/~stegu/circle/circlealgorithm.pdf
            int x = 0;
            int y = radius;
            int d = 5 - 4 * radius;
            int da = 12;
            int db = 20 - 8 * radius;

            bool[,] circle = new bool[radius * 2 - 1, radius * 2 - 1];

            // make an circle outline
            while (x < y)
            {
                if (d < 0)
                {
                    d += da;
                    db += 8;
                }
                else
                {
                    y--;
                    d += db;
                    db += 16;
                }
                da += 8;

                circle[++x, y] = true;
            }

            // fill the circle
            radius--;
            for (int a = -radius; a < radius; a++)
                for (int b = -radius; b < radius; b++)
                    if (a * a + b * b < radius * radius)
                        circle[a + radius, b + radius] = true;

            return circle;
        }

        public static string getPDamageInWords(int dmg, ref Random ran)
        {
            if (dmg == 0)
            {
                switch (ran.Next(0, 4))
                {
                    default: return "You embarassingly missed the";
                    case 1: return "You barely missed the";
                    case 2: return "You completely missed the ";
                }
            }
            else if (dmg < 5) return ran.Next(0, 2) == 1 ? "You did a pathetic hit on the " : "You did a minor hit on the ";
            else if (dmg < 10) return "You did a hit on the ";
            else return ran.Next(0, 2) == 1 ? "You did a Great hit on the " : "You injured";
        }

        public static string getCDamageInWords(int dmg, ref Random ran)
        {
            if (dmg < 0) return "has healed you with " + dmg;
            else if (dmg == 0)
            {
                switch (ran.Next(0, 4))
                {
                    default: return "missed you";
                    case 1: return "barely missed you";
                    case 2: return "completely missed you";
                }
            }
            else if (dmg < 5) return ran.Next(0, 2) == 1 ? "did a simple hit on you" : "did a minor hit on you";
            else if (dmg < 10) return "did a great hit on you";
            else return ran.Next(0, 2) == 1 ? "did a critical hit on you" : "injured you";
        }

        // makes a string break into pieces
        public static string[] generateReadableString(string input, int lineLength)
        {
            string[] sInput = input.Split(' ');
            string[] lines = new string[sInput.Length/*input.Length / lineLength + 4*/];

            string currentLine = "";
            int a = 0;

            for (int i = 0; i < sInput.Length; i++)
            {
                if (currentLine.Length + sInput[i].Length < lineLength)
                {
                    if (currentLine.Length > 0)
                        if (currentLine.Length + sInput[i].Length + 1 <= lineLength) currentLine += ' ';
                        else
                        {
                            // GOD DAMNIT! NEED TO DO IT TWICE
                            //lines[a] = fillUpEmptySpace(currentLine, lineLength);
                            lines[a] = currentLine;
                            a++;
                            currentLine = "";
                            i--;
                        }
                    currentLine += sInput[i];
                }
                else if (currentLine.Length <= 0)
                {
                    lines[a] = sInput[i].Remove(lineLength, sInput[i].Length - lineLength);
                    sInput[i] = sInput[i].Remove(0, lineLength);
                    a++;
                    i--;
                }
                else
                {
                    //lines[a] = fillUpEmptySpace(currentLine, lineLength);
                    lines[a] = currentLine;
                    a++;
                    currentLine = "";
                    i--;
                }
            }

            if (currentLine != "")
            {
                //lines[a] = fillUpEmptySpace(currentLine, lineLength);
                lines[a] = currentLine;
                a++;
            }


            Array.Resize(ref lines, a);
            return lines;
        }

        // makes a string fit into 
        public static string fillUpEmptySpace(string s, int l, char c = ' ')
        {
            l -= s.Length;
            for (int i = 0; i < l; i++) s += c;
            return s;
        }

        // Lazyness level: Over 9000!
        public static bool invert(this bool b)
        {
            /*if (b) return false;
            return true;*/
            /*return (b) ? false : true;*/
            /*return b ^= true;*/
            return !b;
        }

        // converts 340 to 34.0%
        public static string ToPercent(short val)
        {
            string s = val.ToString();
            return s.Insert(s.Length - 1, ".") + '%';
        }
        #endregion
    }
}

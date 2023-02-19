using System;

namespace CC6GameProject
{
    partial class Game
    {
        void draw()
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y].needsToBeDrawn)
                    {
                        Console.SetCursorPosition(x, y);
                        tiles[x, y].Draw();
                        tiles[x, y].needsToBeDrawn = false;
                    }
                }
            }

            if (drawOnceAfterInit)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.CursorLeft = 0;
                Console.CursorTop++;

                drawOnce(Console.WindowWidth * 2);
                drawOnceAfterInit = false;
            }

            drawInfoBar();
        }


        void reDrawDungeon()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            drawOnceAfterInit = true;
            for (int x = 0; x < tiles.GetLength(0); x++) for (int y = 0; y < tiles.GetLength(1); y++) tiles[x, y].needsToBeDrawn = true;
            draw();
        }

        void drawMessages(string loc)
        {
            if (currentMessage == "")
            {
                currentMessage = getDefaultMessage();
            }

            string[] msgs = currentMessage.Split('\n');
            currentMessage = "";

            for (int i = 1; i < msgs.Length - 1; i++)
            {
                infoLine2.Draw(new string[] { msgs[i] + " -- Press the Spacebar --", loc });

                Console.BackgroundColor = ConsoleColor.Black;
                while (true)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Spacebar || key == ConsoleKey.Escape) break;
                }
                Console.CursorLeft = 0;
                Console.CursorTop--;
                Console.Write(' ');
                Console.CursorLeft = 0;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }

            infoLine2.Draw(new string[] { msgs[msgs.Length - 1], loc });
        }

        void drawInfoBar()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(0, tiles.GetLength(1));

            Player p = (Player)tiles[playerPos.X, playerPos.Y];

            infoLine1.Draw(new string[] {
                "Health: " + p.health + '(' + p.maxHealth + ')',
                "xp: " + p.xp + '/' + p.reqXp + "(lvl " + p.level + ')',
                p.meleeWeapon == null ? "str: " + p.damage.X : "str: " + p.damage.X + '+' + p.meleeWeapon.damage.X + '-' + p.meleeWeapon.damage.Y,
                "Money: " +  p.money,
                "Money: " + p.money
            });

            string location = "";

            if (p.lastTile.tiletype != TileType.Air) location = p.lastTile.tiletype.ToString();
            else
            {
                Room r = getDungeonAt(playerPos);
                location = r == null ? "x-x What the hell did you do this game?" : r.name;
            }

            drawMessages(location);

            Console.BackgroundColor = ConsoleColor.Black;
        }

        void drawOnce(int amount)
        {
            Point orgin = new Point(Console.CursorLeft, Console.CursorTop);
            for (int i = 0; i < amount; i++) Console.Write(' ');
            Console.SetCursorPosition(orgin.X, orgin.Y);
        }

        /// <summary>
        /// Writes a message in the center of the screen
        /// </summary>
        /// <param name="s">String you want to display</param>
        public static void WriteCenter(string s)
        {
            Console.CursorLeft = Console.BufferWidth / 2 - s.Length / 2;
            Console.CursorTop = Console.BufferHeight / 2 - 2;
            Console.Write(s);
        }
    }
}

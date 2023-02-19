using System;

namespace CC6GameProject
{

    enum TileType
    {
        None,
        Air,
        Wall,
        Money,
        Player,
        Corridor,
        Torch,
        Scroll,
        Snake,
        Rat,
        Goblin,
        Orc,
        Dragon,
        Wizzard,
        Chest,
        Up,
        Down
    }

    class Tile
    {
        public char drawChar;
        public bool walkable = false;
        public bool lighten = false;
        public bool discovered = false;
        public bool needsToBeDrawn = true;
        public ConsoleColor color;
        public TileType tiletype;

        public char notLightenChar = '.';

        public Tile() { }
        public Tile(TileType tp)
        {
            setTile(tp);
        }

        public void setTile(TileType tp)
        {
            needsToBeDrawn = true;
            tiletype = tp;

            switch (tp)
            {
                default:
                    drawChar = ' ';
                    break;
                case TileType.None:
                    drawChar = notLightenChar = ' ';
                    break;
                case TileType.Air:
                    walkable = true;
                    drawChar = '.';
                    notLightenChar = '.';
                    color = ConsoleColor.Gray; break;
                case TileType.Wall:
                    //drawChar = '#';
                    color = ConsoleColor.Gray; break;
                case TileType.Corridor:
                    drawChar = notLightenChar = '#';
                    color = ConsoleColor.White;
                    walkable = true; break;
                case TileType.Up:
                    drawChar = Constants.chars[1];
                    color = ConsoleColor.DarkCyan;
                    walkable = true; break;
                case TileType.Down:
                    drawChar = Constants.chars[2];
                    color = ConsoleColor.Green;
                    walkable = true; break;
            }
        }

        public virtual void Draw()
        {
            if (discovered)
                if (lighten)
                {
                    Console.ForegroundColor = color;
                    Console.Write(drawChar);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(notLightenChar);
                }
        }

        public override string ToString()
        {
            return drawChar.ToString();
        }
    }



    class Wall : Tile
    {
        public Wall(char c)
            : base(TileType.Wall)
        {
            this.drawChar = c;
        }

        public override void Draw()
        {
            if (discovered)
                if (lighten)
                {
                    Console.ForegroundColor = color;
                    Console.Write(drawChar);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(drawChar);
                }
        }
    }
}

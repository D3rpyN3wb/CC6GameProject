using System;

namespace CC6GameProject
{
    // Refractor count: 22
    partial class Game
    {
        int invSelItem;
        int invActionSel;

        bool invDidDescDraw = false;

        Point invPrevPoint = new Point();
        int invLowestY = 0;

        InventoryDisplayItem[] invDItems;
        InventoryDisplayItem invDescription = new InventoryDisplayItem(new Point(), new Point());

        // I feel sooo redundant. It's not that easy you know. I could just have an array of "chars", but drawing one char is wayyy much slower than 10.
        void drawInventory(int fromIndex = 0 /*from what item it should start drawing from*/)
        {
            Player p = (Player)tiles[playerPos.X, playerPos.Y];

            if (p.nInvItems > 0)
            {
                invLowestY = 0;

                if (fromIndex == 0)
                {
                    for (int i = 0; i < invSelItem; i++)
                        inv_drawProcess(p.inventory[i], i, Constants.invItemBorderColor);
                    inv_drawProcess(p.inventory[invSelItem], invSelItem, Constants.invSelItemBorderColor);
                    for (int i = invSelItem + 1; i < p.nInvItems; i++)
                        inv_drawProcess(p.inventory[i], i, Constants.invItemBorderColor);
                }
                else
                {
                    if (fromIndex <= invSelItem)
                    {
                        for (int i = 0; i < invSelItem; i++)
                            inv_drawProcess(p.inventory[i], i, Constants.invItemBorderColor);
                        inv_drawProcess(p.inventory[invSelItem], invSelItem, Constants.invSelItemBorderColor);
                        for (int i = invSelItem + 1; i < p.nInvItems; i++)
                            inv_drawProcess(p.inventory[i], i, Constants.invItemBorderColor);
                    }
                    else
                    {
                        for (int i = fromIndex; i < p.nInvItems; i++)
                            inv_drawProcess(p.inventory[i], i, Constants.invItemBorderColor);
                    }
                }

                // Draw two times. Very redundant, but used for the background.
                inv_drawDescription();
                inv_drawDescription();

                invPrevPoint = new Point();
            }
            else
            {
                // If the description was shown and it's the last item, it should be removed!
                if (invDidDescDraw)
                {
                    makeBlackSpace(invDescription);
                }
                WriteCenter("Your inventory is empty");
            }
        }

        void makeBlackSpace(InventoryDisplayItem item)
        {
            Console.CursorTop = item.begin.Y;

            // writing an array of characters 
            char[] blank = new char[item.end.X - item.begin.X];
            for (int i = 0; i < blank.Length; i++)
                blank[i] = ' ';

            for (int y = 0; y < item.end.Y - item.begin.Y; y++)
            {
                Console.CursorLeft = item.begin.X;
                Console.Write(blank);
                Console.CursorTop++;
            }
        }

        void inv_drawProcess(InventoryItem item, int i, ConsoleColor clr)
        {
            int width = item.image.GetLength(1) + 2;

            if (invPrevPoint.X + width >= Console.BufferWidth)
            {
                invPrevPoint.X = 0;
                invPrevPoint.Y = invLowestY + 1;
            }

            Point begin = new Point(invPrevPoint.X, invPrevPoint.Y);
            drawInvItem(item, clr, invPrevPoint);

            invDItems[i] = new InventoryDisplayItem(begin, new Point(Console.CursorLeft, Console.CursorTop + 1));

            invPrevPoint.X += width + 1;
            if (Console.CursorTop > invLowestY) invLowestY = Console.CursorTop;
        }

        void drawInvItem(InventoryItem item, ConsoleColor borderColor, Point origin)
        {
            int innerWidth = item.image.GetLength(1);

            // generate lower and upper bar
            string up = Constants.lupWall.ToString();
            string down = Constants.ldownWall.ToString();

            for (int i = 0; i < innerWidth; i++)
            {
                up += Constants.xWall;
                down += Constants.xWall;
            }

            up += Constants.rupWall.ToString();
            down += Constants.rdownWall.ToString();

            // write upper bar
            Console.ForegroundColor = borderColor;
            Console.CursorLeft = origin.X;
            Console.CursorTop = origin.Y;
            Console.Write(up);
            Console.CursorLeft = origin.X;
            Console.CursorTop++;

            string[] title = Constants.generateReadableString(item.name, innerWidth);

            // draw title
            for (int y = 0; y < title.Length; y++)
            {
                Console.ForegroundColor = borderColor;
                Console.Write(Constants.yWall);

                Console.ForegroundColor = ConsoleColor.White;

                string t = title[y];
                Console.CursorLeft = origin.X + innerWidth / 2 - t.Length / 2 + 1;
                Console.Write(t);

                Console.CursorLeft = origin.X + innerWidth + 1;
                Console.ForegroundColor = borderColor;
                Console.Write(Constants.yWall);
                Console.CursorTop++;
                Console.CursorLeft = origin.X;
            }

            // draw image
            for (int y = 0; y < item.image.GetLength(0); y++)
            {
                Console.ForegroundColor = borderColor;
                Console.Write(Constants.yWall);

                string line = "";

                for (int x = 0; x < innerWidth; x++)
                    line += item.image[y, x];

                Console.ForegroundColor = item.color;
                Console.Write(line);

                Console.ForegroundColor = borderColor;
                Console.Write(Constants.yWall);
                Console.CursorLeft = origin.X;
                Console.CursorTop++;
            }

            // draw lower bar
            Console.Write(down);
        }

        void inv_drawDescription()
        {
            // if this needs to change, make sure the background changes too
            if (invDidDescDraw)
            {
                makeBlackSpace(invDescription);
            }

            InventoryItem item = ((Player)tiles[playerPos.X, playerPos.Y]).inventory[invSelItem];
            int itemInnerWidth = item.image.GetLength(1);

            bool lefty = invDItems[invSelItem].begin.X + Constants.invDescriptionWidth >= tiles.GetLength(0);

            int originX = lefty ? invDItems[invSelItem].begin.X - Constants.invDescriptionWidth + itemInnerWidth + 2 : invDItems[invSelItem].begin.X;

            Point begin = new Point(originX, invDItems[invSelItem].end.Y - 1);

            // used to display the horizontal stuff(upper bar and lower bar)
            char[] bar = new char[Constants.invDescriptionWidth];

            // make a "template" for the bars
            for (int i = 0; i < Constants.invDescriptionWidth; i++)
            {
                bar[i] = Constants.xWall;
            }

            // rip in pece uncompressed downloaders
            int thatStupidCharacterThatBreaksEverything;

            // construct upper bar
            if (lefty)
            {
                thatStupidCharacterThatBreaksEverything = Constants.invDescriptionWidth - itemInnerWidth - 2;
                bar[thatStupidCharacterThatBreaksEverything] = Constants.yWallToXWallBothSides;
                bar[0] = Constants.lupWall;
                bar[Constants.invDescriptionWidth - 1] = Constants.yWallWithLeftXWall;
            }
            else
            {
                thatStupidCharacterThatBreaksEverything = itemInnerWidth + 1;
                bar[thatStupidCharacterThatBreaksEverything] = Constants.yWallToXWallBothSides;
                bar[0] = Constants.yWallWithRightXWall;
                bar[Constants.invDescriptionWidth - 1] = Constants.rupWall;
            }

            // draw the upper bar
            Console.CursorLeft = originX;
            Console.CursorTop = begin.Y;
            Console.ForegroundColor = Constants.invSelItemBorderColor;

            Console.Write(bar);

            Console.CursorTop++;
            Console.CursorLeft = originX;

            // draw the description
            string[] description = Constants.generateReadableString(item.description, Constants.invDescriptionWidth - 4);
            for (int y = 0; y < description.Length; y++)
            {
                Console.Write(Constants.yWall);
                Console.CursorLeft++;
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write(description[y]);

                Console.ForegroundColor = Constants.invSelItemBorderColor;
                Console.CursorLeft = originX + Constants.invDescriptionWidth - 1;
                Console.Write(Constants.yWall);

                Console.CursorTop++;
                Console.CursorLeft = originX;
            }

            // extra line
            Console.Write(Constants.yWall);
            Console.CursorLeft = originX + Constants.invDescriptionWidth - 1;
            Console.Write(Constants.yWall);
            Console.CursorTop++;
            Console.CursorLeft = originX;

            // draw additional info
            if (item.extraInfo != null)
            {
                int valLoc = 0;

                // calculate the max distance so everything looks neat
                for (int i = 0; i < item.extraInfo.Length; i++)
                    if (item.extraInfo[i].label.Length > valLoc) valLoc = item.extraInfo[i].label.Length;

                valLoc += originX + 4;

                for (int i = 0; i < item.extraInfo.Length; i++)
                {
                    Console.Write(Constants.yWall);
                    Console.CursorLeft++;
                    Console.ForegroundColor = item.extraInfo[i].lColor;

                    Console.Write(item.extraInfo[i].label + ':');
                    Console.CursorLeft = valLoc;
                    Console.ForegroundColor = item.extraInfo[i].vColor;
                    Console.Write(item.extraInfo[i].value);

                    Console.ForegroundColor = Constants.invSelItemBorderColor;
                    Console.CursorLeft = originX + Constants.invDescriptionWidth - 1;
                    Console.Write(Constants.yWall);

                    Console.CursorTop++;
                    Console.CursorLeft = originX;
                }

                // ... and an extra line
                Console.Write(Constants.yWall);
                Console.CursorLeft = originX + Constants.invDescriptionWidth - 1;
                Console.Write(Constants.yWall);
                Console.CursorTop++;
                Console.CursorLeft = originX;
            }

            for (int i = 0; i < item.actions.Length; i++)
            {
                Console.Write(Constants.yWall);

                // Draw the description of the action when needed
                if (i == invActionSel)
                {
                    // draw cursor on selected item
                    Console.CursorLeft++;
                    Console.ForegroundColor = ConsoleColor.DarkRed; // TODO: Not sure. Blue? Background color?
                    Console.Write(Constants.chars[4]);
                    Console.CursorLeft += 2;

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(item.actions[i].Action);

                    Console.CursorLeft += 2;

                    int x = Console.CursorLeft;
                    string[] desc = Constants.generateReadableString(
                        item.actions[i].Description,
                        Constants.invDescriptionWidth - (x - originX) - 2
                    );

                    if (desc.Length > 1)
                    {
                        Console.ForegroundColor = Constants.invSelItemBorderColor;

                        for (int y = 0; y < desc.Length - 1; y++)
                        {
                            Console.CursorLeft = x;
                            Console.ForegroundColor = ConsoleColor.Gray; // FOR COLOR CHANGE CHECK

                            Console.Write(desc[y]);

                            Console.ForegroundColor = Constants.invSelItemBorderColor;
                            Console.CursorLeft = originX + Constants.invDescriptionWidth - 1;
                            Console.Write(Constants.yWall);

                            Console.CursorLeft = originX;
                            Console.CursorTop++;

                            Console.Write(Constants.yWall);
                        }
                    }

                    Console.CursorLeft = x;
                    Console.ForegroundColor = ConsoleColor.Gray; // THIS OUT TOO

                    // one line doesn't count
                    Console.Write(desc[desc.Length - 1]);
                }
                else
                {
                    Console.CursorLeft += 4;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(item.actions[i].Action);
                }

                Console.ForegroundColor = Constants.invSelItemBorderColor;
                Console.CursorLeft = originX + Constants.invDescriptionWidth - 1;
                Console.Write(Constants.yWall);
                Console.CursorLeft = originX;
                Console.CursorTop++;
            }

            // construct lower bar
            bar[0] = Constants.ldownWall;
            bar[Constants.invDescriptionWidth - 1] = Constants.rdownWall;
            bar[thatStupidCharacterThatBreaksEverything] = Constants.xWall;

            // draw lower bar
            Console.Write(bar);

            int endY = invDescription.end.Y;

            invDescription = new InventoryDisplayItem(begin, new Point(Console.CursorLeft, Console.CursorTop + 1));

            // if description border had decreased, draw the collided inventory items again
            if (invDidDescDraw && (endY > invDescription.end.Y || invDescription.end.Y - endY > 1))
            {
                Player p = ((Player)tiles[playerPos.X, playerPos.Y]);
                bool needed = false;
                for (int i = 0; i < p.nInvItems; i++)
                {
                    if (invDItems[i].end.Y > invDescription.end.Y && inv_collides(invDescription, invDItems[i]))
                    {
                        needed = true;
                        drawInvItem(p.inventory[i], Constants.invItemBorderColor/*can never be selected item, no need to check*/, invDItems[i].begin);
                    }
                }
                if (needed)
                {
                    // ... and itself again
                    inv_drawDescription();
                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorLeft = 0;
            Console.CursorTop = Console.BufferHeight - 1;

            invDidDescDraw = true;
        }

        void doSelectedInventoryAction()
        {
            Player p = (Player)tiles[playerPos.X, playerPos.Y];

            // if an item gets added while the other is being removed,
            // it will crash because invDItems doesn't get updated while the inventory does.
            int idiCount = p.nInvItems;

            // Do the action, check if needs to be destroyed
            // It seems kindof redundant passing in the arguments you use to call the function itself
            if (p.inventory[invSelItem].actions[invActionSel].Act(ref p, invSelItem))
            {
                // destroy item
                for (int i = invSelItem; i < idiCount; i++)
                {
                    makeBlackSpace(invDItems[i]);
                }
                p.removeInventoryItem(invSelItem);

                // == seems unsafe, but if you mess arround with it it'll crash <i>anyway</i>
                if (invSelItem == p.nInvItems && invSelItem != 0)
                    invSelItem--;

                drawInventory(invSelItem);
            }

            drawInfoBar();
        }

        void inv_changeSelectedItem(int to)
        {
            Player p = (Player)tiles[playerPos.X, playerPos.Y];

            if (to < 0) to = p.nInvItems - 1;
            else if (to >= p.nInvItems) to = 0;

            invSelItem = to;

            invDidDescDraw = false;
            makeBlackSpace(invDescription);
            inv_handleCollision(invDescription);
            drawInvItem(p.inventory[invSelItem], Constants.invSelItemBorderColor, invDItems[invSelItem].begin);
            inv_drawDescription();
            inv_drawDescription();
        }

        void inv_handleCollision(InventoryDisplayItem item)
        {
            Player p = (Player)tiles[playerPos.X, playerPos.Y];

            for (int i = 0; i < p.nInvItems; i++)
            {
                if (inv_collides(item, invDItems[i]))
                {
                    drawInvItem(p.inventory[i], i == invSelItem ? Constants.invSelItemBorderColor : Constants.invItemBorderColor, invDItems[i].begin);
                }
            }
        }

        bool inv_collides(InventoryDisplayItem a, InventoryDisplayItem b)
        {
            return (a.begin.X + Constants.invDescriptionWidth > b.begin.X && a.begin.X < b.begin.X + Constants.invDescriptionWidth &&
                    a.begin.Y + (a.end.Y - a.begin.Y) > b.begin.Y && a.begin.Y < b.begin.Y + (b.end.Y - b.begin.Y));
        }
    }
}

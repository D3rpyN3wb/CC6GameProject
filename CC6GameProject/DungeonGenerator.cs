using System;

namespace CC6GameProject
{
    partial class Game
    {
        //maxvalue is the maxvalue + 1 at all times!
        Room[] createDungeons(ushort howManyRooms, Room size,
            int cSpawnTries = 100, int bSpawnTries = 100, byte corrMinLength = 5, byte corrMaxLength = 15,
            byte cbMinLength = 5, byte cbMaxLength = 10)
        {
            Point spawnRoomGenPoint = new Point(tiles.GetLength(0) / 2 + ran.Next(-10, 5), tiles.GetLength(1) / 2 + ran.Next(-10, 5));
            Room spawnRoom = new Room(spawnRoomGenPoint, new Point(spawnRoomGenPoint.X + ran.Next(4, 12), spawnRoomGenPoint.Y + ran.Next(4, 8)));
            spawnRoom.appendName(ref ran);
            generateRoom(spawnRoom);

            Room[] dungeons = new Room[howManyRooms + 1];
            dungeons[0] = spawnRoom;
            int a = 1;

            for (int i = 0; i < howManyRooms; i++)
            {
                Room r;
                Point cWhere = new Point(-1, -1);
                int cEnd = 0;
                Point[] corrP = { };
                byte dir = 0;

                bool fail = true;
                for (int t = 0; t < cSpawnTries; t++) // cSpawnTries: how many tries to generate a corridor on a random wall
                {
                    r = dungeons[ran.Next(0, a)]; // 0,a --> owl eyes?
                    dir = (byte)ran.Next(0, 4);
                    cWhere = getRandomWallPoint(r, dir);

                    corrP = getCorridorPoints(cWhere, dir, ran.Next(corrMinLength, corrMaxLength)); // corrMinLength, corrMaxLength: Length of corridor

                    if (corrP == null) continue;
                    cEnd = corrP.Length - 1;

                    if (!corrP[cEnd].same(-1, -1))
                    {
                        fail = false;
                        break;
                    }
                }

                if (fail) break;

                // Algorithm:
                // 1: Check if there's room for both corridor and the room itself
                // 2: Build both if so.
                //      when building, build the corridor needed.
                // goto 1
                int highestIndex = 0;
                for (int t = 0; t < bSpawnTries; t++) // bSpawnTries: How many tries to attach a room on a corridor
                {
                    int cIndex = ran.Next(0, corrP.Length);
                    Point connPoint = corrP[cIndex];


                    byte[] nDirArr = getPossibleCorridorDirections(dir);
                    byte nDir;

                    if (cEnd == cIndex)
                    {
                        nDir = getOppositeDirection(dir);
                    }
                    else nDir = nDirArr[ran.Next(0, nDirArr.Length)];


                    // cbMinLength, cbMaxLength: Length of connection between room and corridor
                    Point[] cPoints = getCorridorPoints(connPoint, getOppositeDirection(nDir), ran.Next(cbMinLength, cbMaxLength));
                    if (cPoints == null) continue;

                    // new Point(ran.Next(4, 19), ran.Next(5, 15))
                    // size: size of room
                    Room toBuild = generateRoom(cPoints[cPoints.Length - 1], new Point(ran.Next(size.where.X, size.where.Y), ran.Next(size.end.X, size.end.Y)), nDir, true, true);

                    if (toBuild != null)
                    {
                        if (cIndex > highestIndex) highestIndex = cIndex;

                        //Array.Resize(ref corrP, highestIndex + 1);
                        generateCorridor(corrP, highestIndex + 1);
                        dungeons[a] = toBuild;
                        generateCorridor(cPoints, cPoints.Length);

                        a++;

                        if (a == dungeons.Length)
                        {
                            i = howManyRooms;
                            break;
                        }
                    }
                }
            }

            if (a != dungeons.Length) Array.Resize(ref dungeons, a);

            // Add exit
            Point p = getRandomPointInRoom(dungeons[ran.Next(0, dungeons.Length)]);
            tiles[p.X, p.Y] = new Tile(TileType.Down);

            return dungeons;
        }

        void setDungeonToEmpty()
        {
            for (int a = 0; a < tiles.GetLength(0); a++)
                for (int b = 0; b < tiles.GetLength(1); b++)
                    tiles[a, b] = new Tile(noneTile);
        }

        bool canRoomBePlaced(Room r)
        {
            if (!isInScreen(r.where) || !isInScreen(r.end)) return false;
            for (int x = r.where.X; x <= r.end.X; x++) for (int y = r.where.Y; y <= r.end.Y; y++)
                    if (tiles[x, y].tiletype != noneTile) return false;
            return true;
        }

        void spawnPlayerInRoom(Room r, Player p)
        {
            Point where = r.where;
            Point end = r.end;

            where.X++;
            where.Y++;
            end.X--;
            end.Y--;

            playerPos = new Point(-1, -1);
            while (!isValidMove(playerPos))
                playerPos = new Point(ran.Next(where.X, end.X + 1), ran.Next(where.Y, end.Y + 1));

            p.needsToBeDrawn = true;
            p.lastTile = new Tile(TileType.Up);
            tiles[playerPos.X, playerPos.Y] = p;
        }

        Room generateRoom(Point p, Point size, byte dir, bool doorAtP = true, bool placeMatters = true)
        {
            Room r;

            // This switch has a point
            switch (dir)
            {
                default:
                    int mSize = ran.Next(1, size.X);
                    r = new Room(
                        new Point(p.X - mSize, p.Y),
                        new Point(p.X + size.X - mSize, p.Y + size.Y));
                    break;
                case 1:
                    mSize = ran.Next(1, size.X);
                    r = new Room(
                        new Point(p.X - mSize, p.Y - size.Y),
                        new Point(p.X + size.X - mSize, p.Y));
                    break;
                case 2:
                    mSize = ran.Next(1, size.Y);
                    r = new Room(
                        new Point(p.X, p.Y - mSize),
                        new Point(p.X + size.X, p.Y + size.Y - mSize));
                    break;
                case 3:
                    mSize = ran.Next(1, size.Y);
                    r = new Room(
                        new Point(p.X - size.X, p.Y - mSize),
                        new Point(p.X, p.Y + size.Y - mSize));
                    break;
            }

            if (placeMatters)
            {
                if (canRoomBePlaced(r)) generateRoom(r);
                else return null;
            }
            else generateRoom(r);

            if (doorAtP) tiles[p.X, p.Y] = new Tile(TileType.Corridor);


            r.appendName(ref ran);
            return r;
        }

        void generateRoom(Room r)
        {
            Point where = r.where;
            Point end = r.end;

            // horizontal
            for (int i = where.X; i <= end.X; i++)
            {
                tiles[i, where.Y] = new Wall(Constants.xWall);
                tiles[i, end.Y] = new Wall(Constants.xWall);
            }

            // vertical
            for (int i = where.Y; i <= end.Y; i++)
            {
                tiles[where.X, i] = new Wall(Constants.yWall);
                tiles[end.X, i] = new Wall(Constants.yWall);
            }

            // corners
            tiles[where.X, where.Y].drawChar = Constants.lupWall;
            tiles[where.X, end.Y].drawChar = Constants.ldownWall;
            tiles[end.X, where.Y].drawChar = Constants.rupWall;
            tiles[end.X, end.Y].drawChar = Constants.rdownWall;

            // Corners?
            where.X++;
            where.Y++;
            end.X--;
            end.Y--;


            // fill in rooms
            if (ran.Next(0, 50) == 0)
            {
                // Loot Room
                for (int x = where.X; x <= end.X; x++)
                    for (int y = where.Y; y <= end.Y; y++)
                    {
                        if (ran.Next(0, 20) == 0) tiles[x, y] = new Money(ran.Next(1, 7));
                        else if (ran.Next(0, 33) == 0)
                        {
                            switch (ran.Next(0, 5))
                            {
                                case 0:
                                case 1:
                                    tiles[x, y] = new Scroll(SpellGenerator.GenerateRandomSpellEffects(ref ran, ran.Next(2, 6)));
                                    break;
                                case 2:
                                    tiles[x, y] = new Scroll(SpellGenerator.GenerateMultipleUncommon(ref ran, ran.Next(1, 5)));
                                    break;
                                case 3:
                                    tiles[x, y] = new Scroll(SpellGenerator.GenerateMultipleRare(ref ran, ran.Next(1, 4)));
                                    break;
                                case 4:
                                    tiles[x, y] = new Scroll(SpellGenerator.GenerateMultipleCommon(ref ran, ran.Next(3, 8)));
                                    break;
                            }
                        }
                        else if (ran.Next(0, 500) == 0) tiles[x, y] = new Snake(ref ran);
                        else tiles[x, y].setTile(TileType.Air);
                    }
            }
            else if (ran.Next(0, 150) == 0)
            {
                // Enemy Room / Also this is where to add Mobs
                for (int x = where.X; x <= end.X; x++)
                    for (int y = where.Y; y <= end.Y; y++)
                    {
                        if (ran.Next(0, 25) == 0) tiles[x, y] = new Rat(ref ran, ran.Next(0, 3));
                        else if (ran.Next(0, 50) == 0) tiles[x, y] = new Goblin(ref ran, ran.Next(0, 2));
                        else if (ran.Next(0, 100) == 0) tiles[x, y] = new Orc(ref ran, ran.Next(0, 2));
                        else if (ran.Next(0, 35) == 0) tiles[x, y] = new Snake(ref ran, ran.Next(0, 3));
                        else if (ran.Next(0, 500) == 0) tiles[x, y] = new Dragon(ref ran, ran.Next(0, 2));
                        else tiles[x, y].setTile(TileType.Air);
                    }
            }
            else
            {
                // Mobless room
                for (int x = where.X; x <= end.X; x++)
                    for (int y = where.Y; y <= end.Y; y++)
                    {
                        if (ran.Next(0, 40) == 0) tiles[x, y] = new Money(ran.Next(1, 6));
                        else if (ran.Next(0, 909) == 0) tiles[x, y] = new Scroll(SpellGenerator.GenerateMultiple(ref ran));
                        else if (ran.Next(0, 700) < 2) tiles[x, y] = new Rat(ref ran);
                        else if (ran.Next(0, 1200) < 2) tiles[x, y] = new Snake(ref ran);
                        else if (ran.Next(0, 1500) < 2) tiles[x, y] = new Goblin(ref ran);
                        else if (ran.Next(0, 2200) < 2) tiles[x, y] = new Orc(ref ran);
                        else if (ran.Next(0, 5000) < 2) tiles[x, y] = new Dragon(ref ran);
                        else if (ran.Next(0, 1250) == 0) tiles[x, y] = new Chest(ref ran);
                        else tiles[x, y].setTile(TileType.Air);
                    }
            }
        }

        Point getRandomPointInRoom(Room r)
        {
            return new Point(ran.Next(++r.where.X, r.end.X), ran.Next(++r.where.Y, r.end.Y));
        }

        byte getOppositeDirection(byte dir)
        {
            switch (dir)
            {
                default: return 1;
                case 1: return 0;
                case 2: return 3;
                case 3: return 2;
            }
        }

        byte[] getPossibleCorridorDirections(byte dir)
        {
            switch (dir)
            {
                default: return new byte[] { 2, 3 };
                case 2:
                case 3: return new byte[] { 0, 1 };
            }
        }

        Point getIncrementByDirection(byte dir)
        {
            switch (dir)
            {
                default: return new Point(0, -1);
                case 1: return new Point(0, 1);
                case 2: return new Point(-1, 0);
                case 3: return new Point(1, 0);
            }
        }

        Point[] getCorridorPoints(Point p, byte dir, int howMany)
        {
            if (!isInScreen(p)) return null;

            Point d = getIncrementByDirection(dir);
            Point current = p;
            Point[] points = new Point[howMany];

            for (int i = 0; i < howMany; i++)
            {
                points[i] = current;
                current.X += d.X;
                current.Y += d.Y;
            }

            // Current is now the max, so we can easyally check if everything is right!
            if (!isInScreen(current)) return null;

            // Skip first one because that one is usually a wall, that needs to be replaced.
            for (int i = 1; i < howMany; i++)
                if (tiles[points[i].X, points[i].Y].tiletype != noneTile) return null;

            return points;
        }

        void generateCorridor(Point[] p, int max, TileType t = TileType.Corridor)
        {
            for (int i = 0; i < max; i++)
                tiles[p[i].X, p[i].Y] = new Tile(t);
        }

        Room getDungeonAt(Point p)
        {
            // not effecient
            for (int i = 0; i < rooms.Length; i++)
                if (p.X >= rooms[i].where.X && p.X <= rooms[i].end.X && p.Y >= rooms[i].where.Y && p.Y <= rooms[i].end.Y) return rooms[i];
            return null;
        }

        /// <summary>
        /// Is itz clears to you?
        /// </summary>
        /// <param name="r">Boxs to sitz in</param>
        /// <param name="direction">Favoritez direction to lie down: 0 = up, 1 = down, 2 = left, 3 = right</param>
        /// <returns>How happy catz is!</returns>
        Point getRandomWallPoint(Room r, byte direction)
        {
            switch (direction)
            {
                default: return new Point(ran.Next(r.where.X + 1, r.end.X), r.where.Y); // Return up
                case 1: return new Point(ran.Next(r.where.X + 1, r.end.X), r.end.Y); // Down,
                case 2: return new Point(r.where.X, ran.Next(r.where.Y + 1, r.end.Y)); // Left...
                case 3: return new Point(r.end.X, ran.Next(r.where.Y + 1, r.end.Y)); // Right!
            }
        }
    }
}

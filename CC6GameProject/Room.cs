using System;

namespace CC6GameProject
{
    class Room
    {
        public string name = "";
        public Point where, end;

        public Room(Point w, Point e)
        {
            this.where = w;
            this.end = e;
        }

        public void appendName(ref Random ran)
        {
            string dung = Constants.dungeonNameParts[ran.Next(0, Constants.dungeonNameParts.Length)];

            if (ran.Next(2) == 0)
                this.name = string.Format("{0} of The {1} {2}", dung, Constants.namefwords[ran.Next(0, Constants.namefwords.Length)],
                    Constants.nameswords[ran.Next(0, Constants.nameswords.Length)]);
            else if (ran.Next(2) == 0)
            {
                if (dung[dung.Length - 1] != 's') dung += 's';
                this.name = string.Format("{0} of {1}", dung, Constants.namewords[ran.Next(0, Constants.namewords.Length)]);
            }
            else
            {
                this.name = string.Format("The {0} {1}", Constants.namefwords[ran.Next(0, Constants.namewords.Length)], dung);
            }
        }
    }
}

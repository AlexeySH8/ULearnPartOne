using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName()
        {
            return "Terrain.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = null };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }
    }

    public class Player : ICreature
    {
        public static int X, Y = 0;
        public static int DX, DY = 0;

        public string GetImageFileName()
        {
            return "Digger.png";
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            bool die = false;
            if (conflictedObject is Sack || conflictedObject is Monster) die = true;
            if (die) Game.IsOver = true;
            return die;
        }

        public CreatureCommand Act(int x, int y)
        {
            if (Game.KeyPressed == System.Windows.Forms.Keys.Up &&
                y - 1 >= 0 && !(Game.Map[x, y - 1] is Sack))
                return new CreatureCommand() { DeltaX = 0, DeltaY = -1, TransformTo = null };
            else if (Game.KeyPressed == System.Windows.Forms.Keys.Left &&
                x - 1 >= 0 && !(Game.Map[x - 1, y] is Sack))
                return new CreatureCommand() { DeltaX = -1, DeltaY = 0, TransformTo = null };
            else if (Game.KeyPressed == System.Windows.Forms.Keys.Right &&
                x + 1 < Game.MapWidth && !(Game.Map[x + 1, y] is Sack))
                return new CreatureCommand() { DeltaX = 1, DeltaY = 0, TransformTo = null };
            else if (Game.KeyPressed == System.Windows.Forms.Keys.Down &&
                y + 1 < Game.MapHeight && !(Game.Map[x, y + 1] is Sack))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1, TransformTo = null };
            else return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = null };
        }
    }

    public class Sack : ICreature
    {
        public int Fall = 0;
        public string GetImageFileName()
        {
            return "Sack.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public CreatureCommand Act(int x, int y)
        {
            var nextObjY = Game.Map[x, y];

            if (y + 1 < Game.MapHeight)
                nextObjY = Game.Map[x, y + 1];

            if (y + 1 < Game.MapHeight && ((nextObjY is null) ||
                (Fall >= 1 && nextObjY is Player || Fall >= 1 && nextObjY is Monster)))
            {
                Fall++;
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1, TransformTo = null };
            }
            else
            {
                if (Fall > 1 && (nextObjY is Terrain || nextObjY is Sack ||
                    nextObjY is Gold || y - 1 == Game.MapHeight))
                {
                    Fall = 0;
                    return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
                }
                else
                {
                    Fall = 0;
                    return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = null };
                }
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }
    }

    public class Gold : ICreature
    {
        public string GetImageFileName()
        {
            return "Gold.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = null };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player) Game.Scores = Game.Scores + 10;
            return true;
        }
    }

    public class Monster : ICreature
    {
        public string GetImageFileName()
        {
            return "Monster.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public CreatureCommand Act(int x, int y)
        {
            int dx = 0;
            int dy = 0;

            if (IsPlayerAlive())
            {
                if (Player.X == x)
                {
                    if (Player.Y < y) dy = -1;
                    else if (Player.Y > y) dy = 1;
                }

                else if (Player.Y == y)
                {
                    if (Player.X < x) dx = -1;
                    else if (Player.X > x) dx = 1;
                }
                else
                {
                    if (Player.X < x) dx = -1;
                    else if (Player.X > x) dx = 1;
                }
            }
            else return Stay();

            if (!(x + dx >= 0 && x + dx < Game.MapWidth &&
                y + dy >= 0 && y + dy < Game.MapHeight))
                return Stay();

            var map = Game.Map[x + dx, y + dy];
            if (map != null)
                if (map.ToString() == "Digger.Terrain" ||
                    map.ToString() == "Digger.Sack" ||
                    map.ToString() == "Digger.Monster")
                    return Stay();
            return new CreatureCommand() { DeltaX = dx, DeltaY = dy };
        }

        static private CreatureCommand Stay()
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        static private bool IsPlayerAlive()
        {
            for (int i = 0; i < Game.MapWidth; i++)
                for (int j = 0; j < Game.MapHeight; j++)
                {
                    var map = Game.Map[i, j];
                    if (map != null)
                    {
                        if (map.ToString() == "Digger.Player")
                        {
                            Player.X = i;
                            Player.Y = j;
                            return true;
                        }
                    }
                }
            return false;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Monster || conflictedObject is Sack;
        }
    }
}


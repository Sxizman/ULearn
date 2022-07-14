using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Player;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var deltaX = 0;
            var deltaY = 0;

            var key = Game.KeyPressed;
            if (key == Keys.Right && CanMoveTo(x + 1, y))
                deltaX = 1;
            if (key == Keys.Left && CanMoveTo(x - 1, y))
                deltaX = -1;
            if (key == Keys.Down && CanMoveTo(x, y + 1))
                deltaY = 1;
            if (key == Keys.Up && CanMoveTo(x, y - 1))
                deltaY = -1;

            if (Game.Map[x + deltaX, y + deltaY] is Gold)
                Game.Scores += 10;

            return new CreatureCommand
            {
                DeltaX = deltaX,
                DeltaY = deltaY
            };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }

        private bool CanMoveTo(int x, int y)
        {
            return (x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight) && !(Game.Map[x, y] is Sack);
        }
    }

    public class Sack : ICreature
    {
        private int _fallDistance = 0;

        public CreatureCommand Act(int x, int y)
        {
            var deltaY = 0;
            ICreature transformTo = null;

            if (CanFallDown(x, y))
            {
                ++_fallDistance;
                deltaY = 1;
            }
            else
            {
                if (_fallDistance > 1)
                    transformTo = new Gold();
                _fallDistance = 0;
            }

            return new CreatureCommand
            {
                DeltaX = 0,
                DeltaY = deltaY,
                TransformTo = transformTo
            };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }

        private bool CanFallDown(int x, int y)
        {
            return (y + 1) < Game.MapHeight &&
                (Game.Map[x, y + 1] is null || 
                _fallDistance > 0 && (Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster));
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Player || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var optimalDirection = GetOptimalMoveDirection(x, y);

            return new CreatureCommand
            {
                DeltaX = optimalDirection.Item1,
                DeltaY = optimalDirection.Item2
            };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }

        private (int, int) GetOptimalMoveDirection(int x, int y)
        {
            var left = MinPathLengthToPlayer(x - 1, y);
            var right = MinPathLengthToPlayer(x + 1, y);
            var up = MinPathLengthToPlayer(x, y - 1);
            var down = MinPathLengthToPlayer(x, y + 1);

            var min = Math.Min(Math.Min(left, right), Math.Min(up, down));
            if (min == int.MaxValue)
                return (0, 0);

            if (left == min) return (-1, 0);
            if (right == min) return (1, 0);
            if (up == min) return (0, -1);
            if (down == min) return (0, 1);
            return (0, 0);
        }

        private bool CanMoveTo(int x, int y)
        {
            return (x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight) &&
                !(Game.Map[x, y] is Terrain || Game.Map[x, y] is Sack || Game.Map[x, y] is Monster);
        }
    }
}
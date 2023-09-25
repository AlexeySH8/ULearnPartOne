using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;

namespace GeometryPainting
{
    //Напишите здесь код, который заставит работать методы segment.GetColor и segment.SetColor
    public static class SegmentExtensions
    {
        public static Dictionary<Segment, Color> Dictionary = new Dictionary<Segment, Color>();

        public static Color GetColor(this Segment segment)
        {
            if (Dictionary.ContainsKey(segment))
                return Dictionary[segment];
            else return Color.Black;
        }

        public static void SetColor(this Segment segment, Color newColor)
        {
            Dictionary[segment] = newColor;
        }
    }
}

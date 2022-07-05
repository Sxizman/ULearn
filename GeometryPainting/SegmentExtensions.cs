using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;

namespace GeometryPainting
{
    public static class SegmentColorExtension
    {
        private static Dictionary<Segment, Color> _colors = new Dictionary<Segment, Color>();

        public static void SetColor(this Segment s, Color c)
        {
            _colors[s] = c;
        }

        public static Color GetColor(this Segment s)
        {
            return _colors.ContainsKey(s) ? _colors[s] : Color.Black;
        }
    }
}

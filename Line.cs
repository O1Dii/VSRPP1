using System;
using System.Drawing;

namespace VSRPP1
{
    [Serializable()]
    public class Line
    {
        public Line(Color color, Point start, Point end)
        {
            this.Color = ColorTranslator.ToHtml(color);
            this.Start = start;
            this.End = end;
        }

        public string Color { get; set; }
        
        public Point Start { get; set; }
        public Point End { get; set; }
    }
}

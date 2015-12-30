using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SFMLUI
{
    public static class FloatRectExtensions
    {
        public static IEnumerable<Vector2f> GetPoints(this FloatRect rect)
        {
            var list = new List<Vector2f>
            {
                rect.TopLeft(),
                rect.TopRight(),
                rect.BottomRight(),
                rect.BottomLeft()
            };

            return list;
        }

        public static float Right(this FloatRect rect)
        {
            return rect.Left + rect.Width;
        }

        public static float Bottom(this FloatRect rect)
        {
            return rect.Top + rect.Height;
        }

        public static Vector2f TopLeft(this FloatRect rect)
        {
            return new Vector2f(rect.Left, rect.Top);
        }

        public static Vector2f TopRight(this FloatRect rect)
        {
            return new Vector2f(rect.Right(), rect.Top);
        }

        public static Vector2f BottomLeft(this FloatRect rect)
        {
            return new Vector2f(rect.Left, rect.Bottom());
        }

        public static Vector2f BottomRight(this FloatRect rect)
        {
            return new Vector2f(rect.Right(), rect.Bottom());
        }
    }
}
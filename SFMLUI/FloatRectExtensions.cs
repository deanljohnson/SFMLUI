using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SFMLUI
{
    public static class FloatRectExtensions
    {
        /// <summary>
        /// Get the four corners of the given FloatRect
        /// </summary>
        public static IEnumerable<Vector2f> GetCorners(this FloatRect rect)
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

        /// <summary>
        /// Returns the X position of the right side of the given FloatRect
        /// </summary>
        public static float Right(this FloatRect rect)
        {
            return rect.Left + rect.Width;
        }

        /// <summary>
        /// Returns the Y position of the bottom side of the given FloatRect
        /// </summary>
        public static float Bottom(this FloatRect rect)
        {
            return rect.Top + rect.Height;
        }

        /// <summary>
        /// Returns the positions of the TopLeft corner of the given FloatRect
        /// </summary>
        public static Vector2f TopLeft(this FloatRect rect)
        {
            return new Vector2f(rect.Left, rect.Top);
        }

        /// <summary>
        /// Returns the positions of the TopRight corner of the given FloatRect
        /// </summary>
        public static Vector2f TopRight(this FloatRect rect)
        {
            return new Vector2f(rect.Right(), rect.Top);
        }

        /// <summary>
        /// Returns the positions of the BottomLeft corner of the given FloatRect
        /// </summary>
        public static Vector2f BottomLeft(this FloatRect rect)
        {
            return new Vector2f(rect.Left, rect.Bottom());
        }

        /// <summary>
        /// Returns the positions of the BottomRight corner of the given FloatRect
        /// </summary>
        public static Vector2f BottomRight(this FloatRect rect)
        {
            return new Vector2f(rect.Right(), rect.Bottom());
        }
    }
}
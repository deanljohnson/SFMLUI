using SFML.System;

namespace SFMLUI
{
    public static class UILayouter
    {
        /// <summary>
        ///     Centers this UIElement on another UIElement.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement CenterOn(this UIElement self, UIElement other)
        {
            var selfCenter = self.GetCenter();
            var otherCenter = other.GetCenter();

            var dif = otherCenter - selfCenter;

            self.Position += dif;

            return self;
        }

        /// <summary>
        ///     Centers this UIElement on another UIElement vertically.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement CenterVertically(this UIElement self, UIElement other,
            Vector2f padding = default(Vector2f))
        {
            var selfCenter = self.GetCenter();
            var otherCenter = other.GetCenter();

            var dif = otherCenter - selfCenter;

            self.Position += new Vector2f(0, dif.Y) + padding;

            return self;
        }

        /// <summary>
        ///     Centers this UIElement on another UIElement horizontally.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement CenterHorizontally(this UIElement self, UIElement other,
            Vector2f padding = default(Vector2f))
        {
            var selfCenter = self.GetCenter();
            var otherCenter = other.GetCenter();

            var dif = otherCenter - selfCenter;

            self.Position += new Vector2f(dif.X, 0) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the top edge of this UIElement with another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement AlignTops(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfTop = self.GetBounds().Top;
            var otherTop = other.GetBounds().Top;

            var dif = otherTop - selfTop;

            self.Position += new Vector2f(0, dif) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the left edge of this UIElement with another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement AlignLefts(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfLeft = self.GetBounds().Left;
            var otherLeft = other.GetBounds().Left;

            var dif = otherLeft - selfLeft;

            self.Position += new Vector2f(dif, 0) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the right edge of this UIElement with another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement AlignRights(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfBounds = self.GetBounds();
            var otherBounds = other.GetBounds();
            var selfRight = selfBounds.Left + selfBounds.Width;
            var otherRight = otherBounds.Left + otherBounds.Width;

            var dif = otherRight - selfRight;

            self.Position += new Vector2f(dif, 0) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the bottom edge of this UIElement with another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement AlignBottoms(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfBounds = self.GetBounds();
            var otherBounds = other.GetBounds();
            var selfBottom = selfBounds.Top + selfBounds.Height;
            var otherBottom = otherBounds.Top + otherBounds.Height;

            var dif = otherBottom - selfBottom;

            self.Position += new Vector2f(0, dif) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the bottom edge of this UIElement with the top edge of another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement StackOnTop(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfBounds = self.GetBounds();
            var otherBounds = other.GetBounds();
            var selfBottom = selfBounds.Top + selfBounds.Height;
            var otherTop = otherBounds.Top;

            var dif = otherTop - selfBottom;

            self.Position += new Vector2f(0, dif) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the top edge of this UIElement with the bottom edge of another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement StackOnBottom(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfBounds = self.GetBounds();
            var otherBounds = other.GetBounds();
            var selfTop = selfBounds.Top;
            var otherBottom = otherBounds.Top + otherBounds.Height;

            var dif = otherBottom - selfTop;

            self.Position += new Vector2f(0, dif) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the right edge of this UIElement with the left edge of another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement StackOnLeft(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfBounds = self.GetBounds();
            var otherBounds = other.GetBounds();
            var selfRight = selfBounds.Left + selfBounds.Width;
            var otherLeft = otherBounds.Left;

            var dif = otherLeft - selfRight;

            self.Position += new Vector2f(dif, 0) + padding;

            return self;
        }

        /// <summary>
        ///     Aligns the left edge of this UIElement with the right edge of another.
        ///     Both elements should be members of the same UIPanel.
        /// </summary>
        public static UIElement StackOnRight(this UIElement self, UIElement other, Vector2f padding = default(Vector2f))
        {
            var selfBounds = self.GetBounds();
            var otherBounds = other.GetBounds();
            var selfLeft = selfBounds.Left;
            var otherRight = otherBounds.Left + otherBounds.Width;

            var dif = otherRight - selfLeft;

            self.Position += new Vector2f(dif, 0) + padding;

            return self;
        }
    }
}
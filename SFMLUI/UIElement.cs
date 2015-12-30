using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    /// <summary>
    ///     Base class for all UI elements
    /// </summary>
    public abstract class UIElement : Transformable, Drawable
    {
        public virtual bool HasKeyboardFocus { get; set; } = false;
        public bool Active { get; set; } = true;
        public abstract void Update();
        public abstract bool HandleMouseMove(Vector2f mousePos);
        public abstract bool HandleMouseClick(Vector2f mousePos, Mouse.Button button);
        /// <summary>
        /// Returns a FloatRect representing this UIElements bounds with it's Transform applied.
        /// </summary>
        public abstract FloatRect GetBounds();
        /// <summary>
        /// Gets the center of this UIElement with it's Transform applied
        /// </summary>
        public abstract Vector2f GetCenter();

        public void Move(Vector2f amount)
        {
            Position += amount;
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
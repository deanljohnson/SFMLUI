using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public class UIExpandingFrame : UIPanel
    {
        private Sprite m_Sprite { get; }
        private IntRect m_CornerRect { get; }
        private IntRect m_SideRect { get; }
        private IntRect m_FillRect { get; }

        private float m_WidthScale { get; set; }
        private float m_HeightScale { get; set; }
        private Vector2f m_StartSize { get; }
        private Vector2f m_Padding { get; }

        public UIExpandingFrame(Vector2f startSize, Vector2f padding, Texture texture, IntRect corner, IntRect side, IntRect fill)
        {
            m_Sprite = new Sprite(texture);
            m_CornerRect = corner;
            m_SideRect = side;
            m_FillRect = fill;

            m_StartSize = startSize;
            m_Padding = padding;

            m_WidthScale = ((2 * corner.Width) + fill.Width) / startSize.X;
            m_HeightScale = ((2 * corner.Height) + fill.Height) / startSize.Y;
        }

        //TODO: There are probably some ways to improve this method - ~13 new Vector2f's... inefficient
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            var selfBounds = GetBounds();

            var desiredSideWidth = selfBounds.Width - (m_CornerRect.Width*2);
            var desiredSideHeight = selfBounds.Height - (m_CornerRect.Height*2);
            var sideWidthScale = desiredSideWidth / m_SideRect.Width;
            var sideHeightScale = desiredSideHeight / m_SideRect.Width;//We use the sides width here because we rotate the sprite to draw the sides of the frame

            //-----Corners-----//
            m_Sprite.Scale = new Vector2f(1, 1);
            m_Sprite.TextureRect = m_CornerRect;
            //TopLeft
            m_Sprite.Rotation = 0f;
            m_Sprite.Position = new Vector2f(0, 0);
            target.Draw(m_Sprite, states);
            //TopRight
            m_Sprite.Rotation = 90f;
            m_Sprite.Position = new Vector2f(selfBounds.Width, 0);
            target.Draw(m_Sprite, states);
            //BottomRight
            m_Sprite.Rotation = 180f;
            m_Sprite.Position = new Vector2f(selfBounds.Width, selfBounds.Height);
            target.Draw(m_Sprite, states);
            //BottomLeft
            m_Sprite.Rotation = 270f;
            m_Sprite.Position = new Vector2f(0, selfBounds.Height);
            target.Draw(m_Sprite, states);

            //----Top/Bottom-----//
            m_Sprite.TextureRect = m_SideRect;
            m_Sprite.Scale = new Vector2f(sideWidthScale, 1f);
            //Top
            m_Sprite.Rotation = 0f;
            m_Sprite.Position = new Vector2f(m_CornerRect.Width, 0);
            target.Draw(m_Sprite, states);
            //Bottom
            m_Sprite.Rotation = 180f;
            m_Sprite.Position = new Vector2f(selfBounds.Width - m_CornerRect.Width, selfBounds.Height);
            target.Draw(m_Sprite, states);

            //-----Left/Right-----//
            m_Sprite.Scale = new Vector2f(sideHeightScale, 1f);
            //Right
            m_Sprite.Rotation = 90f;
            m_Sprite.Position = new Vector2f(selfBounds.Width, m_CornerRect.Height);
            target.Draw(m_Sprite, states);
            //Left
            m_Sprite.Rotation = 270f;
            m_Sprite.Position = new Vector2f(0, selfBounds.Height - m_CornerRect.Height);
            target.Draw(m_Sprite, states);

            //-----Fill-----//
            m_Sprite.TextureRect = m_FillRect;
            m_Sprite.Rotation = 0f;
            m_Sprite.Scale = new Vector2f(sideWidthScale, sideHeightScale);
            m_Sprite.Position = new Vector2f(m_CornerRect.Width, m_CornerRect.Height);
            target.Draw(m_Sprite, states);

            //Move the transform so that children are drawn within the fill area, with padding applied
            states.Transform.Translate(m_CornerRect.Width + m_Padding.X, m_CornerRect.Height + m_Padding.Y);

            foreach (var child in Children.Where(c => c.Active))
            {
                target.Draw(child, states);
            }
        }

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            var baseResult = base.HandleMouseMove(mousePos - m_Padding);

            if (baseResult) return true;

            //Moving over the frame itself counts as handling the event
            return GetBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var baseResult = base.HandleMouseClick(mousePos - m_Padding, button);

            if (baseResult) return true;

            //Clicking on the frame itself counts as handling the event
            return GetBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override FloatRect GetBounds()
        {
            return Transform.TransformRect(new FloatRect(0, 0, m_StartSize.X * m_WidthScale, m_StartSize.Y * m_HeightScale));
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        public override void Add(UIElement child)
        {
            base.Add(child);

            Resize();
        }

        protected virtual void Resize()
        {
            var childsBounds = Children.Select(child => child.GetBounds()).ToList();

            var lowestX = childsBounds.Min(b => b.Left);
            var lowestY = childsBounds.Min(b => b.Top);
            var highestX = childsBounds.Max(b => b.Right());
            var highestY = childsBounds.Max(b => b.Bottom());

            var childBB = new FloatRect(lowestX, lowestY, highestX - lowestX, highestY - lowestY);
            var totalPadding = new Vector2f(m_Padding.X + m_CornerRect.Width, m_Padding.Y + m_CornerRect.Height);

            var localRect = new FloatRect(0, 0, childBB.Width + childBB.Left + (totalPadding.X * 2), childBB.Height + childBB.Top + (totalPadding.Y * 2));

            m_WidthScale = localRect.Width / m_StartSize.X;
            m_HeightScale = localRect.Height / m_StartSize.Y;
        }

        protected override Vector2f ToLocalCoordinates(Vector2f source)
        {
            return base.ToLocalCoordinates(source) - new Vector2f(m_CornerRect.Width, m_CornerRect.Height);
        }
    }
}
using System.Linq;
using Engine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public class UIExpandingFrame : UIPanel
    {
        private Sprite m_CornerSprite { get; }
        private Sprite m_SideSprite { get; }
        private Sprite m_FillSprite { get; }
        private float m_WidthScale { get; set; }
        private float m_HeightScale { get; set; }

        public UIExpandingFrame(Vector2f startSize, Texture texture, IntRect corner, IntRect side, IntRect fill)
        {
            m_CornerSprite = new Sprite(texture, corner);
            m_SideSprite = new Sprite(texture, side);
            m_FillSprite = new Sprite(texture, fill);

            m_WidthScale = (startSize.X - (2*corner.Width))/side.Width;
            m_HeightScale = (startSize.Y - (2*corner.Width))/side.Width;
            //We use width because we will be rotating the side
        }

        //TODO: There are probably some ways to improve this method - ~14 new Vector2f's... inefficient
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            var selfBounds = GetGlobalBounds();
            var cornerBounds = m_CornerSprite.GetLocalBounds();

            //TopLeft & Top
            m_CornerSprite.Rotation = 0f;
            m_CornerSprite.Position = new Vector2f(0, 0);
            target.Draw(m_CornerSprite, states);
            m_SideSprite.Rotation = 0f;
            m_SideSprite.Scale = new Vector2f(m_WidthScale, 1f);
            m_SideSprite.Position = new Vector2f(cornerBounds.Width, 0);
            target.Draw(m_SideSprite, states);

            //TopRight & Right
            m_CornerSprite.Rotation = 90f;
            m_CornerSprite.Position = new Vector2f(selfBounds.Width, 0);
            target.Draw(m_CornerSprite, states);
            m_SideSprite.Rotation = 90f;
            m_SideSprite.Scale = new Vector2f(m_HeightScale, 1f);
            m_SideSprite.Position = new Vector2f(selfBounds.Width, cornerBounds.Height);
            target.Draw(m_SideSprite, states);

            //BottomRight & Bottom
            m_CornerSprite.Rotation = 180f;
            m_CornerSprite.Position = new Vector2f(selfBounds.Width, selfBounds.Height);
            target.Draw(m_CornerSprite, states);
            m_SideSprite.Rotation = 180f;
            m_SideSprite.Scale = new Vector2f(m_WidthScale, 1f);
            m_SideSprite.Position = new Vector2f(selfBounds.Width - cornerBounds.Width, selfBounds.Height);
            target.Draw(m_SideSprite, states);

            //BottomLeft & Left
            m_CornerSprite.Rotation = 270f;
            m_CornerSprite.Position = new Vector2f(0, selfBounds.Height);
            target.Draw(m_CornerSprite, states);
            m_SideSprite.Rotation = 270f;
            m_SideSprite.Scale = new Vector2f(m_HeightScale, 1f);
            m_SideSprite.Position = new Vector2f(0, selfBounds.Height - cornerBounds.Height);
            target.Draw(m_SideSprite, states);

            //Fill
            m_FillSprite.Scale = new Vector2f(m_WidthScale, m_HeightScale);
            m_FillSprite.Position = new Vector2f(cornerBounds.Width, cornerBounds.Height);
            target.Draw(m_FillSprite, states);

            //Move the transform so that children are drawn within the fill area
            states.Transform.Translate(cornerBounds.Width, cornerBounds.Height);

            foreach (var child in Children.Where(c => c.Active))
            {
                target.Draw(child, states);
            }
        }

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            var baseResult = base.HandleMouseMove(mousePos);

            if (baseResult) return true;

            return GetGlobalBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var baseResult = base.HandleMouseClick(mousePos, button);

            if (baseResult) return true;

            return GetGlobalBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override FloatRect GetGlobalBounds()
        {
            float w = 0, h = 0;

            w += 2*(m_CornerSprite.TextureRect.Width*m_CornerSprite.Scale.X);
            h += 2*(m_CornerSprite.TextureRect.Height*m_CornerSprite.Scale.Y);

            w += m_SideSprite.TextureRect.Width*m_WidthScale;
            h += m_SideSprite.TextureRect.Width*m_HeightScale;
            //Using width because we rotate to draw the left and right sides

            return Transform.TransformRect(new FloatRect(0, 0, w, h));
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetGlobalBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        public override void Add(UIElement child)
        {
            base.Add(child);

            Resize();
        }

        protected virtual void Resize()
        {
            var childsBounds = Children.Select(child => child.GetGlobalBounds()).ToList();

            var lowestX = childsBounds.Min(b => b.Left);
            var lowestY = childsBounds.Min(b => b.Top);
            var highestX = childsBounds.Max(b => b.Right());
            var highestY = childsBounds.Max(b => b.Bottom());

            var localRect = new FloatRect(lowestX, lowestY, (highestX - lowestX), (highestY - lowestY));

            m_WidthScale = localRect.Right()/m_SideSprite.TextureRect.Width;
            m_HeightScale = localRect.Bottom()/m_SideSprite.TextureRect.Width;
        }

        protected override Vector2f Vector2fInLocalCoordinates(Vector2f source)
        {
            var cornerBounds = m_CornerSprite.GetLocalBounds();
            return base.Vector2fInLocalCoordinates(source) - new Vector2f(cornerBounds.Width, cornerBounds.Height);
        }
    }
}
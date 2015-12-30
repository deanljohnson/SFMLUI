using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public class UIIcon : UIElement
    {
        public Texture Texture
        {
            get { return m_Sprite.Texture; }
            set { m_Sprite.Texture = value; }
        }

        public IntRect Rect
        {
            get { return m_Sprite.TextureRect; }
            set { m_Sprite.TextureRect = value; }
        }

        private Sprite m_Sprite { get; }

        public UIIcon()
        {
            m_Sprite = new Sprite();
        }

        public UIIcon(Texture texture, IntRect rect)
        {
            m_Sprite = new Sprite(texture, rect);
        }

        public override void Update()
        {
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            target.Draw(m_Sprite, states);
        }

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            return GetBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            return GetBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override FloatRect GetBounds()
        {
            return Transform.TransformRect(m_Sprite.GetGlobalBounds());
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }
    }
}
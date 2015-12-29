using SFML.Graphics;
using SFML.System;

namespace SFMLUI
{
    public class UIButton : UIClickable
    {
        public IntRect NormalRect { get; set; }
        public IntRect HoverRect { get; set; } = new IntRect(0, 0, 0, 0);
        public IntRect ClickRect { get; set; } = new IntRect(0, 0, 0, 0);
        public Texture Texture { get; set; }
        private Sprite m_Sprite { get; }

        public UIButton(Texture texture, IntRect normalRect)
        {
            Texture = texture;
            NormalRect = normalRect;
            m_Sprite = new Sprite(Texture, NormalRect);
        }

        public override void Update()
        {
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            target.Draw(m_Sprite, states);
        }

        public override FloatRect GetGlobalBounds()
        {
            return Transform.TransformRect(m_Sprite.GetGlobalBounds());
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetGlobalBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        protected override bool Contains(Vector2f pos)
        {
            return Transform.TransformRect(m_Sprite.GetGlobalBounds())
                .Contains(pos.X, pos.Y);
        }

        protected override void SwitchState(ClickableState newState)
        {
            base.SwitchState(newState);

            SetRect(newState);
        }

        private void SetRect(ClickableState state)
        {
            var changeRect = NormalRect;

            switch (state)
            {
                case ClickableState.None:
                    changeRect = NormalRect;
                    break;
                case ClickableState.Hover:
                    changeRect = HoverRect;
                    break;
                case ClickableState.Active:
                    changeRect = ClickRect;
                    break;
            }

            if (changeRect.Width != 0f || changeRect.Height != 0f)
            {
                m_Sprite.TextureRect = changeRect;
            }
        }
    }
}
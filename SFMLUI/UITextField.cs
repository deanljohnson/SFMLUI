using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public class UITextField : UISelectable, ITakesKeyboardFocus
    {
        public override bool HasKeyboardFocus
        {
            get { return base.HasKeyboardFocus; }
            set
            {
                if (base.HasKeyboardFocus != value)
                {
                    base.HasKeyboardFocus = value;
                    if (value) OnGainKeyboardFocus?.Invoke();
                    else OnLoseKeyboardFocus?.Invoke();
                }
            }
        }

        public Color BackgroundColor
        {
            get { return m_Box.FillColor; }
            set { m_Box.FillColor = value; }
        }

        public Vector2f Size
        {
            get { return m_Box.Size; }
            set { m_Box.Size = value; }
        }

        public String Text
        {
            get { return m_Caption.DisplayedText; }
            set { m_Caption.DisplayedText = value; }
        }

        private RectangleShape m_Box { get; }
        private UICaption m_Caption { get; }

        public UITextField(Window window, Vector2f size, Color backgroundColor, Font font, uint fontSize,
            Color fontColor)
        {
            m_Box = new RectangleShape(size)
            {
                FillColor = backgroundColor
            };

            m_Caption = new UICaption("", font, fontSize, fontColor);
            m_Caption.AlignLefts(this);
            m_Caption.CenterVertically(this, new Vector2f(0, -fontSize/2));

            window.TextEntered += HandleTextEntered;
        }

        public override void Update()
        {
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            target.Draw(m_Box, states);
            target.Draw(m_Caption, states);
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var baseResult = base.HandleMouseClick(mousePos, button);

            if (Selected && !HasKeyboardFocus)
            {
                HasKeyboardFocus = true;
            }
            else if (!Selected && HasKeyboardFocus)
            {
                HasKeyboardFocus = false;
            }

            return baseResult;
        }

        public override FloatRect GetGlobalBounds()
        {
            return Transform.TransformRect(m_Box.GetGlobalBounds());
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetGlobalBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        protected override bool Contains(Vector2f pos)
        {
            return GetGlobalBounds().Contains(pos.X, pos.Y);
        }

        private void HandleTextEntered(object sender, TextEventArgs e)
        {
            if (!HasKeyboardFocus) return;

            if (e.Unicode == "\b" && m_Caption.DisplayedText.Length > 0)
            {
                m_Caption.DisplayedText = m_Caption.DisplayedText.Remove(m_Caption.DisplayedText.Length - 1);
            }
            else if (e.Unicode == "\r")
            {
                HasKeyboardFocus = false;
            }
            else
            {
                m_Caption.DisplayedText += e.Unicode;
            }
        }

        public Action OnGainKeyboardFocus { get; set; }
        public Action OnLoseKeyboardFocus { get; set; }
    }
}
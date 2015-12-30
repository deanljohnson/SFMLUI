using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    /// <summary>
    ///     UIElement to be used for simple lines of text that do not
    ///     require wrapping or advanced formatting.
    /// </summary>
    public class UICaption : UIElement
    {
        private Text m_Text { get; }
        public Func<String> TextFunction { get; set; }

        public Font Font
        {
            get { return m_Text.Font; }
            set { m_Text.Font = value; }
        }

        public uint FontSize
        {
            get { return m_Text.CharacterSize; }
            set { m_Text.CharacterSize = value; }
        }

        public Color FontColor
        {
            get { return m_Text.Color; }
            set { m_Text.Color = value; }
        }

        public string DisplayedText
        {
            get { return m_Text.DisplayedString; }
            set { m_Text.DisplayedString = value; }
        }

        public UICaption(String startingText, Font font, uint fontSize, Color fontColor)
        {
            Debug.Assert(font != null);

            m_Text = new Text(startingText, font, fontSize)
            {
                Color = fontColor
            };
        }

        public UICaption(Func<string> textFunction, Font font, uint fontSize, Color fontColor)
            : this(textFunction(), font, fontSize, fontColor)
        {
            TextFunction = textFunction;
        }

        public override void Update()
        {
            if (TextFunction != null)
            {
                UpdateText();
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            target.Draw(m_Text, states);
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
            return Transform.TransformRect(m_Text.GetGlobalBounds());
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        private void UpdateText()
        {
            m_Text.DisplayedString = DisplayedText;
        }
    }
}
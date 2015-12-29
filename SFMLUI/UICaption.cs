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
        private string m_displayedText;
        private Font m_font;
        private Color m_fontColor;
        private uint m_fontSize;
        public Func<String> TextFunction { get; set; }

        public Font Font
        {
            get { return m_font; }
            set
            {
                m_font = value;
                m_Text.Font = m_font;
            }
        }

        public uint FontSize
        {
            get { return m_fontSize; }
            set
            {
                m_fontSize = value;
                m_Text.CharacterSize = m_fontSize;
            }
        }

        public Color FontColor
        {
            get { return m_fontColor; }
            set
            {
                m_fontColor = value;
                m_Text.Color = m_fontColor;
            }
        }

        public string DisplayedText
        {
            get { return m_displayedText; }
            set
            {
                m_displayedText = value;
                m_Text.DisplayedString = m_displayedText;
            }
        }

        private Text m_Text { get; }

        public UICaption(String startingText, Font font, uint fontSize, Color fontColor)
        {
            Debug.Assert(font != null);

            m_displayedText = startingText;
            m_font = font;
            m_fontSize = fontSize;
            m_fontColor = fontColor;

            m_Text = new Text(m_displayedText, Font, FontSize)
            {
                Color = FontColor
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
            return GetGlobalBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            return GetGlobalBounds().Contains(mousePos.X, mousePos.Y);
        }

        public override FloatRect GetGlobalBounds()
        {
            return Transform.TransformRect(m_Text.GetGlobalBounds());
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetGlobalBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        private void UpdateText()
        {
            DisplayedText = TextFunction();
            m_Text.DisplayedString = DisplayedText;
        }
    }
}
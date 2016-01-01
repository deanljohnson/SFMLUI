using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLUI.Interfaces;

namespace SFMLUI.Controls
{
    public class UITextField : UISelectable, ITakesKeyboardFocus, ITextualElement
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

        public Font Font
        {
            get { return m_Caption.Font; }
            set { m_Caption.Font = value; }
        }

        public uint FontSize
        {
            get { return m_Caption.FontSize; }
            set { m_Caption.FontSize = value; }
        }

        public Color FontColor
        {
            get { return m_Caption.FontColor; }
            set { m_Caption.FontColor = value; }
        }

        public Action OnGainKeyboardFocus { get; set; }
        public Action OnLoseKeyboardFocus { get; set; }

        private RectangleShape m_Box { get; }
        private UICaption m_Caption { get; }
        private Sprite m_CaptionSprite { get; }
        private RenderTexture m_CaptionRenderTexture { get; }
        private int m_CaretPosition { get; set; }

        public UITextField(Window window, Vector2f size, Color backgroundColor, Font font, uint fontSize,
            Color fontColor)
        {
            m_Box = new RectangleShape(size)
            {
                FillColor = backgroundColor
            };
            m_CaptionRenderTexture = new RenderTexture((uint) size.X, (uint) size.Y);
            m_CaptionSprite = new Sprite();

            m_Caption = new UICaption("", font, fontSize, fontColor);

            //TODO: Remove the need to pass a window object around
            //Ideally a user would pass the desired text/keyPress events to the UI
            window.TextEntered += HandleTextEntered;
            window.KeyPressed += HandleKeyPressed;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            //It's a code smell to have caret handling in a rendering method,
            //but the only other way I thought to do this was much messier.
            if (HasKeyboardFocus)
            {
                InsertCaret();
            }

            states.Transform.Combine(Transform);

            target.Draw(m_Box, states);

            DrawCaptionToRenderTexture();

            //Pull the Texture off of the RenderTexture, apply it to a Sprite, and render it to the RenderTarget
            m_CaptionSprite.Texture = m_CaptionRenderTexture.Texture;
            m_CaptionSprite.Scale = new Vector2f(1, -1); //I don't know why, but I need to flip the y-axis...
            m_CaptionSprite.Position = new Vector2f(0, m_Box.Size.Y); //Because we flipped the axis, we need a translation
            target.Draw(m_CaptionSprite, states);

            if (HasKeyboardFocus)
            {
                RemoveCaret();
            }
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var baseResult = base.HandleMouseClick(mousePos, button);

            if (Selected && !HasKeyboardFocus)
            {
                GainFocus();
            }
            else if (!Selected && HasKeyboardFocus)
            {
                LoseFocus();
            }

            return baseResult;
        }

        public override FloatRect GetBounds()
        {
            return Transform.TransformRect(m_Box.GetGlobalBounds());
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        protected override bool Contains(Vector2f pos)
        {
            return GetBounds().Contains(pos.X, pos.Y);
        }

        private void DrawCaptionToRenderTexture()
        {
            //Find out where to move the Caption to display the proper piece
            var capBounds = m_Caption.GetBounds();
            var caretPos = m_Caption.FindCharacterPos(m_CaretPosition).X;
            if (caretPos > m_CaptionRenderTexture.Size.X - capBounds.Left)
            {
                //BUG: When this happens during masking, the caret is not visible.
                m_Caption.Position = new Vector2f(-caretPos + m_CaptionRenderTexture.Size.X, m_Caption.Position.Y);
            }
            if (caretPos < -capBounds.Left)
            {
                //BUG: When this happens during masking, while using backspace to delete characters, you cannot see what is being deleted
                m_Caption.Position = new Vector2f(-caretPos, m_Caption.Position.Y);
            }

            //Draw the Caption to a RenderTexture so we can mask it
            m_CaptionRenderTexture.Clear(Color.Transparent);
            m_CaptionRenderTexture.Draw(m_Caption);
        }

        private void HandleTextEntered(object sender, TextEventArgs e)
        {
            if (!HasKeyboardFocus) return;

            if (e.Unicode == "\b")
            {
                RemoveOneAtCaret();
            }
            else if (e.Unicode == "\r")
            {
                LoseFocus();
                State = SelectableState.Unselected;
            }
            else
            {
                InsertTextAtCaret(e.Unicode);
            }
        }

        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Left)
            {
                MoveCaret(-1);
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                MoveCaret(1);
            }
        }

        private void LoseFocus()
        {
            HasKeyboardFocus = false;
        }

        private void GainFocus()
        {
            HasKeyboardFocus = true;
            m_CaretPosition = Text.Length;
        }

        private void InsertTextAtCaret(String text)
        {
            Text = Text.Insert(m_CaretPosition, text);
            m_CaretPosition += text.Length;
        }

        private void RemoveOneAtCaret()
        {
            if (Text.Length > 0 && m_CaretPosition > 0)
            {
                m_CaretPosition--;
                Text = Text.Remove(m_CaretPosition, 1);
            }
        }

        private void MoveCaret(int amount)
        {
            m_CaretPosition += amount;

            //Keep the Caret within bounds
            if (m_CaretPosition < 0)
            {
                m_CaretPosition = 0;
            }
            else if (m_CaretPosition > Text.Length)
            {
                m_CaretPosition = Text.Length;
            }
        }

        private void InsertCaret()
        {
            Text = Text.Insert(m_CaretPosition, "|");
        }

        private void RemoveCaret()
        {
            Text = Text.Remove(m_CaretPosition, 1);
        }
    }
}
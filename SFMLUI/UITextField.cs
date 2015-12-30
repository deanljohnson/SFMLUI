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

        public Vector2f TextOffset
        {
            set { m_Caption.Position = value; }
        }

        public Action OnGainKeyboardFocus { get; set; }
        public Action OnLoseKeyboardFocus { get; set; }

        private RectangleShape m_Box { get; }
        private UICaption m_Caption { get; }
        private int m_CaretPosition { get; set; }

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

            //TODO: Remove the need to pass a window object around
            //Ideally a user would pass the desired text/keyPress events to the UI
            window.TextEntered += HandleTextEntered;
            window.KeyPressed += HandleKeyPressed;
        }

        public override void Update()
        {
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            //It's a code smell to have caret handling in a rendering method,
            //but the alternative is much messier in this case
            if (HasKeyboardFocus)
            {
                InsertCaret();
            }

            states.Transform.Combine(Transform);

            target.Draw(m_Box, states);
            target.Draw(m_Caption, states);

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
            if (Text.Length > 0)
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
using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public class UIDropDownList : UISelectable
    {
        private RectangleShape m_Box { get; }
        private UICaption m_SelectedCaption { get; }
        private bool m_Extended { get; set; }

        public int SelectedIndex { get; set; }
        public List<String> Items { get; set; }
        public String SelectedItem => Items.Count > 0 ? Items[SelectedIndex] : "";

        public UIDropDownList(Vector2f size, Color backgroundColor, Font font, uint fontSize,
            Color fontColor)
        {
            m_Box = new RectangleShape(size)
            {
                FillColor = backgroundColor
            };

            m_SelectedCaption = new UICaption("", font, fontSize, fontColor);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            m_SelectedCaption.DisplayedText = Items.Count > 0 
                ? Items[SelectedIndex]
                : "";

            target.Draw(m_Box, states);
            target.Draw(m_SelectedCaption, states);
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var baseResult = base.HandleMouseClick(mousePos, button);

            if (Selected)
            {
                var clicked = !Mouse.IsButtonPressed(button);
                if (m_Extended && clicked)
                {
                    SetSelectedItem(mousePos);
                    Collapse();
                    State = SelectableState.Unselected;
                }
                else if (!m_Extended)
                {
                    Extend();
                }
            }
            else if (!Selected && m_Extended)
            {
                Collapse();
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

            return Position - Origin + new Vector2f(selfBounds.Width / 2f, selfBounds.Height / 2f);
        }

        protected override bool Contains(Vector2f pos)
        {
            return GetBounds().Contains(pos.X, pos.Y);
        }

        private void Extend()
        {
            m_Extended = true;
        }

        private void Collapse()
        {
            m_Extended = false;
        }

        private void SetSelectedItem(Vector2f mousePos)
        {

        }
    }
}

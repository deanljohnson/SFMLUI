using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public class UIDropDownList : UISelectable
    {
        private int m_SelectedIndex;
        private RectangleShape m_SelectedBox { get; }
        private RectangleShape m_ExtendedBox { get; }
        private UICaption m_SelectedCaption { get; }
        private UICaption m_ExtendedCaption { get; }
        private bool m_Extended { get; set; }

        public int SelectedIndex {
            get { return m_SelectedIndex; }
            set
            {
                if (m_SelectedIndex == value) return;

                m_SelectedIndex = value;
                OnSelectionChangeAction(this, SelectedItem);
            }
        }
        public List<String> Items { get; set; } = new List<String>();
        public String SelectedItem => Items.Count > 0 ? Items[SelectedIndex] : "";
        public Action<UIDropDownList, String> OnSelectionChangeAction { get; set; }

        public UIDropDownList(Vector2f size, Color backgroundColor, Font font, uint fontSize,
            Color fontColor)
        {
            m_SelectedBox = new RectangleShape(size)
            {
                FillColor = backgroundColor
            };
            m_ExtendedBox = new RectangleShape
            {
                FillColor = backgroundColor
            };

            m_SelectedCaption = new UICaption("", font, fontSize, fontColor);
            m_ExtendedCaption = new UICaption("", font, fontSize, fontColor);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            m_SelectedCaption.DisplayedText = SelectedItem;
            target.Draw(m_SelectedBox, states);
            target.Draw(m_SelectedCaption, states);

            if (m_Extended)
            {
                target.Draw(m_ExtendedBox, states);
                target.Draw(m_ExtendedCaption, states);
            }
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var baseResult = base.HandleMouseClick(mousePos, button);
            
            if (Selected)
            {
                var clicked = !Mouse.IsButtonPressed(button);
                if (m_Extended && clicked)
                {
                    SetSelectedIndex(mousePos);
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
            if (!m_Extended)
            {
                return Transform.TransformRect(m_SelectedBox.GetGlobalBounds());
            }

            var selectedBounds = m_SelectedBox.GetGlobalBounds();
            var extendedBounds = m_ExtendedBox.GetGlobalBounds();
            var combined = new FloatRect(selectedBounds.Left, selectedBounds.Top, selectedBounds.Width, selectedBounds.Height + extendedBounds.Height);
            return Transform.TransformRect(combined);
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

        private FloatRect GetExtendedAreaGlobalBounds()
        {
            return Transform.TransformRect(m_ExtendedBox.GetGlobalBounds());
        }

        private void Extend()
        {
            m_Extended = true;

            var newCaption = String.Empty;
            for (var i = 0; i < Items.Count; i++)
            {
                newCaption += Items[i];
                if (i != Items.Count - 1)
                {
                    newCaption += "\n";
                }
            }

            m_ExtendedCaption.DisplayedText = newCaption;
            m_ExtendedCaption.Position = new Vector2f(m_SelectedCaption.Position.X, m_SelectedBox.GetGlobalBounds().Height);

            var extendedBounds = m_ExtendedCaption.GetBounds();
            m_ExtendedBox.Size = new Vector2f(m_SelectedBox.Size.X, extendedBounds.Height);
            m_ExtendedBox.Position = m_ExtendedCaption.Position;
        }

        private void Collapse()
        {
            m_Extended = false;

            m_ExtendedCaption.DisplayedText = String.Empty;

            //This shouldn't be needed, but we put it here just in case some code
            //tries to refer to the extended box size even though it's not extended
            m_ExtendedBox.Size = new Vector2f(0, 0);
        }

        private void SetSelectedIndex(Vector2f mousePos)
        {
            var bounds = GetExtendedAreaGlobalBounds();

            if (mousePos.Y < bounds.Top || mousePos.Y > bounds.Bottom())
            {
                return;
            }

            var heightInExtendedArea = mousePos.Y - bounds.Top;
            var singleItemHeight = bounds.Height/Items.Count;
            var index = (int)Math.Floor(heightInExtendedArea / singleItemHeight);

            SelectedIndex = index;
        }
    }
}

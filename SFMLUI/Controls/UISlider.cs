using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLUI.BaseTypes;

namespace SFMLUI.Controls
{
    public enum SliderStyle
    {
        Gradient,
        Meter
    }

    public class UISlider : UIElement
    {
        private bool m_HasMouseFocus { get; set; }
        private Color m_LowColor { get; }
        private Color m_HighColor { get; }

        private VertexArray m_BackgroundShape { get; }
        private RectangleShape m_ForegroundShape { get; }
        private RectangleShape m_Selector { get; }

        private Vector2f m_Size { get; }

        public Func<float> ValueFunc { get; set; }
        public Action<UISlider, float> OnValueChangedAction { get; set; }

        public float Value
        {
            get { return m_Selector.Position.X / m_Size.X; }
            set
            {
                if (value > 1f) value = 1f;
                if (value < 0f) value = 0f;

                if (value != Value)
                {
                    m_Selector.Position = new Vector2f(m_Size.X * value, m_Selector.Position.Y);

                    if (m_ForegroundShape != null)
                    {
                        m_ForegroundShape.Scale = new Vector2f(value, m_ForegroundShape.Scale.Y);
                    }

                    OnValueChangedAction?.Invoke(this, Value);
                }
            }
        }

        public UISlider(Vector2f size, Color lowColor, Color highColor, float selectorWidth, Color selectorColor, SliderStyle style)
        {
            m_Size = size;
            m_LowColor = lowColor;
            m_HighColor = highColor;
            
            m_BackgroundShape = new VertexArray(PrimitiveType.Quads);

            if (style == SliderStyle.Gradient)
            {
                m_BackgroundShape.Append(new Vertex(new Vector2f(0, 0), m_LowColor));
                m_BackgroundShape.Append(new Vertex(new Vector2f(m_Size.X, 0), m_HighColor));
                m_BackgroundShape.Append(new Vertex(m_Size, m_HighColor));
                m_BackgroundShape.Append(new Vertex(new Vector2f(0, m_Size.Y), m_LowColor));

                m_ForegroundShape = null;
            }
            else if (style == SliderStyle.Meter)
            {
                m_BackgroundShape.Append(new Vertex(new Vector2f(0, 0), m_HighColor));
                m_BackgroundShape.Append(new Vertex(new Vector2f(m_Size.X, 0), m_HighColor));
                m_BackgroundShape.Append(new Vertex(m_Size, m_HighColor));
                m_BackgroundShape.Append(new Vertex(new Vector2f(0, m_Size.Y), m_HighColor));

                m_ForegroundShape = new RectangleShape(size)
                {
                    FillColor = lowColor
                };
            }

            m_Selector = new RectangleShape(new Vector2f(selectorWidth, m_Size.Y))
            {
                FillColor = selectorColor,
                Position = new Vector2f(m_Size.X, 0),
                Origin = new Vector2f(selectorWidth / 2f, 0)
            };
        }

        public override void Update()
        {
            if (ValueFunc != null)
            {
                Value = ValueFunc();
            }

            base.Update();
        }

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            var bounds = GetBounds();

            if (m_HasMouseFocus)
            {
                if (!Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    m_HasMouseFocus = false;
                }
                else
                {
                    var xPos = mousePos.X - bounds.Left;
                    var val = xPos / m_Size.X;

                    Value = val;

                    return true;
                }
            }

            if (bounds.Contains(mousePos.X, mousePos.Y))
            {
                return true;
            }

            return false;
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var bounds = GetBounds();

            if (bounds.Contains(mousePos.X, mousePos.Y))
            {
                if (button != Mouse.Button.Left) return true;

                m_HasMouseFocus = Mouse.IsButtonPressed(Mouse.Button.Left);

                if (m_HasMouseFocus)
                {
                    var xPos = mousePos.X - bounds.Left;
                    var val = xPos / m_Size.X;

                    Value = val;
                }

                return true;
            }

            return false;
        }

        public override FloatRect GetBounds()
        {
            return Transform.TransformRect(m_BackgroundShape.Bounds);
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetBounds();

            return Position - Origin + new Vector2f(selfBounds.Width / 2f, selfBounds.Height / 2f);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            target.Draw(m_BackgroundShape, states);

            if (m_ForegroundShape != null)
            {
                target.Draw(m_ForegroundShape, states);
            }

            target.Draw(m_Selector, states);
        }
    }
}

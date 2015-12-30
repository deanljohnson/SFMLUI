using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public class UIPanel : UIElement, IEnumerable<UIElement>
    {
        public override bool HasKeyboardFocus
        {
            get { return Children.Any(c => c.HasKeyboardFocus); }
            set { throw new InvalidOperationException("UIPanel's cannot have KeyboardFocus set"); }
        }

        protected List<UIElement> Children { get; set; }

        public UIPanel()
        {
            Children = new List<UIElement>();
        }

        public UIPanel(Vector2f position)
            : this()
        {
            Position = position;
        }

        public override void Update()
        {
            foreach (var child in Children)
            {
                child.Update();
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            foreach (var child in Children)
            {
                target.Draw(child, states);
            }
        }

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            var result = false;
            var localMousePos = ToLocalCoordinates(mousePos);

            foreach (var child in Children)
            {
                result = child.HandleMouseMove(localMousePos) || result;
            }

            return result;
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var result = false;
            var localMousePos = ToLocalCoordinates(mousePos);

            foreach (var child in Children)
            {
                result = child.HandleMouseClick(localMousePos, button) || result;
            }

            return result;
        }

        public override FloatRect GetBounds()
        {
            var childsBounds = Children.Select(child => child.GetBounds()).ToList();

            var lowestX = childsBounds.Min(b => b.Left);
            var lowestY = childsBounds.Min(b => b.Top);
            var highestX = childsBounds.Max(b => b.Right());
            var highestY = childsBounds.Max(b => b.Bottom());

            var localRect = new FloatRect(lowestX, lowestY, (highestX - lowestX), (highestY - lowestY));

            return Transform.TransformRect(localRect);
        }

        public override Vector2f GetCenter()
        {
            var selfBounds = GetBounds();

            return Position - Origin + new Vector2f(selfBounds.Width/2f, selfBounds.Height/2f);
        }

        public virtual void Add(UIElement element)
        {
            Children.Add(element);
        }

        protected virtual Vector2f ToLocalCoordinates(Vector2f source)
        {
            return InverseTransform.TransformPoint(source);
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Children).GetEnumerator();
        }
    }
}
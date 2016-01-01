using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    /// <summary>
    ///     The base class for building a UI. Acts as the base container for all UIElements.
    /// </summary>
    public class UIBase : Drawable, IContainer
    {
        public bool HasKeyboardFocus
        {
            get { return m_Children.Any(c => c.HasKeyboardFocus); }
        }

        private List<UIElement> m_Children { get; }

        public UIBase()
        {
            m_Children = new List<UIElement>();
        }

        public void Update()
        {
            foreach (var child in m_Children.Where(c => c.Active))
            {
                child.Update();
            }
        }

        /// <summary>
        /// Adds the given UIElement to the UIBase.
        /// </summary>
        public void Add(UIElement element)
        {
            m_Children.Add(element);
        }

        /// <summary>
        /// Removes the given UIElement to the UIBase.
        /// </summary>
        public bool Remove(UIElement element)
        {
            if (m_Children.Remove(element))
            {
                return true;
            }

            return m_Children.OfType<IContainer>().Any(child => child.Remove(element));
        }

        public bool Contains(UIElement element)
        {
            if (m_Children.Contains(element))
            {
                return true;
            }

            return m_Children.OfType<IContainer>().Any(child => child.Contains(element));
        }

        /// <summary>
        /// Posts a new mouse position to the UI. Returns whether or not any UIElements handled the event.
        /// </summary>
        public bool PostMouseMoveToUI(Vector2f mousePos)
        {
            var result = false;

            foreach (var child in m_Children.Where(c => c.Active))
            {
                result = child.HandleMouseMove(mousePos) || result;
            }

            return result;
        }

        /// <summary>
        /// Posts a mouse click to the UI. Returns whether or not any UIElements handled the event.
        /// </summary>
        public bool PostMouseClickEventToUI(Vector2f mousePos, Mouse.Button button)
        {
            var result = false;

            foreach (var child in m_Children.Where(c => c.Active))
            {
                result = child.HandleMouseClick(mousePos, button) || result;
            }

            return result;
        }

        /// <summary>
        /// Draws all children of this UIBase.
        /// </summary>
        public void Draw(RenderTarget target, RenderStates states)
        {
            var view = new View(target.GetView());
            target.SetView(target.DefaultView);

            foreach (var child in m_Children.Where(c => c.Active))
            {
                target.Draw(child, states);
            }

            target.SetView(view);
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return m_Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) m_Children).GetEnumerator();
        }
    }
}
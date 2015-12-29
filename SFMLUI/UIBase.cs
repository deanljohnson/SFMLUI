using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    /// <summary>
    ///     The bases class for building a UI. Acts as the base container for all UIElements.
    /// </summary>
    public class UIBase : Drawable
    {
        public bool HasKeyboardFocus
        {
            get { return m_Children.Any(c => c.HasKeyboardFocus); }
        }

        private List<UIElement> m_Children { get; }
        private Window m_Window { get; }

        public UIBase(Window window)
        {
            Active = true;
            m_Window = window;
            m_Children = new List<UIElement>();
        }

        public void Update()
        {
            foreach (var child in m_Children.Where(c => c.Active))
            {
                child.Update();
            }
        }

        public void Add(UIElement element)
        {
            m_Children.Add(element);
        }

        public void Remove(UIElement element)
        {
            m_Children.Remove(element);
        }

        private bool PostMouseMoveToUI(Vector2f mousePos)
        {
            var result = false;

            foreach (var child in m_Children.Where(c => c.Active))
            {
                result = child.HandleMouseMove(mousePos) || result;
            }

            return result;
        }

        private bool PostMouseClickEventToUI(Vector2f mousePos, Mouse.Button button)
        {
            var result = false;

            foreach (var child in m_Children.Where(c => c.Active))
            {
                result = child.HandleMouseClick(mousePos, button) || result;
            }

            return result;
        }

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

        public bool Active { get; set; }
    }
}
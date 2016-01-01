using System.Collections.Generic;

namespace SFMLUI
{
    public interface IContainer : IEnumerable<UIElement>
    {
        /// <summary>
        /// Adds the given UIElement to this IContainer
        /// </summary>
        void Add(UIElement element);
        /// <summary>
        /// Removes the given UIElement from this IContainer or 
        /// any of it's children that implement IContainer
        /// </summary>
        bool Remove(UIElement element);
        /// <summary>
        /// Returns whether this IContainer or any children that
        /// implement IContainer contain the given UIElement
        /// </summary>
        bool Contains(UIElement element);
    }
}
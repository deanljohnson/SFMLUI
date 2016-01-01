using SFML.Graphics;

namespace SFMLUI.Interfaces
{
    public interface ITextualElement
    {
        Font Font { get; set; }
        uint FontSize { get; set; }
        Color FontColor { get; set; }
    }
}
using SFML.Graphics;

namespace SFMLUI
{
    public interface ITextualElement
    {
        Font Font { get; set; }
        uint FontSize { get; set; }
        Color FontColor { get; set; }
    }
}
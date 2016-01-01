using SFML.Graphics;

namespace SFMLUI
{
    public class TextOptions
    {
        public Font Font { get; set; }
        public uint FontSize { get; set; }
        public Color Color { get; set; }

        public TextOptions(Font font, uint fontSize, Color color)
        {
            Font = font;
            FontSize = fontSize;
            Color = color;
        }
    }
}

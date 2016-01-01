using System;
using SFML.Graphics;
using SFMLUI.Interfaces;

namespace SFMLUI
{
    /// <summary>
    /// A button with a caption that stays centered
    /// </summary>
    public class UICaptionedButton : UIButton, ITextualElement
    {
        protected UICaption Caption { get; set; }

        public Font Font
        {
            get { return Caption.Font; }
            set
            {
                Caption.Font = value;
                Caption.CenterOn(this);
            }
        }

        public uint FontSize
        {
            get { return Caption.FontSize; }
            set
            {
                Caption.FontSize = value;
                Caption.CenterOn(this);
            }
        }

        public Color FontColor
        {
            get { return Caption.FontColor; }
            set
            {
                Caption.FontColor = value;
                Caption.CenterOn(this);
            }
        }

        protected UICaptionedButton(Texture texture, IntRect normalRect, UICaption caption)
            : base(texture, normalRect)
        {
            Caption = caption;
            Caption.CenterOn(this);
        }

        public UICaptionedButton(Texture texture, IntRect normalRect,
            String text, Font font, uint fontSize, Color fontColor)
            : this(texture, normalRect, new UICaption(text, font, fontSize, fontColor))
        {
        }

        public override void Update()
        {
            base.Update();

            Caption.Update();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            states.Transform.Combine(Transform);
            target.Draw(Caption, states);
        }
    }
}
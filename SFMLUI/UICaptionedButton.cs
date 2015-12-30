using System;
using SFML.Graphics;

namespace SFMLUI
{
    public class UICaptionedButton : UIButton
    {
        public UICaption Caption { get; set; }

        public UICaptionedButton(Texture texture, IntRect normalRect, UICaption caption)
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
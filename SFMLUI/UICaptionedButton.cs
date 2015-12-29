using System;
using SFML.Graphics;

namespace SFMLUI
{
    public class UICaptionedButton : UIButton
    {
        public UICaption Caption { get; set; }

        public UICaptionedButton(Texture texture, IntRect normalRect,
            String text, Font font, uint fontSize, Color fontColor)
            : base(texture, normalRect)
        {
            Caption = new UICaption(text, font, fontSize, fontColor);
            Caption.CenterOn(this);
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
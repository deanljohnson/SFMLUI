using System;
using SFML.System;
using SFML.Window;

namespace SFMLUI.BaseTypes
{
    public enum CheckableState
    {
        Checked,
        Unchecked
    }

    public abstract class UICheckable : UIElement
    {
        public Action<UICheckable> OnCheck { get; set; }
        public Action<UICheckable> OnUncheck { get; set; }
        public CheckableState State { get; set; } = CheckableState.Unchecked;
        public bool Checked => State == CheckableState.Checked;

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            return Contains(mousePos);
        }

        /// <summary>
        /// Toggles state between Checked/Unchecked if the mouse click occurs within this UIElement
        /// </summary>
        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            var clicked = !Mouse.IsButtonPressed(Mouse.Button.Left);
            var mouseIsInside = Contains(mousePos);

            if (mouseIsInside && clicked)
            {
                ToggleState();
                return true;
            }

            return false;
        }

        protected abstract bool Contains(Vector2f pos);

        protected virtual void ToggleState()
        {
            SwitchState(State == CheckableState.Checked 
                ? CheckableState.Unchecked 
                : CheckableState.Checked);
        }

        protected virtual void SwitchState(CheckableState newState)
        {
            if (newState == State)
            {
                return;
            }

            State = newState;
            switch (State)
            {
                case CheckableState.Unchecked:
                    OnUncheck?.Invoke(this);
                    return;
                case CheckableState.Checked:
                    OnCheck?.Invoke(this);
                    return;
            }

            throw new Exception($"Unrecognized State {State}");
        }
    }
}

using System;
using System.Diagnostics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
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

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            switch (State)
            {
                case CheckableState.Checked:
                    return CheckedHandleMouseClick(mousePos);
                case CheckableState.Unchecked:
                    return UncheckedHandleMouseClick(mousePos);
            }

            throw new Exception("Unrecognized State");
        }

        protected abstract bool Contains(Vector2f mousePos);

        protected virtual void SwitchState(CheckableState newState)
        {
            State = newState;
            switch (State)
            {
                case CheckableState.Unchecked:
                    OnUncheck?.Invoke(this);
                    break;
                case CheckableState.Checked:
                    OnCheck?.Invoke(this);
                    break;
            }
        }

        protected bool CheckedHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == CheckableState.Checked);

            if (Contains(mousePos) && !Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                SwitchState(CheckableState.Unchecked);
                return true;
            }

            return false;
        }

        protected bool UncheckedHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == CheckableState.Unchecked);

            if (Contains(mousePos) && !Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                SwitchState(CheckableState.Checked);
                return true;
            }

            return false;
        }
    }
}
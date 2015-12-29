using System;
using System.Diagnostics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public enum ClickableState
    {
        None,
        Hover,
        Active
    }

    /// <summary>
    ///     An abstract UIElement that can be Clicked and Hovered by the mouse
    /// </summary>
    public abstract class UIClickable : UIElement
    {
        public Action<UIClickable> OnDown { get; set; }
        public Action<UIClickable> WhileDown { get; set; }
        public Action<UIClickable> WhileHover { get; set; }
        public Action<UIClickable> OnClick { get; set; }
        public Action<UIClickable> OnHover { get; set; }
        public ClickableState State { get; set; } = ClickableState.None;

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            switch (State)
            {
                case ClickableState.None:
                    return NormalHandleMouseMove(mousePos);
                case ClickableState.Hover:
                    return HoverHandleMouseMove(mousePos);
                case ClickableState.Active:
                    return ActiveHandleMouseMove(mousePos);
            }

            throw new Exception("Unrecognized State");
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            switch (State)
            {
                case ClickableState.None:
                    return NormalHandleMouseClick(mousePos);
                case ClickableState.Hover:
                    return HoverHandleMouseClick(mousePos);
                case ClickableState.Active:
                    return ActiveHandleMouseClick(mousePos);
            }

            throw new Exception("Unrecognized State");
        }

        protected abstract bool Contains(Vector2f mousePos);

        /// <summary>
        ///     Switches the current state to the given ClickableState, and calls OnDown/OnHover as needed
        /// </summary>
        protected virtual void SwitchState(ClickableState newState)
        {
            State = newState;
            switch (State)
            {
                case ClickableState.None:
                    break;
                case ClickableState.Hover:
                    OnHover?.Invoke(this);
                    break;
                case ClickableState.Active:
                    OnDown?.Invoke(this);
                    break;
            }
        }

        protected bool NormalHandleMouseMove(Vector2f mousePos)
        {
            Debug.Assert(State == ClickableState.None);

            if (Contains(mousePos))
            {
                SwitchState(ClickableState.Hover);
                return true;
            }

            return false;
        }

        protected bool HoverHandleMouseMove(Vector2f mousePos)
        {
            Debug.Assert(State == ClickableState.Hover);

            if (Contains(mousePos))
            {
                WhileHover?.Invoke(this);
                return true;
            }

            SwitchState(ClickableState.None);
            return false;
        }

        protected bool ActiveHandleMouseMove(Vector2f mousePos)
        {
            Debug.Assert(State == ClickableState.Active);

            if (Contains(mousePos))
            {
                return true;
            }

            SwitchState(ClickableState.None);
            return false;
        }

        protected bool NormalHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == ClickableState.None);

            if (Contains(mousePos))
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    SwitchState(ClickableState.Active);
                    return true;
                }
                SwitchState(ClickableState.Hover);
            }

            return false;
        }

        protected bool HoverHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == ClickableState.Hover);

            if (Contains(mousePos))
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    SwitchState(ClickableState.Active);
                }

                return true;
            }

            SwitchState(ClickableState.None);
            return false;
        }

        protected bool ActiveHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == ClickableState.Active);

            if (Contains(mousePos))
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    WhileDown?.Invoke(this);
                }
                else
                {
                    OnClick?.Invoke(this);
                    SwitchState(ClickableState.None);
                }

                return true;
            }

            SwitchState(ClickableState.None);
            return false;
        }
    }
}
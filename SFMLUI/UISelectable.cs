using System;
using System.Diagnostics;
using SFML.System;
using SFML.Window;

namespace SFMLUI
{
    public enum SelectableState
    {
        Selected,
        Unselected
    }

    /// <summary>
    /// An abstract UIElement that can be Selected and Unselected by the mouse.
    /// </summary>
    public abstract class UISelectable : UIElement
    {
        public Action<UISelectable> OnSelect { get; set; }
        public Action<UISelectable> OnUnselect { get; set; }
        public SelectableState State { get; set; } = SelectableState.Unselected;
        protected bool Selected => State == SelectableState.Selected;

        public override bool HandleMouseMove(Vector2f mousePos)
        {
            return Contains(mousePos);
        }

        public override bool HandleMouseClick(Vector2f mousePos, Mouse.Button button)
        {
            switch (State)
            {
                case SelectableState.Selected:
                    return SelectedHandleMouseClick(mousePos);
                case SelectableState.Unselected:
                    return UnselectedHandleMouseClick(mousePos);
            }

            throw new Exception($"Unrecognized State {State}");
        }

        protected abstract bool Contains(Vector2f pos);

        protected virtual void SwitchState(SelectableState newState)
        {
            if (newState == State)
            {
                return;
            }

            State = newState;
            switch (State)
            {
                case SelectableState.Unselected:
                    OnUnselect?.Invoke(this);
                    return;
                case SelectableState.Selected:
                    OnSelect?.Invoke(this);
                    return;
            }

            throw new Exception($"Unrecognized State {State}");
        }

        protected bool SelectedHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == SelectableState.Selected);

            var clicked = !Mouse.IsButtonPressed(Mouse.Button.Left);
            var mouseIsInside = Contains(mousePos);

            if (mouseIsInside) return true;

            if (clicked)
            {
                SwitchState(SelectableState.Unselected);
            }
                
            return false;
        }

        protected bool UnselectedHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == SelectableState.Unselected);

            var clicked = !Mouse.IsButtonPressed(Mouse.Button.Left);
            var mouseIsInside = Contains(mousePos);

            if (!mouseIsInside) return false;

            if (clicked)
            {
                SwitchState(SelectableState.Selected);
            }
            return true;
        }
    }
}
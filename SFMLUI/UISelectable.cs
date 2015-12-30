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
        public bool Selected => State == SelectableState.Selected;

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

            if (!Contains(mousePos) && !Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                SwitchState(SelectableState.Unselected);
                return true;
            }

            return false;
        }

        protected bool UnselectedHandleMouseClick(Vector2f mousePos)
        {
            Debug.Assert(State == SelectableState.Unselected);

            if (Contains(mousePos) && !Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                SwitchState(SelectableState.Selected);
                return true;
            }

            return false;
        }
    }
}
using System;

namespace SFMLUI.Interfaces
{
    internal interface ITakesKeyboardFocus
    {
        Action OnGainKeyboardFocus { get; set; }
        Action OnLoseKeyboardFocus { get; set; }
    }
}
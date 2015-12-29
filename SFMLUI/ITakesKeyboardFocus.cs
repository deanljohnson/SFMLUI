using System;

namespace SFMLUI
{
    internal interface ITakesKeyboardFocus
    {
        Action OnGainKeyboardFocus { get; set; }
        Action OnLoseKeyboardFocus { get; set; }
    }
}
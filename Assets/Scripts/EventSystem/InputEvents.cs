using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputEvents
{
    public event Action onKeyPressed;

    public void KeyPressed()
    {
        onKeyPressed?.Invoke();
    }
}

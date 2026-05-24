using UnityEngine;
using UnityEngine.UIElements;
using System;

public static class VisualElementExtensions
{
    public static void MakeInteractiveButton(this VisualElement element, Action onClickAction)
    {
        element.focusable = true;
        element.AddManipulator(new Clickable(() => onClickAction?.Invoke()));
        element.RegisterCallback<KeyDownEvent>(ev => {
            if (ev.keyCode == KeyCode.Return || ev.keyCode == KeyCode.Space)
            {
                onClickAction?.Invoke();
                ev.StopPropagation();
            }
        });
    }
}

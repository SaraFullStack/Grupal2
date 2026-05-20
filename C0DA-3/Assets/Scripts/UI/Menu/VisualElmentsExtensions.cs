using UnityEngine;
using UnityEngine.UIElements;
using System;

public static class VisualElementExtensions
{
    // Método para hacer cualquier elemento clicable con ratón y teclado
    public static void MakeInteractiveButton(this VisualElement element, Action onClickAction)
    {
        element.focusable = true;

        // 1. Clonar comportamiento de clic de ratón
        element.AddManipulator(new Clickable(() => onClickAction?.Invoke()));

        // 2. Clonar comportamiento de teclado (Intro / Espacio)
        element.RegisterCallback<KeyDownEvent>(ev => {
            if (ev.keyCode == KeyCode.Return || ev.keyCode == KeyCode.Space)
            {
                onClickAction?.Invoke();
                ev.StopPropagation();
            }
        });
    }
}

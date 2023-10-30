using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualElementFill : VisualElement
{
    public VisualElementFill()
    {
        this.style.width = Length.Percent(100);
        this.style.height = Length.Percent(100);
        this.style.alignItems = Align.Center;
        this.style.justifyContent = Justify.Center;
    }
}

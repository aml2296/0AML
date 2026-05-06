using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugComentVE : VisualElement
{
    public DebugComentVE()
        : base() {
        this.AddToClassList("Comment");
    }
}
public class DebugErrorVE : DebugComentVE
{
    public DebugErrorVE()
        : base()
    {
        this.AddToClassList("Error");
    }
}
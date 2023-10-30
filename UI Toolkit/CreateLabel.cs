using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[ExecuteAlways]
public class CreateLabel : MonoBehaviour
{
    [SerializeField] protected UIDocument document;
    [SerializeField] public string text;
    protected string oldText;
    protected VisualElement root;
    protected VisualElement labelBackground;
    protected Label label;

    void OnEnable()
    {
        root = document.rootVisualElement;
        label = root.Q<Label>("Text");
        labelBackground = root.Q<VisualElement>("TextBackground");
        label.text = text;
        oldText = text;
    }
    void Update()
    {
        UpdateLabel();
    }

    protected void UpdateLabel()
    {
        if (oldText != text)
        {
            BuildText();
        }
    }

    protected void BuildText()
    {
        oldText = text;
        label.text = oldText;

        root.Clear();
        labelBackground.Clear();
        VisualElementFill fill = new();
        labelBackground.Add(label);
        fill.Add(labelBackground);
        root.Add(fill);
    }
}

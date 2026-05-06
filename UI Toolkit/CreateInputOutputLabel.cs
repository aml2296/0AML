using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]
public class CreateInputOutputLabel : CreateLabel
{
    [SerializeField] bool changeColors = false;
    [SerializeField] Color giveColor;
    Color oldGiveColor;
    [SerializeField] Color takeColor;
    Color oldTakeColor;

    [SerializeField] string giveText;
    string oldGiveText;
    [SerializeField] string takeText;
    string oldTakeText;
    
    VisualElement GiveLabelBackground;
    Label GiveLabel;
    VisualElement TakeLabelBackground;
    Label TakeLabel;
    public void OnEnable()
    {
        root = document.rootVisualElement;
        label = root.Q<Label>("MainText");
        labelBackground = root.Q<VisualElement>("TextBackground");
        TakeLabel = root.Q<Label>("TakeText");
        TakeLabelBackground = root.Q<VisualElement>("TakeBackground");
        GiveLabel = root.Q<Label>("GiveText");
        GiveLabelBackground = root.Q<VisualElement>("GiveBackground");

        label.text = text;
        GiveLabel.text = giveText;
        TakeLabel.text = takeText;

        if(changeColors)
        {
            TakeLabelBackground.style.backgroundColor = takeColor;
            GiveLabelBackground.style.backgroundColor = giveColor;
        }

        ResetValues();
    }
    public void Update()
    {
        UpdateLabel();
    }
    public new void UpdateLabel()
    {
        if (giveColor != oldGiveColor || takeColor != oldTakeColor || giveText != oldGiveText || takeText != oldTakeText || text != oldText)
        {
            ResetValues();
            root.Clear();
            labelBackground.Clear();
            GiveLabelBackground.Clear();
            TakeLabelBackground.Clear();

            VisualElementFill fill = new();
            fill.style.flexDirection = FlexDirection.Row;

            label.text = oldText;
            GiveLabel.text = giveText;
            TakeLabel.text = takeText;

            GiveLabelBackground.style.marginTop = labelBackground.style.height;
            TakeLabelBackground.style.marginTop = labelBackground.style.height;

            if (changeColors)
            {
                TakeLabelBackground.style.backgroundColor = takeColor;
                GiveLabelBackground.style.backgroundColor = giveColor;
            }

            labelBackground.Add(label);
            GiveLabelBackground.Add(GiveLabel);
            TakeLabelBackground.Add(TakeLabel);

            fill.Add(TakeLabelBackground);
            fill.Add(labelBackground);
            fill.Add(GiveLabelBackground);
            root.Add(fill);
        }
    }

    private void ResetValues()
    {
        oldText = text;
        oldGiveColor = giveColor;
        oldTakeColor = takeColor;
        oldTakeText = takeText;
        oldGiveText = giveText;
    }
}
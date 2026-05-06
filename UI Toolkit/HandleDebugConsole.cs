using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HandleDebugConsole : MonoBehaviour
{
    [SerializeField] UIDocument consoleUI;
    [SerializeField] ActivateOnInput inputlistener;

    List<string> output = new List<string>();
    List<int> outputIDs = new List<int>(); //0 is comment, 1 is error, can be customized to add more styles with 2+
    public void OnEnable()
    {
        if (inputlistener.Listening == false)
            inputlistener.Listening = true;
        TestInput();
        StartCoroutine(Generate());
    }

    private void TestInput()
    {
        output.Clear();
        outputIDs.Clear();
        for (int i = 0; i < 10; i++)
        {
            output.Add(i.ToString());
            outputIDs.Add(UnityEngine.Random.Range((int)0, (int)3));
        }
    }

    public void Input(string text, int id = 0)
    {
        output.Add(text);
        outputIDs.Add(id);
    }
    public IEnumerator Generate()
    {
        ScrollView scrollElement = consoleUI.rootVisualElement.Q<ScrollView>("ConsoleWindow");
        scrollElement.Clear();
        for (int i = 0; i < output.Count; i++)
        {
            Label outputValue;
            if (outputIDs[i] == 0)
            {
                outputValue = CreateComment(output[i]);
            }
            else if (outputIDs[i] == 1)
            {
                outputValue = CreateError(output[i]);
            }
            else
                outputValue = HandleOutputID(outputIDs[i]);
            scrollElement.Add(outputValue);
        }
        yield return null;
    }

    private Label CreateComment(string v)
    {
        var el = new DebugComentVE();
        var classList = el.GetClasses();
        var comment = new Label();
        foreach (var c in classList)
            comment.AddToClassList(c);
        comment.text = v;
        return comment;
    }

    private Label CreateError(string v)
    {
        var el = new DebugErrorVE();
        var classList = el.GetClasses();
        var error = new Label();
        foreach (var c in classList)
            error.AddToClassList(c);
        error.text = v;
        return error;
    }

    private Label HandleOutputID(int v) =>
         CreateError("No Method to Handle Output outside of 0 and 1. [ID: " + v + "]");
}

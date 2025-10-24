using UnityEngine;
using TMPro;
using System;

public class TextChanger : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public String Text;

    void OnEnable()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        textMeshPro.text = Text;
    }
}
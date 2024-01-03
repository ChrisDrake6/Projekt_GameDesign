using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class TooltipSizer : MonoBehaviour
{
    public TextMeshProUGUI headerField;

    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    private void Update()
    {
        int headerlength = headerField.text.Length;
        int contentlength = contentField.text.Length;

        layoutElement.enabled = (headerlength > characterWrapLimit || contentlength > characterWrapLimit) ? true : false;

    }

}

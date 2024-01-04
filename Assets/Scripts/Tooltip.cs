using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;

    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header="")
    {
        //header active y/n? if n -> set header inactive

        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        //check the text and resize/turn the layout element on/off (only needs to change when text is set intitially)
        int headerlength = headerField.text.Length;
        int contentlength = contentField.text.Length;

        layoutElement.enabled = (headerlength > characterWrapLimit || contentlength > characterWrapLimit) ? true : false;
    }


    private void Update()
    {
        //only preview in real time in edit mode
        if (Application.isEditor)
        {
            //check the text and resize/turn the layout element on/off
            int headerlength = headerField.text.Length;
            int contentlength = contentField.text.Length;

            layoutElement.enabled = (headerlength > characterWrapLimit || contentlength > characterWrapLimit) ? true : false;
        }

        //follows mouse
        Vector2 position=Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;


    }

}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerSpeechBubble : MonoBehaviour
{
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;
    public GameObject player;
    public float displayDuration = 5;

    public float offsetY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Activate()
    {
        string nextLine = PopupTextSystem.Instance.GetNextLine();

        SetText(nextLine);
    }

    void SetText(string text)
    {
        contentField.text = text;

        //check the text and resize/turn the layout element on/off (only needs to change when text is set intitially)
        int contentlength = contentField.text.Length;

        layoutElement.enabled = contentlength > characterWrapLimit ? true : false;

        Invoke("NextTextOrCancel", displayDuration);
    }


    private void Update()
    {
        //only preview in real time in edit mode
        if (Application.isEditor)
        {
            //check the text and resize/turn the layout element on/off
            int contentlength = contentField.text.Length;

            layoutElement.enabled = contentlength > characterWrapLimit ? true : false;
        }

        // Follow Player
        float playerSize = player.GetComponent<SpriteRenderer>().bounds.size.y;

        Vector3 position = Camera.main.WorldToViewportPoint(
            new Vector3(player.transform.position.x, player.transform.position.y + playerSize + offsetY, 0));
        position.Scale(new Vector3(Screen.width, Screen.height, 0));
        position.y += rectTransform.rect.height / 2;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextTextOrCancel();
        }
    }

    private void NextTextOrCancel()
    {
        CancelInvoke();

        string nextLine = PopupTextSystem.Instance.GetNextLine();

        if (nextLine != null)
        {
            SetText(nextLine);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PopupTextSystem : MonoBehaviour
{
    public static PopupTextSystem Instance { get; private set; }
    public Tooltip tooltip;
    public PlayerSpeechBubble playerSpeechBubble;

    int currentLine = 0;
    List<string> textList = new List<string>();

    public void Awake()
    {
        Instance = this;
    }

    //need content to show
    public void ShowToolTip(string content, string header = "")
    {
        tooltip.SetText(content, header);

        tooltip.gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        tooltip.gameObject.SetActive(false);
    }

    public void AddPlayerText(params string[] lines)
    {
        if (lines.Length > 0)
        {
            textList.AddRange(lines);
            playerSpeechBubble.gameObject.SetActive(true);
            playerSpeechBubble.Activate();
        }
    }

    public string GetNextLine()
    {
        if (currentLine < textList.Count)
        {
            string nextLine = textList[currentLine];
            currentLine++;
            return nextLine;
        }
        else
        {
            currentLine = 0;
            textList.Clear();
            return null; 
        }
    }
}

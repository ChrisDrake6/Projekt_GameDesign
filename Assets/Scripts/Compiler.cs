using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Compiler : MonoBehaviour
{
    public static Compiler Instance { get; private set; }

    public Button button;
    public GraphicRaycaster graphicRaycaster;
    public PlayerStateManager player;

    PointerEventData pointer = new PointerEventData(EventSystem.current);
    List<RaycastResult> rayCastResults = new List<RaycastResult>();

    List<CodeBlock> codeBlocks = new List<CodeBlock>();
    int currentOrderIndex = 0;
    bool processRunning = false;
    float offsetDistance;

    public Compiler()
    {
        Instance = this;
    }

    private void Start()
    {
        offsetDistance = GetComponent<RectTransform>().sizeDelta.y * transform.localScale.y;
    }
    void Update()
    {
        if (!processRunning)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void StartCompiling()
    {
        processRunning = true;

        // Get all Codeblocks
        // TODO: Get CodeBlocks from IFs and Loops
        CodeBlock nextCodeBlocK = GetNextCodeBlock(transform.position);
                
        while (nextCodeBlocK != null)
        {
            codeBlocks.Add(nextCodeBlocK);
            nextCodeBlocK = GetNextCodeBlock(nextCodeBlocK.transform.position);
        }

        player.CallForNextOrder();
    }

    CodeBlock GetNextCodeBlock(Vector3 startingPosition)
    {
        rayCastResults.Clear();

        // calculate position of next possible runestone
        Vector3 targetPosition = startingPosition;
        targetPosition.y -= offsetDistance;

        // get next runestone
        pointer.position = targetPosition;
        graphicRaycaster.Raycast(pointer, rayCastResults);

        return rayCastResults.FirstOrDefault(result => result.gameObject.CompareTag("RuneStone")).gameObject?.GetComponent<CodeBlock>();
    }

    public CodeBlock GetNextOrder()
    {
        if (currentOrderIndex < codeBlocks.Count)
        {
            currentOrderIndex++;
            return codeBlocks[currentOrderIndex - 1];
        }
        else
        {
            processRunning = false;
            codeBlocks.Clear();
            currentOrderIndex = 0;
            return null;
        }
    }
}

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
    public bool processRunning = false;

    public Button compileButton;
    public Button clearButton;
    //public GraphicRaycaster graphicRaycaster;
    public PlayerStateManager player;
    public Transform codeBlocksParent;

    //PointerEventData pointer = new PointerEventData(EventSystem.current);
    //List<RaycastResult> rayCastResults = new List<RaycastResult>();

    List<CodeBlock> codeBlocks = new List<CodeBlock>();
    int currentOrderIndex = 0;
    //float offsetDistance;

    public Compiler()
    {
        Instance = this;
    }

    //private void Start()
    //{
    //    offsetDistance = codeBlocksParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y * transform.localScale.y;
    //}

    void Update()
    {
        if (!processRunning && !GameManager.Instance.progressing)
        {
            compileButton.interactable = true;
            clearButton.interactable = true;
        }
        else
        {
            compileButton.interactable = false;
            clearButton.interactable = false;
        }
    }

    public void StartCompiling()
    {
        processRunning = true;

        // Get all Codeblocks
        // TODO: Get CodeBlocks from IFs and Loops
        foreach (Transform child in codeBlocksParent)
        {
            if(child.CompareTag("RuneStone"))
            {
                codeBlocks.Add(child.GetComponent<CodeBlock>());
            }
        }

        //CodeBlock nextCodeBlocK = GetNextCodeBlock(transform.position);
                
        //while (nextCodeBlocK != null)
        //{
        //    codeBlocks.Add(nextCodeBlocK);
        //    nextCodeBlocK = GetNextCodeBlock(nextCodeBlocK.transform.position);
        //}

        player.CallForNextOrder();
    }

    // TODO: Make this work? Does not work with scrollarea
    //CodeBlock GetNextCodeBlock(Vector3 startingPosition)
    //{
    //    rayCastResults.Clear();

    //    // calculate position of next possible runestone
    //    Vector3 targetPosition = startingPosition;
    //    targetPosition.y -= offsetDistance;

    //    // get next runestone
    //    pointer.position = targetPosition;
    //    graphicRaycaster.Raycast(pointer, rayCastResults);

    //    return rayCastResults.FirstOrDefault(result => result.gameObject.CompareTag("RuneStone")).gameObject?.GetComponent<CodeBlock>();
    //}

    public CodeBlock GetNextOrder()
    {
        if (currentOrderIndex < codeBlocks.Count)
        {
            currentOrderIndex++;
            return codeBlocks[currentOrderIndex - 1];
        }
        else
        {
            GameManager.Instance.ValidateUserInput(false, "I did not make it to the door.");
            codeBlocks.Clear();
            currentOrderIndex = 0;
            processRunning = false;
            return null;
        }
    }
}

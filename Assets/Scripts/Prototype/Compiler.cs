using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Compiler : MonoBehaviour, IDropHandler
{
    public GameObject player;

    Player playerScript;
    List<CodeBlock> waitingRoom;
    bool processRunning = false;
    int currentStepIndex = 0;
    Button button;

    void Start()
    {
        playerScript = player.GetComponent<Player>();
        button = transform.GetChild(0).GetComponent<Button>();
    }

    void Update()
    {
        if (processRunning)
        {
            if (playerScript.StepComplete)
            {
                CompileNextStep();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ToDo: order children from top to bottom
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform, true);
        }
    }

    public void StartCompiling()
    {
        button.interactable = false;
        processRunning = true;
        currentStepIndex = 0;
        waitingRoom = new List<CodeBlock>();

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CodeBlock>() != null)
            {
                waitingRoom.Add(child.gameObject.GetComponent<CodeBlock>());
            }
        }
    }

    void CompileNextStep()
    {
        if (waitingRoom.Count == currentStepIndex)
        {
            processRunning = false;
            button.interactable = true;
        }
        else
        {
            switch (waitingRoom[currentStepIndex])
            {
                case CodeBlock_Statement cs:
                    playerScript.StepComplete = false;
                    if (cs is CodeBlock_Statement_Move csm)
                    {
                        playerScript.Move(csm.Direction);
                    }
                    break;

                // TODO: Insert ifs and loops here

                default: break;
            }
            currentStepIndex++;
        }
    }
}

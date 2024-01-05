using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<LevelStage> stages = new List<LevelStage>();
    public PlayerStateManager player;
    public Transform runeStoneContainer;

    public Tilemap pathMap;

    bool success = false;

    // Public for testing purposes
    public int currentStage = 0;

    public GameManager()
    {
        Instance = this;
    }

    private void Start()
    {
        ReLoadStage(currentStage);
    }

    public void ReLoadStage(int index)
    {
        // Reset player postion to starting point
        player.SetPlayerPosition(stages[currentStage].PlayerStartPosition);
        LoadStage(index);
    }
    public void LoadStage(int index)
    {
        success = false;

        // Load CodeBlocks from Stage
        foreach (Transform child in runeStoneContainer.transform)
        {
            if (child.gameObject.CompareTag("RuneStone"))
            {
                Destroy(child.gameObject);
            }
        }
        foreach (CodeBlock codeBlock in stages[currentStage].availableCodeBlocks)
        {
            Instantiate(codeBlock, runeStoneContainer);
        }

        CameraController.instance.SetTargetPosition(stages[currentStage].cameraPosition, stages[currentStage].cameraSize);

        // Clear RuneStones
        RuneStoneManager.Instance.Clear();
    }

    public void CheckForWin()
    {
        if (success)
        {
            currentStage++;
            if (currentStage < stages.Count)
            {

                // Let player progress to next starting point
                player.ProgressToNextStage(stages[currentStage].PlayerStartPosition);

                LoadStage(currentStage);
            }
            else
            {
                // All Stages Completed
                Debug.Log("Victory!");

                // Load next Level
            }
        }
        else
        {
            Debug.Log("Stage failed");
            ReLoadStage(currentStage);
        }
    }

    public void SetWinConditionAchieved()
    {
        success = true;
    }
}

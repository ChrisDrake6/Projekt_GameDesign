using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<LevelStage> stages = new List<LevelStage>();
    public PlayerStateManager player;
    public Transform runeStoneContainer;
    public GameObject pauseMenu;
    public Tilemap pathMap;

    // Public for testing purposes
    public int currentStage = 0;
    public bool progressing = false;

    public GameManager()
    {
        Instance = this;
    }

    private void Start()
    {
        ReLoadStage(currentStage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }

    public void ReLoadStage(int index)
    {
        // Reset player postion to starting point
        player.SetPlayerPosition(stages[currentStage].PlayerStartPosition);
        LoadStage(index);
    }
    public void LoadStage(int index)
    {
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
        Compiler.Instance.processRunning = false;
    }

    public void ValidateUserInput(bool success)
    {
        if (!progressing)
        {
            if (success)
            {
                currentStage++;
                if (currentStage < stages.Count)
                {

                    // Let player progress to next starting point
                    player.ProgressToNextStage(stages[currentStage].PlayerStartPosition);

                    // Clear RuneStones
                    RuneStoneManager.Instance.Clear();

                    LoadStage(currentStage);
                }
                else
                {
                    // All Stages Completed
                    Debug.Log("Victory!");

                    // Load next Level
                }
                progressing = true;
            }
            else
            {
                Debug.Log("Stage failed");
                ReLoadStage(currentStage);
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;
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

    void Start()
    {
        ReLoadStage(currentStage);
        LoadIntroText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }

    void ReLoadStage(int index)
    {
        // Reset player postion to starting point
        player.SetPlayerPosition(stages[currentStage].PlayerStartPosition);
        LoadStage(index);
    }
    void LoadStage(int index)
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

    void LoadIntroText()
    {
        string path = Application.dataPath + "/IntroTextLines/" + stages[currentStage].pathToIntroTextLines;
        string introTextLinesCompressed = File.ReadAllText(path);
        string[] introTextLines = introTextLinesCompressed.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        PopupTextSystem.Instance.AddPlayerText(introTextLines);
    }

    public void ValidateUserInput(bool success, string message)
    {
        if (!progressing)
        {
            PopupTextSystem popupTextSystem = PopupTextSystem.Instance;
            if (success)
            {
                currentStage++;
                if (currentStage < stages.Count)
                {
                    //popupTextSystem.AddPlayerText("Yeay! I made it to the Door!");

                    // Let player progress to next starting point
                    player.ProgressToNextStage(stages[currentStage].PlayerStartPosition);

                    // Clear RuneStones
                    RuneStoneManager.Instance.Clear();

                    LoadStage(currentStage);
                    LoadIntroText();
                }
                else
                {
                    // All Stages Completed
                    Debug.Log("Victory!");
                    popupTextSystem.AddPlayerText("Awesome! You have guided me to the end!");

                    // Load next Level
                }
                progressing = true;
            }
            else
            {
                Debug.Log("Stage failed");
                popupTextSystem.AddPlayerText(message);
                ReLoadStage(currentStage);
            }
        }
    }
}

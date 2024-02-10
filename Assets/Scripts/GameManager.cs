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
    private Animator animator;

    // Public for testing purposes
    public int currentStage = 0;
    public bool progressing = false;

    public GameManager()
    {
        Instance = this;
    }

    void Start()
    {
        SetIntroTextLines();
        ReLoadStage(currentStage);
        LoadIntroText();
        animator = player.GetComponent<Animator>();
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
        PopupTextSystem.Instance.AddPlayerText(stages[currentStage].introTextLines);
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
                    SoundManager.Instance.PlaySound(SoundManager.Instance.winsound);
                    popupTextSystem.AddPlayerText("Awesome! You have guided me to the end!");
                    animator.SetBool("wasSuccessful", true);

                    // Load next Level
                }
                progressing = true;
            }
            else
            {
                Debug.Log("Stage failed");
                SoundManager.Instance.PlaySound(SoundManager.Instance.failuresound);
                popupTextSystem.AddPlayerText(message);
                ReLoadStage(currentStage);
            }
        }
    }

    /// <summary>
    /// TODO: Move those somewhere else.
    /// Moving those into a file didnt work, so i put them here. 
    /// </summary>
    void SetIntroTextLines()
    {
        stages[0].introTextLines = new string[] {
            "Ouch, My Main Processing Unit hurts!",
            "Where am I?\nWho am I?\nAnd who are you?",
            "Hm. My Hard Drive says that my name is Cody.\nAnd you must be the player.",
            "Wherever I am, I need your help to get out of here.\nOh, don't worry, the difficulty parameters for this task are relatively low.",
            "I can only do something if my player gives me a script.",
            "You will need administration access to my pool of rune stones.\nYou will find those at the top of your screen.",
            "Now, each of those contains one statement. An order, if you will.",
            "To create a script, you must put those rune stones into the compilation list at the left side of the screen.",
            "You can do so by dragging them there or by using left shift + click on them.",
            "Also, you can move them around in the list and delete them by dragging them out or also using left shift + click.",
            "If you order them from top to bottom and click on the compile button,\nI will execute one statement at a time, from top to bottom.",
            "If you order them correctly, I may progress to the next stage. If not, I will have to start over.",
            "Oh, by the way, if I am talking too much, you have permission to skip my lines by pressing space bar.",
            "Now, if you would be so kind to help me through that door over there..."
        };
        stages[1].introTextLines = new string[] {
            "Ah. I forgot to mention that my movement functionality may be limited... for some... reason.",
            "Right now, I can only move until I hit the next wall or obstacle.",
            "I will try to regain my full arsenal as soon as possile. Thank you for your patience. :)"
        };
        stages[2].introTextLines = new string[] {
            "Oh dear. It seems, you have to navigate me through all that rubble."
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButton : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exiting Game");
    }
}

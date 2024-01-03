using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Specific scene will be loaded
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}


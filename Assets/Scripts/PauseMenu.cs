using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public static bool isGamePaused= false;

    [SerializeField] private GameObject thePauseMenu;


  public void PauseGame()
    {
        isGamePaused = true;
        thePauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        isGamePaused = false;
        thePauseMenu.SetActive(false);
    }
    public void RestartGame()
    {
        isGamePaused = false;
        SceneManager.LoadScene(2);
    }
}

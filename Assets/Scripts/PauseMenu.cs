using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pausePannel;
    public static bool isPaused = false;



    public void PauseGame()
    {
        isPaused = true;
        GameplayManager.Instance.isPaused = true;
        pausePannel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        GameplayManager.Instance.isPaused = false;
        pausePannel.SetActive(false);

    }

    public void RestartGame()
    {
        isPaused = false;
        GameplayManager.Instance.isPaused = false;
        GameManager.Instance.GoToGameplay();
    }

    public void ReturnToMainMenu()
    {
        isPaused = false;
        GameplayManager.Instance.isPaused = false;
        GameManager.Instance.GoToMainMenu();
    }
}

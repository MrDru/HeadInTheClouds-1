using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FixedMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
   public void GoToGameplay()
    {
        SceneManager.LoadScene(2);
    }
}

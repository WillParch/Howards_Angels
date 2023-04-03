using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

public void QuitGame ()
{
    Debug.Log("Quit!");
    Application.Quit();
}

    public void returnMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}


using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    string levelName;

    public void LoadLevel(string name)
    {
        //Debug.Log ("New Level load: " + name);
        SceneManager.LoadScene(name);
        this.levelName = name;

        if (name == "Game")
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

    }

    public void QuitRequest()
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        this.levelName = SceneManager.GetActiveScene().ToString();

        if (levelName == "Game")
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }
}

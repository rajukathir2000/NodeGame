using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonManager : MonoBehaviour
{
    public void LoadLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelNumber); // Store selected level
        SceneManager.LoadScene(1); // Load game scene
    }

    public void LoadHome()
    {
        //PlayerPrefs.SetInt("SelectedLevel", levelNumber); // Store selected level
        SceneManager.LoadScene(0); // Load game scene
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}

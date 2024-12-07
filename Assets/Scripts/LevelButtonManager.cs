using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonManager : MonoBehaviour
{
    public void LoadLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelNumber); // Store selected level
        SceneManager.LoadScene("GameScene"); // Load game scene
    }

    public void LoadHome(int levelNumber)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelNumber); // Store selected level
        SceneManager.LoadScene(0); // Load game scene
    }
}

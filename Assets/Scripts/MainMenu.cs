using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Toggle timerToggle;

    void Start()
    {
        timerToggle.isOn = PlayerPrefs.GetInt("ShowTimer", 1) == 1;
    }
    public void PlayGame()
    {
        // Stocke la valeur actuelle du timer avant de charger la scène
        PlayerPrefs.SetInt("ShowTimer", timerToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("bedroom");
    }
    
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

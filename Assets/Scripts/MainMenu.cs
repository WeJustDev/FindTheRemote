using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Toggle timerToggle;
    public Toggle proximityToggle;

    void Start()
    {
        timerToggle.isOn = PlayerPrefs.GetInt("ShowTimer", 1) == 1;
        proximityToggle.isOn = PlayerPrefs.GetInt("ShowProximity", 1) == 1;
    }
    public void PlayGame()
    {
        PlayerPrefs.SetInt("ShowTimer", timerToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("ShowProximity", proximityToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("bedroomTest");
    }
    
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

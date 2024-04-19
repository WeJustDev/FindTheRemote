using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public bool isActivated = false;

    public void ActivatePortal()
    {
        isActivated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        // Vérifier si l'objet qui est entré dans la sphère de détection est le joueur
        if (other.gameObject.tag == "Player" && isActivated)
        {
            // Charger la nouvelle scène
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
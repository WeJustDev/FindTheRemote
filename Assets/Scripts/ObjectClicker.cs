using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public ParticleSystem openAnimation;
    public ParticleSystem openAnimation2;
    public ParticleSystem openAnimation3; // Système de particules pour l'ouverture du portail
    public PortalTrigger portalTrigger; // Référence au script PortalTrigger
    public GameObject portalBg; // Objet à activer

    public void OnMouseDown()
    {
        // Activer l'animation d'ouverture du portail
        if (openAnimation && openAnimation2 && openAnimation3 != null)
        {
            openAnimation.gameObject.SetActive(true);
            openAnimation2.gameObject.SetActive(true);
            openAnimation3.gameObject.SetActive(true);

            openAnimation.Play();
            openAnimation2.Play();
            openAnimation3.Play();

            // Activer le portail
            if (portalTrigger != null)
            {
                portalTrigger.ActivatePortal();
            }
            else
            {
                Debug.LogWarning("PortalTrigger not assigned.");
            }

            // Activer l'objet
            if (portalBg != null)
            {
                portalBg.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Object to activate not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Open animation not assigned.");
        }
    }
}
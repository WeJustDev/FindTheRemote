using UnityEngine;
using UnityEngine.UI;

public class ProximityBar : MonoBehaviour
{
    [SerializeField] Slider proximitySlider; // Le Slider UI
    [SerializeField] Transform user; // L'utilisateur
    [SerializeField] Transform remote; // La télécommande

    // La distance maximale à laquelle la barre de proximité sera pleine
    [SerializeField] float maxDistance = 0.1f;

    void Update()
    {
        // Calculer la distance entre l'utilisateur et la télécommande
        float distance = Vector3.Distance(user.position, remote.position);

        // Mettre à jour la valeur du slider en fonction de la distance
        proximitySlider.value = 1 - (distance / maxDistance);
    }
}
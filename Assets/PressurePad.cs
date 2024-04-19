using UnityEngine;

public class PressurePad : MonoBehaviour
{
    public bool hasRedKey = false; // Indicateur pour savoir si la RedKeyCrystal est présente

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedKeyCrystal"))
        {
            hasRedKey = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RedKeyCrystal"))
        {
            hasRedKey = false;
        }
    }
}

using UnityEngine;

public class PressurePad3 : MonoBehaviour
{
    public bool hasBlueKey = false; // Indicateur pour savoir si la BlueKeyCrystal est présente

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueKeyCrystal"))
        {
            hasBlueKey = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlueKeyCrystal"))
        {
            hasBlueKey = false;
        }
    }
}

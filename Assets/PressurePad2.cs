using UnityEngine;

public class PressurePad2 : MonoBehaviour
{
    public bool hasGreenKey = false; // Indicateur pour savoir si la GreenKeyCrystal est présente

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreenKeyCrystal"))
        {
            hasGreenKey = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GreenKeyCrystal"))
        {
            hasGreenKey = false;
        }
    }
}

using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float sensitivity = 3.0f; // Sensibilité de la souris pour la rotation de la caméra
    public Transform playerBody; // Référence au corps du joueur pour la rotation horizontale

    float rotationX = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur au centre de l'écran
    }

    void Update()
    {
        // Rotation verticale de la caméra (regarder vers le haut et vers le bas)
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limite la rotation verticale pour éviter les retournements

        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f); // Applique la rotation à la caméra

        // Rotation horizontale du joueur (regarder à gauche et à droite)
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f; // Vitesse de déplacement normale
    private float originalSpeed; // Vitesse de déplacement normale sauvegardée
    public float jumpForce = 3.0f; // Force de saut
    private bool isJumping = false; // Indique si le joueur est en train de sauter

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    // Position et rotation initiales
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalSpeed = speed; // Sauvegarde de la vitesse de déplacement normale

        // Enregistrer la position et la rotation initiales
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Vérifier si le joueur est au sol
        if (controller.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Déplacements horizontaux (gauche/droite et avant/arrière)
            Vector3 move = transform.right * horizontal + transform.forward * vertical;

            // Appliquer la vitesse de déplacement
            moveDirection = move * speed;

            // Sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = originalSpeed * 2; // Multiplie simplement la vitesse par 2
            }
            else
            {
                speed = originalSpeed; // Rétablit la vitesse de déplacement normale
            }

            // Saut
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
            }
        }

        // Gestion du saut
        if (isJumping)
        {
            moveDirection.y = jumpForce; // Applique la force de saut
            isJumping = false; // Réinitialise l'état de saut
        }

        // Appliquer la gravité
        moveDirection.y -= 9.81f * Time.deltaTime;

        // Déplacer le joueur
        controller.Move(moveDirection * Time.deltaTime);
    }

    // Méthode pour restaurer la position et la rotation du joueur
    public void RestorePlayerPositionAndRotation()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}

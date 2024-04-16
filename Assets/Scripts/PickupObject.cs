using UnityEngine;

public class PickupObject : MonoBehaviour
{
    // Référence à la caméra du joueur
    public Camera playerCamera;

    // Distance à laquelle l'objet peut être ramassé
    public float pickupRange = 3f;

    // Vitesse à laquelle l'objet est ramassé
    public float pickupSpeed = 10f;

    public Transform wrist;

    // Indique si l'objet est actuellement ramassé
    private bool isPickedUp = false;

    // Référence à l'objet actuellement ramassé
    private Transform pickedUpObject;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Vérifie si le joueur appuie sur le bouton de la souris pour ramasser ou lâcher un objet
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPickedUp)
            {
                Pickup();
            }
            else
            {
                Drop();
            }
        }

        // Si un objet est ramassé, déplace-le vers la position de la caméra du joueur
        if (isPickedUp)
        {
            MovePickedUpObject();
        }

        // Si un objet est ramassé et que le joueur appuie sur le bouton droit de la souris, lâche l'objet
        if (Input.GetMouseButtonDown(1) && isPickedUp)
        {
            Drop();
        }
    }

    void Pickup()
    {
        // Lancer un rayon depuis la caméra du joueur pour détecter les objets à ramasser
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            // Récupérer la référence de l'objet ramassé
            Transform objectToPickup = hit.collider.transform;

            // Vérifier si l'objet détecté est ramassable
            if (objectToPickup.CompareTag("Ramassable"))
            {

                // Mettre à jour l'état du ramassage
                pickedUpObject = objectToPickup;
                isPickedUp = true;

                // Désactiver la gravité de l'objet ramassé
                pickedUpObject.GetComponent<Rigidbody>().useGravity = false;
            }
            // Vérifier si l'objet détecté est une télécommande
            else if (objectToPickup.CompareTag("Telecommande"))
            {
                animator.SetBool("Telecommande", true);
                pickedUpObject = objectToPickup;
                pickedUpObject.parent = wrist;
                Vector3 customPosition = new Vector3(0.155f, 0.027f, 0.09f); // Remplacez xValue, yValue, zValue par vos dimensions personnalisées
                pickedUpObject.localPosition = customPosition;
                Quaternion rotation = Quaternion.Euler(40f, 60f, 110f);
                pickedUpObject.localRotation = rotation;
                pickedUpObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }


    void MovePickedUpObject()
    {
        // Déplacer l'objet ramassé vers la position de la caméra du joueur avec une interpolation
        pickedUpObject.position = Vector3.Lerp(pickedUpObject.position, playerCamera.transform.position + playerCamera.transform.forward * pickupRange, pickupSpeed * Time.deltaTime);
    }

    void Drop()
    {
        // Réactiver la gravité de l'objet lâché
        pickedUpObject.GetComponent<Rigidbody>().useGravity = true;

        // Réinitialiser les variables de ramassage
        isPickedUp = false;
        pickedUpObject = null;
    }
}

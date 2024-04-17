using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour
{
    // Référence à la caméra du joueur
    public Camera playerCamera;

    // Distance à laquelle l'objet peut être ramassé
    public float pickupRange = 3f;

    public GameObject telecomande_bouton; // Référence à l'objet "telecomande_bouton"

    public GameObject telecomande_Led; // Référence à l'objet "telecommande"

    public Light World_Light;

    // Vitesse à laquelle l'objet est ramassé
    public float pickupSpeed = 10f;

    public Transform wrist;

    public GameObject pickupUI;

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
        if (Input.GetKeyDown(KeyCode.E))
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

        // Si un objet est ramassé et que le joueur appuie sur la touche "E", passe l'animation TelecommandePush à true
        if (Input.GetMouseButtonDown(0) && pickedUpObject != null && pickedUpObject.CompareTag("Telecommande"))
        {
            animator.SetBool("TelecommandePush", true);
            StartCoroutine(AnimateButton());

            // Si l'objet ramassé est la télécommande et que le bouton gauche de la souris est cliqué, active les animations et le passage du portail
            ClickableObject clickableObject = pickedUpObject.GetComponent<ClickableObject>();
            if (clickableObject != null)
            {
                clickableObject.OnMouseDown();
            }
        }
        else
        {
            animator.SetBool("TelecommandePush", false);
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Ramassable"))
            {
                pickupUI.SetActive(true);
            }
            else
            {
                pickupUI.SetActive(false);
            }
        }
        else
        {
            pickupUI.SetActive(false);
        }
        
    }

    IEnumerator AnimateButton()
    {
        yield return new WaitForSeconds(0.95f);
        // Réduire l'échelle du bouton à 0.23
        Vector3 targetScale = new Vector3(0.792043f, 0.3946826f, 0.23f);
        telecomande_bouton.transform.localScale = targetScale;

        yield return new WaitForSeconds(1f); // Attendre pendant 0.5 secondes

        Light telecommandeLed = telecomande_Led.GetComponent<Light>();
        telecommandeLed.intensity = 0f;
        yield return new WaitForSeconds(0.5f); // Attendre pendant 0.5 secondes
        telecommandeLed.intensity = 30f;
        yield return new WaitForSeconds(0.5f); // Attendre pendant 0.5 secondes
        telecommandeLed.intensity = 0f;
        yield return new WaitForSeconds(0.5f); // Attendre pendant 0.5 secondes
        telecommandeLed.intensity = 30f;
        yield return new WaitForSeconds(0.5f); // Attendre pendant 0.5 secondes
        telecommandeLed.intensity = 0f;
        yield return new WaitForSeconds(0.5f); // Attendre pendant 0.5 secondes
        telecommandeLed.intensity = 30f;
        yield return new WaitForSeconds(1f); // Attendre pendant 0.5 secondes
        
        if (World_Light != null)
        {
            // Augmenter progressivement l'intensité de la lumière
            float targetIntensity = 1000f;
            float duration = 1f; // Durée de l'animation en secondes
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;
                World_Light.intensity = Mathf.Lerp(0f, targetIntensity, progress);
                yield return null;
            }
            
            // Assurez-vous que l'intensité atteint exactement la valeur cible
            World_Light.intensity = targetIntensity;
        }

        yield return new WaitForSeconds(8f);

        // Remettre l'échelle du bouton à sa taille d'origine
        targetScale = new Vector3(0.792043f, 0.3946826f, 0.4297258f);
        telecomande_bouton.transform.localScale = targetScale;

        if (World_Light != null)
        {
            World_Light.intensity = 0f;
        }
    }

    void Pickup()
    {
        animator.SetBool("TelecommandePush", false);
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
                Quaternion rotation = Quaternion.Euler(34.9f, -54.8f, -121f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Vector3 MoveDir;
    private Vector2 inputVector;
    public CharacterController characterController;
    Vector3 PlayerVelocity;
    float PlayerSpeed = 5;
    float Gravity = 9.81f;
    private Animator animator;
    private float jumpForce = 6.0f;
    private bool isJumping = false;
    public Camera playerCamera;
    public float pickupRange = 3f;
    public GameObject telecomande_bouton;
    public GameObject telecomande_Led;
    public Light World_Light;
    public float pickupSpeed = 10f;
    public Transform wrist;
    public GameObject pickupUI;
    private bool isPickedUp = false;
    private Transform pickedUpObject;

    // Start is called before the first frame update
    void Start()
    {
        MoveDir = Vector3.zero;
        animator = GetComponent<Animator>();
    }

    public void OnPickup()
    {
        Debug.Log("Pickup");
        if (!isPickedUp)
        {
            Pickup();
        }
        else
        {
            Drop();
        }
    
        if (isPickedUp)
        {
            MovePickedUpObject();
        }
    }
    
    void Pickup()
    {
        animator.SetBool("TelecommandePush", false);
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            Transform objectToPickup = hit.collider.transform;
            if (objectToPickup.CompareTag("Ramassable"))
            {
                pickedUpObject = objectToPickup;
                isPickedUp = true;
                pickedUpObject.GetComponent<Rigidbody>().useGravity = false;
            }
            else if (objectToPickup.CompareTag("Telecommande"))
            {
                animator.SetBool("Telecommande", true);
                pickedUpObject = objectToPickup;
                pickedUpObject.parent = wrist;
                Vector3 customPosition = new Vector3(0.155f, 0.027f, 0.09f);
                pickedUpObject.localPosition = customPosition;
                Quaternion rotation = Quaternion.Euler(34.9f, -54.8f, -121f);
                pickedUpObject.localRotation = rotation;
                pickedUpObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }
    
    void MovePickedUpObject()
    {
        pickedUpObject.position = Vector3.Lerp(pickedUpObject.position, playerCamera.transform.position + playerCamera.transform.forward * pickupRange, pickupSpeed * Time.deltaTime);
    }
    
    void Drop()
    {
        pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
        isPickedUp = false;
        pickedUpObject = null;
    }
    
    IEnumerator AnimateButton()
    {
        yield return new WaitForSeconds(0.95f);
        Vector3 targetScale = new Vector3(0.792043f, 0.3946826f, 0.23f);
        telecomande_bouton.transform.localScale = targetScale;
        yield return new WaitForSeconds(1f);
        Light telecommandeLed = telecomande_Led.GetComponent<Light>();
        telecommandeLed.intensity = 0f;
        yield return new WaitForSeconds(0.5f);
        telecommandeLed.intensity = 30f;
        yield return new WaitForSeconds(0.5f);
        telecommandeLed.intensity = 0f;
        yield return new WaitForSeconds(0.5f);
        telecommandeLed.intensity = 30f;
        yield return new WaitForSeconds(0.5f);
        telecommandeLed.intensity = 0f;
        yield return new WaitForSeconds(0.5f);
        telecommandeLed.intensity = 30f;
        yield return new WaitForSeconds(1f);
        if (World_Light != null)
        {
            float targetIntensity = 1000f;
            float duration = 1f;
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;
                World_Light.intensity = Mathf.Lerp(0f, targetIntensity, progress);
                yield return null;
            }
            World_Light.intensity = targetIntensity;
        }
        yield return new WaitForSeconds(8f);
        targetScale = new Vector3(0.792043f, 0.3946826f, 0.4297258f);
        telecomande_bouton.transform.localScale = targetScale;
        if (World_Light != null)
        {
            World_Light.intensity = 0f;
        }
    }

    public void OnMove(InputAction.CallbackContext ctxt) {
        inputVector = ctxt.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext ctxt) {
        if(ctxt.performed) {
            PlayerSpeed = PlayerSpeed * 2;
            animator.SetBool("isSprint", true);
        }
        if(ctxt.canceled) {
            PlayerSpeed = 5;
            animator.SetBool("isSprint", false);
        }
    }

    // Update is called once per frame
    void Update()
    {       
        if(characterController.isGrounded){
            // Saut
            if (Input.GetButtonDown("Jump")) // Ne permet pas de sauter quand accroupi
            {
                PlayerVelocity.y = jumpForce; // Applique la force de saut
                isJumping = true; // Le joueur est en train de sauter
                animator.SetBool("isJumping", true);
            }
        } else {
            // Le joueur n'est pas au sol, il ne peut pas être en train de sauter
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        // affichage de l'ui de ramassage
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

        if(isPickedUp)
        {
            MovePickedUpObject();
        }
    
        // Appliquer la gravité
        PlayerVelocity.y -= Gravity * Time.deltaTime;
    
        Vector3 NewMoveDir = new Vector3(inputVector.x, 0, inputVector.y); // Ajoute PlayerVelocity.y à MoveDir
        NewMoveDir = Camera.main.transform.TransformDirection(NewMoveDir);
        NewMoveDir.y = 0;
        NewMoveDir.Normalize();
        MoveDir.x = NewMoveDir.x;
        MoveDir.z = NewMoveDir.z;
    
        animator.SetBool("isWalking", inputVector.y > 0);
        animator.SetBool("Backward", inputVector.y < 0);
        animator.SetBool("StrafeLeft", inputVector.x < 0);
        animator.SetBool("StrafeRight", inputVector.x > 0);
    
        characterController.Move(MoveDir * PlayerSpeed*Time.deltaTime);
    
        // Déplacer le joueur en fonction de la gravité et de la force de saut
        characterController.Move(PlayerVelocity * Time.deltaTime);
    
        if(characterController.isGrounded) {
            Debug.Log("Grounded");
        } else {
            Debug.Log("Not Grounded");
        }
    }
}
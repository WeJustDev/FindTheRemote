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

    // Start is called before the first frame update
    void Start()
    {
        MoveDir = Vector3.zero;
        animator = GetComponent<Animator>();
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
using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    public Animator animator;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool shift_Input;
    public bool jump_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool comboFlag;
    public bool animation1InProgress = false;
    public bool animation2InProgress = false;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        animator = GetComponent<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
    }



    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Shift.performed += i => shift_Input = true;
            playerControls.PlayerActions.Shift.canceled += i => shift_Input = false; //sprinting stops when stopped being held
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.RB.performed += i => rb_Input = true;
            playerControls.PlayerActions.RT.performed += i => rt_Input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleAttackInput();
        //HandleActionInput
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); //always positive because run animation only registers positive values for now
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (shift_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleAttackInput()
    {
        if (rb_Input)
        {

            if (playerManager.canDoCombo)
            {
                if (animation2InProgress || playerManager.isInteracting)
                {
                    return;  // Return early if animation is already playing or interacting
                }

                animation2InProgress = true;
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }

            else
            {
                if (animation1InProgress || playerManager.isInteracting)
                {
                    return;  // Return early if animation is already playing or interacting
                }

                playerManager.isInteracting = true;  // Set isInteracting to true at the beginning
                animation1InProgress = true;  // Set animationInProgress to true

                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                if (playerManager.canDoCombo)
                {
                    animation1InProgress = false;
                    return;
                }
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }

            rb_Input = false;
        }

        if (rt_Input)
        {
            rt_Input = false;
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

}

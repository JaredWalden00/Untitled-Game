using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    Animator animator;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;
    PlayerAttacker playerAttacker;
    EnemyStats enemyStats;

    public List<ItemList> items = new List<ItemList>();

    public bool isInteracting;
    public bool canDoCombo;

    private void Start()
    {
        StartCoroutine(CallItemUpdate());
    }

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        canDoCombo = animator.GetBool("canDoCombo");
    }

    public IEnumerator CallItemUpdate()
    {
        while (true) // Infinite loop for continuous updates
        {
            foreach (ItemList i in items)
            {
                i.item.Update(playerStats, i.stacks);
            }

            yield return null; // Yield to the next frame immediately
        }
    }

    public void CallItemOnHit(EnemyStats enemy)
    {
        foreach (ItemList i in items)
        {
            i.item.OnHit(playerAttacker ,enemy, i.stacks);
        }

    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}

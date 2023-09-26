using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorManager animtorManager;
    InputManager inputManager;
    public string lastAttack;

        private void Awake()
        {
            animtorManager = GetComponentInChildren<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputManager.comboFlag)
        {
            animtorManager.animator.SetBool("canDoCombo", false);
            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animtorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true); //needs to be second attack
            }
        }    
    }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animtorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animtorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;
        }
    }
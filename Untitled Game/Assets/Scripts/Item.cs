using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string GiveName();
    public virtual void Update (PlayerStats player, int stacks)
    {

    }
    public virtual void OnHit(PlayerAttacker player, EnemyStats enemy, int stacks)
    {

    }
}

public class HealingItem : Item
{
    GameObject effect;
    GameObject healingInstance; // Store a reference to the instantiated healing GameObject
    Vector3 middleOffset = new Vector3(0f, 0.5f, 0f); // Adjust the offset as needed

    public override string GiveName()
    {
        return "Healing Name";
    }

    public override void Update(PlayerStats player, int stacks)
    {
        if (effect == null)
        {
            effect = (GameObject)Resources.Load("Item Effects/Healing", typeof(GameObject));
            // Instantiate the effect GameObject when effect is loaded
            healingInstance = GameObject.Instantiate(effect, player.transform.position + middleOffset, Quaternion.identity);
        }

        // Update the position of the healing GameObject to follow the player
        if (healingInstance != null)
        {
            healingInstance.transform.position = player.transform.position + middleOffset;
        }

        player.maxHealth += 3 + (2 + stacks);
    }
}



public class FireDamageItem : Item
{
    public override string GiveName()
    {
        return "Fire Damage Item";
    }
    public override void OnHit(PlayerAttacker player, EnemyStats enemy, int stacks)
    {
        enemy.currentHealth -= 10 * stacks;
    }
}

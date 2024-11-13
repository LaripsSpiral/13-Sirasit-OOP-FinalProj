using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBait : MonoBehaviour, IDealDamageAble
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.gameObject.TryGetComponent(out Player player))
            return;

        DealDamage(player, 1);
    }

    public void DealDamage(Player player, int damage)
    {
        player.TakeDamage(damage);
    }

}

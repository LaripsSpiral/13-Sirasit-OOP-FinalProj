using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFish : Character, IEatable
{
    public Vector3 LastVelocity;

    private float _foodAmount = 2.5f;
    public float FoodAmount => _foodAmount;

    private void Start()
    {
        moveDir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        Move(moveDir, ForceMode2D.Impulse, 10);
    }

    private void Update()
    {
        LastVelocity = Rb2d.velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.TryGetComponent(out Player player))
            return;

        EatenBy(player, FoodAmount);
    }

    public void EatenBy(Player player, float foodAmount)
    {
        Debug.Log($"{this} eaten by {player}");

        player.IncreaseFoodValue(foodAmount);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFish : Character, IEatable
{
    public Vector3 LastVelocity;

    private readonly float _foodAmount = 2.5f;

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

        EatenBy(player);
    }

    public void EatenBy(Player player)
    {
        Debug.Log($"{this} eaten by {player}");

        player.IncreaseFoodValue(_foodAmount);
        Destroy(gameObject);
    }
}

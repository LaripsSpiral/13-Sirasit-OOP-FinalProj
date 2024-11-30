using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FishingBait : Enemy_Fish, IEatable
{
    [Header("Settings")]
    [SerializeField] float _minRange, _maxRange;


    Vector3 spawnPos;
    float hookRange;

    private void Start()
    {
        spawnPos = transform.position;

        hookRange = Random.Range(_minRange, _maxRange);
        transform.position += Vector3.down * hookRange;
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.gameObject.TryGetComponent(out Player player))
            return;

        EatenBy(player);
    }

    public void EatenBy(Player player)
    {
        player.TakeDamage(1);

        if (player.Heart > 0)
            return;

        player.Rb2d.velocity = default;
        player.Rb2d.isKinematic = true;
        player.transform.parent = transform;

        StartCoroutine(AnimHookup());
    }

    IEnumerator AnimHookup()
    {
        while (transform.position.y < spawnPos.y)
        {
            transform.position = Vector3.Lerp(transform.position, spawnPos, hookRange * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.down * _minRange, transform.position + Vector3.down * _maxRange);
    }

}


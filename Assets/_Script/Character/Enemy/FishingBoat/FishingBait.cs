using System.Collections;
using UnityEngine;

public class FishingBait : Character, IEatable
{
    [SerializeField] FishingBoat _ownerBoat;

    float _hookRange;

    LineRenderer _lineRenderer;

    public void Init(FishingBoat ownerBoat, float hookRange) 
    {
        _ownerBoat = ownerBoat;
        _hookRange = hookRange;
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        transform.position = new Vector3(transform.parent.position.x, - _hookRange, 0);
    }

    void Update()
    {
        UpdateHookLine();
    }

    void OnCollisionEnter2D(Collision2D col)
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

        HookUp(player);
    }
    void UpdateHookLine()
    {
        _lineRenderer.SetPosition(0, _ownerBoat.BaitSpawnPoint.position + Vector3.back);
        _lineRenderer.SetPosition(1, transform.position);
    }

    public void HookUp(Player player = null)
    {
        if (player)
        {
            player.Rb2d.velocity = default;
            player.Rb2d.isKinematic = true;

            player.Mouth.parent = transform;
            player.Mouth.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));

            player.transform.parent = player.Mouth;
            player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 90));
        }

        StartCoroutine(AnimHookup());
    }
    public IEnumerator AnimHookup()
    {
        
        Rb2d.velocity = default;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = new(0, 0, 0, .3f);
        

        while (Vector2.SqrMagnitude(transform.position - _ownerBoat.BaitSpawnPoint.position) > 1)
        {
            Rb2d.MovePosition(Vector3.Lerp(transform.position, _ownerBoat.BaitSpawnPoint.position, Speed * 7 * Time.deltaTime));

            yield return new WaitForSeconds(.025f);
        }

        Destroy(gameObject);
    }

}


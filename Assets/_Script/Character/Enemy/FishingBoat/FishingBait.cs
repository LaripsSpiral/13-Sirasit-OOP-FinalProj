using System.Collections;
using UnityEngine;

public class FishingBait : Character, IEatable
{
    [SerializeField] FishingBoat _ownerBoat;

    LineRenderer _lineRenderer;
    RectTransform _areaRectT;

    bool _isCaught;

    public void Init(FishingBoat ownerBoat, RectTransform areaRectT) 
    {
        _ownerBoat = ownerBoat;
        _areaRectT = areaRectT;
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateHookLine();


        MoveTowardPlayer();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.gameObject.TryGetComponent(out Player player))
            return;

        EatenBy(player);
    }

    private void MoveTowardPlayer()
    {
        if (_isCaught) return;

        //Find
        Player target = FindAnyObjectByType<Player>();

        //Not Exist
        if (target == null)
            return;

        //Check Player Inside
        if (!IsPointInRT(target.transform.position, _areaRectT))
        {
            Rb2d.velocity = Vector2.zero;
            return;
        }

        //Move To
        Vector2 lerpTargetPos = Vector2.LerpUnclamped(transform.position, target.transform.position, Speed * 7.5f * Time.deltaTime );
        Rb2d.MovePosition(lerpTargetPos);
    }

    bool IsPointInRT(Vector2 point, RectTransform rt)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, point, null, out Vector2 localPoint);

        return rt.rect.Contains(localPoint);
    }

    public void EatenBy(Player player)
    {
        player.TakeDamage(1);

        if (player.Heart > 0)
            return;

        _isCaught = true;
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
    }

}


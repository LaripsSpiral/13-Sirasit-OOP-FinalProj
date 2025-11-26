using PrimeTween;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float duration = 1f;

    public Sequence CurrentSequence { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sequence Fade(float start, float end)
    {
        var sequence = Sequence.Create();
        CurrentSequence = sequence;

        sequence.Chain(Tween.Alpha(canvasGroup, start, end, duration));
        return sequence;
    }
}

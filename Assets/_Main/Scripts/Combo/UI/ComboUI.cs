using TMPro;
using UnityEngine;
using PrimeTween;

public class ComboUI : MonoBehaviour
{
    [Header("Combo UI")]
    [SerializeField]
    private Canvas comboCanvas;
    [SerializeField]
    private GameObject comboPanel;
    [SerializeField]
    private TMP_Text comboText;
    [SerializeField]
    private TMP_Text comboCountText;

    [Header("Combo Tier")]
    [SerializeField]
    // 5 fishs eat, combo tier will be Yummy!!
    // 8 fishs eat, combo tier will be Delicious!!
    // 10 fishs eat, combo tier will be Crazy!!
    // 15 fishs eat, combo tier will be OMG!!
    // 20 fishs eat, combo tier will be ThisFishIsCrazy!!
    private string[] comboTierTexts = { "Yummy!!", "Delicious!!", "Crazy!!", "OMG!!", "ThisFishIsCrazy!!" };

    [Header("Gradient Colors")]
    [SerializeField]
    // Gradient colors for each tier: [topLeft, topRight, bottomLeft, bottomRight]
    private Color[] tierGradientTopLeft = { Color.white, Color.yellow, Color.green, Color.cyan, Color.red };
    [SerializeField]
    private Color[] tierGradientTopRight = { Color.white, new Color(1f, 0.5f, 0f), new Color(0f, 1f, 0f), Color.blue, Color.magenta };
    [SerializeField]
    private Color[] tierGradientBottomLeft = { Color.white, Color.yellow, Color.green, Color.cyan, Color.red };
    [SerializeField]
    private Color[] tierGradientBottomRight = { Color.white, new Color(1f, 0.5f, 0f), new Color(0f, 1f, 0f), Color.blue, Color.magenta };

    [Header("Animation Settings")]
    [SerializeField]
    private float popAnimationDuration = 0.3f;
    [SerializeField]
    private float popScale = 1.2f;
    [SerializeField]
    private Ease popEase = Ease.OutBack;

    private int previousTierIndex = -1;
    private Sequence currentAnimation;

    public void UpdateComboUI(int comboCount)
    {
        if (comboCountText != null)
        {
            comboCountText.text = "x" + comboCount.ToString();
        }

        int tierIndex = GetTierIndex(comboCount);
        bool tierChanged = tierIndex != previousTierIndex && tierIndex >= 0;

        if (comboText != null)
        {
            if (tierIndex >= 0 && tierIndex < comboTierTexts.Length)
            {
                comboText.text = comboTierTexts[tierIndex];

                // Enable vertex gradient
                comboText.enableVertexGradient = true;

                // Set gradient colors
                if (tierIndex < tierGradientTopLeft.Length &&
                    tierIndex < tierGradientTopRight.Length &&
                    tierIndex < tierGradientBottomLeft.Length &&
                    tierIndex < tierGradientBottomRight.Length)
                {
                    VertexGradient gradient = new VertexGradient(
                        tierGradientTopLeft[tierIndex],
                        tierGradientTopRight[tierIndex],
                        tierGradientBottomLeft[tierIndex],
                        tierGradientBottomRight[tierIndex]
                    );
                    comboText.colorGradient = gradient;
                }
            }
            else
            {
                // Hide tier text if no tier reached yet
                comboText.text = "";
                comboText.enableVertexGradient = false;
            }
        }

        if (tierChanged)
        {
            PlayPopAnimation();
        }

        previousTierIndex = tierIndex;
    }

    public void ShowComboPanel()
    {
        if (comboPanel != null)
        {
            comboPanel.SetActive(true);
            PlayPopAnimation();
        }
    }

    public void HideComboPanel()
    {
        if (comboPanel != null)
        {
            // Stop any ongoing animations
            if (currentAnimation.isAlive)
            {
                currentAnimation.Stop();
            }
            comboPanel.SetActive(false);
        }
        previousTierIndex = -1;
    }

    private void PlayPopAnimation()
    {
        // Stop any ongoing animation
        if (currentAnimation.isAlive)
        {
            currentAnimation.Stop();
        }

        // Reset scale before animating
        if (comboPanel != null)
        {
            comboPanel.transform.localScale = Vector3.one;
        }

        // Create pop animation sequence
        if (comboPanel != null)
        {
            currentAnimation = Tween.Scale(comboPanel.transform, popScale, popAnimationDuration * 0.5f, popEase)
                .Chain(Tween.Scale(comboPanel.transform, Vector3.one, popAnimationDuration * 0.5f, Ease.OutQuad));
        }

        // Also animate the combo text if it exists and has content
        if (comboText != null && !string.IsNullOrEmpty(comboText.text))
        {
            comboText.transform.localScale = Vector3.one;
            Tween.Scale(comboText.transform, popScale, popAnimationDuration * 0.5f, popEase)
                .Chain(Tween.Scale(comboText.transform, Vector3.one, popAnimationDuration * 0.5f, Ease.OutQuad));
        }
    }

    private int GetTierIndex(int count)
    {
        // 5 fishs eat, combo tier will be Yummy!! (index 0)
        // 8 fishs eat, combo tier will be Delicious!! (index 1)
        // 10 fishs eat, combo tier will be Crazy!! (index 2)
        // 15 fishs eat, combo tier will be OMG!! (index 3)
        // 20 fishs eat, combo tier will be ThisFishIsCrazy!! (index 4)

        if (count >= 20)
            return 4; // ThisFishIsCrazy!!
        else if (count >= 15)
            return 3; // OMG!!
        else if (count >= 10)
            return 2; // Crazy!!
        else if (count >= 8)
            return 1; // Delicious!!
        else if (count >= 5)
            return 0; // Yummy!!
        else
            return -1; // No tier yet
    }

    private void LateUpdate()
    {
        // Keep the combo canvas from inheriting/player rotation so it doesn't flip.
        // This keeps it upright and makes it face the main camera on the horizontal plane.
        if (comboCanvas != null)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                Vector3 dir = cam.transform.position - comboCanvas.transform.position;
                dir.y = 0f; // keep upright (no tilt)
                if (dir.sqrMagnitude > 0.0001f)
                {
                    comboCanvas.transform.rotation = Quaternion.LookRotation(dir);
                }
            }
            else
            {
                // If no main camera, just keep canvas upright in world space to avoid flips
                comboCanvas.transform.rotation = Quaternion.identity;
            }
        }
    }
}

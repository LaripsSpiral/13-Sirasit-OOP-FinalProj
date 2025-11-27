using UnityEngine;
using UnityEngine.UI;
using Main.Character;

namespace Main.Player
{
    public class DashCooldownUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerCharacter target;

        [SerializeField]
        private GameObject cooldownPanel;

        [SerializeField]
        private Image cooldownImage;

        private void Start()
        {
            if (target == null)
            {
                target = FindAnyObjectByType<PlayerCharacter>();
            }

            if (cooldownImage == null)
            {
                cooldownImage = GetComponentInChildren<Image>();
            }

            if (target == null)
            {
                Debug.LogWarning("DashCooldownUI: No PlayerCharacter found in scene. Please assign one in inspector.");
                if (cooldownPanel != null)
                    cooldownPanel.SetActive(false);
                return;
            }

            // Hide panel initially since dash is ready
            if (cooldownPanel != null)
            {
                cooldownPanel.SetActive(false);
            }

            target.OnDashCooldownProgress += OnProgress;
        }

        private void OnDestroy()
        {
            if (target != null)
            {
                target.OnDashCooldownProgress -= OnProgress;
            }
        }

        private void OnProgress(float remainingRatio)
        {
            if (cooldownImage != null)
            {
                // `remainingRatio` is already (ElapsedTime / CoolDownTime) and goes 1 -> 0
                cooldownImage.fillAmount = Mathf.Clamp01(remainingRatio);
            }

            // Show panel when on cooldown (ratio > 0), hide when ready (ratio == 0)
            if (cooldownPanel != null)
            {
                bool isOnCooldown = remainingRatio > 0f;
                cooldownPanel.SetActive(isOnCooldown);
            }
        }
    }
}


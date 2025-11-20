using UnityEngine;
using UnityEngine.UI;
using Main.Character;

namespace Main.Effects
{
    public class SpeedBuffUI : MonoBehaviour
    {
        [SerializeField]
        private BaseCharacter target;

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
                Debug.LogWarning("SpeedBuffUI: No target BaseCharacter found in scene. Please assign one in inspector.");
                if (cooldownImage != null)
                    cooldownImage.gameObject.SetActive(false);
                return;
            }

            target.OnSpeedBuffProgress += OnProgress;
            target.OnSpeedBuffChanged += OnBuffChanged;

            OnBuffChanged(target.CurrentSpeedBuff);
        }

        private void OnDestroy()
        {
            if (target != null)
            {
                target.OnSpeedBuffProgress -= OnProgress;
                target.OnSpeedBuffChanged -= OnBuffChanged;
            }
        }

        private void OnProgress(float remainingRatio)
        {
            if (cooldownImage == null)
                return;

            // `remainingRatio` is already (ElapsedTime / CoolDownTime) and goes 1 -> 0
            cooldownImage.fillAmount = Mathf.Clamp01(remainingRatio);
        }

        private void OnBuffChanged(float totalBuff)
        {
            if (cooldownImage == null)
                return;

            bool active = totalBuff > 0f;
            cooldownImage.gameObject.SetActive(active);
        }
    }
}

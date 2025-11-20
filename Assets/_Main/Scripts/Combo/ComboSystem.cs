using NaughtyAttributes;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField, Tooltip("Combo time when fish eat other fish ( if not eat other fish, combo will be reset )")]
    private float comboTime = 5f;

    [Header("Combo UI")]
    [SerializeField]
    private ComboUI comboUI;

    [SerializeField, ReadOnly]
    private int comboCount = 0;
    public int ComboCount => comboCount;

    [SerializeField, ReadOnly]
    private float comboTimer = 0f;

    private void Update()
    {
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    public void AddCombo()
    {
        comboCount++;
        comboTimer = comboTime;

        if (comboUI != null)
        {
            comboUI.ShowComboPanel();
            comboUI.UpdateComboUI(comboCount);
        }
    }

    private void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 0f;

        if (comboUI != null)
        {
            comboUI.HideComboPanel();
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public static class TimeSystem
{
    public static void Resume(float lerp)
    {
        LerpTimeTo(1, lerp);
    }

    public static void Slowdown(float slowVal, float lerp)
    {
        LerpTimeTo(slowVal, lerp);
    }

    public static void PauseTime(float lerp)
    {
        LerpTimeTo(0, lerp);
    }

    static void LerpTimeTo(float targetTime, float lerp)
    {
        for (float f = 0; f < lerp; f++)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTime, f);
            Debug.Log($"Decreasing : Curr Time Scale = {Time.timeScale}");
        }
    }
}

using UnityEngine;

namespace Main.Times
{
    public static class TimeSystem
    {
        private const float timeLerp = 1;
        public static void Resume()
        {
            LerpTimeTo(1);
        }

        public static void Slowdown(float slowVal)
        {
            LerpTimeTo(slowVal);
        }

        public static void PauseTime()
        {
            LerpTimeTo(0);
        }

        static void LerpTimeTo(float targetTime)
        {
            for (float f = 0; f < timeLerp; f++)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, targetTime, f);
                Debug.Log($"Decreasing : Curr Time Scale = {Time.timeScale}");
            }
        }
    }
}
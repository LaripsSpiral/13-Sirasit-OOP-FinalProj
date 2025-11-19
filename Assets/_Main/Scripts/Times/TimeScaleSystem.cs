using UnityEngine;

namespace Main.Times
{
    public static class TimeScaleSystem
    {
        private const float TIMESCALE_LERP = 1;

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
            for (float f = 0; f < TIMESCALE_LERP; f++)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, targetTime, f);
                Debug.Log($"Decreasing : Curr Time Scale = {Time.timeScale}");
            }
        }

    }
}
namespace Main.Effects
{
    public class EffectBuff
    {
        public float SpeedBuff;

        // Add BuffSpeed
        public float AddSpeedBuff(float value)
        {
            SpeedBuff += value;
            return SpeedBuff;
        }

        // Remove BuffSpeed
        public float RemoveSpeedBuff(float value)
        {
            SpeedBuff -= value;
            return SpeedBuff;
        }
    }
}
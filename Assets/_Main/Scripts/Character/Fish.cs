using Unity.Jobs;
using UnityEngine;

namespace Main.Character
{
    public class Fish : BaseCharacter
    {
        public float GetSize() => transform.localScale.x;

        protected void Eat()
        {
            if (CanEat())
            {

            }
            else
            {

            }
        }

        private bool CanEat()
        {
            return true;
        }
    }
}
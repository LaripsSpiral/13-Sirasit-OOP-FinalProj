using UnityEngine;

namespace Main.Character
{
    public class Fish : BaseCharacter
    {
        [SerializeField]
        protected float size = 10;

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
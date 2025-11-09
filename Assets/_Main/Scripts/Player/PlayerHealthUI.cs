using UnityEngine;
using UnityEngine.UI;

namespace Main.Player
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] 
        private Transform heartUIParent;

        [SerializeField] 
        private GameObject heartPrefab;

        private void UpdateHeartUI(int value)
        {
            //Clear Heart
            foreach (var oldHeart in heartUIParent.GetComponentsInChildren<Image>())
            {
                Destroy(oldHeart.gameObject);
            }

            //Add Remain Heart
            for (int i = 0; i < value; i++)
            {
                Instantiate(heartPrefab, heartUIParent);
            }
        }
    }
}
using UnityEngine;

namespace Main.Menu
{
    public abstract class BaseUI : MonoBehaviour
    {
        private void Awake()
        {
            ToggleShow(false);
        }

        public void ToggleShow(bool toggle)
        {
            gameObject.SetActive(toggle);
        }
    }
}

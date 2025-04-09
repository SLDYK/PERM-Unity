using UnityEngine;
namespace PrsEditor
{
    public class EffectRecycle : MonoBehaviour
    {
        public void EndAndRecycle()
        {
            this.gameObject.SetActive(false);
        }
    }
}
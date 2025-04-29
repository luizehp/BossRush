using UnityEngine;

namespace Necromancer.Lightning
{
    public class LightningDestroy : MonoBehaviour
    {
        void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}

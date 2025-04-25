using UnityEngine;

namespace Necromancer.Fireball
{
    public class Fireball : MonoBehaviour
    {
        public float speed = 5f;

        void Start()
        {
            Destroy(gameObject, 3f);
        }

        void Update()
        {
            transform.Translate(Vector2.down * (speed * Time.deltaTime));
        }
    }
}

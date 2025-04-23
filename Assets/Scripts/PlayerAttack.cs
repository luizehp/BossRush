using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            Debug.Log("Hit");
        }
    }
}

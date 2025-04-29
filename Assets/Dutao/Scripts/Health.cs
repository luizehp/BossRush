using UnityEngine;

public class Health : MonoBehaviour
{
    public int attackDamage = 10;
    public int maxHealth = 100;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            maxHealth -= attackDamage;
        }
        if (maxHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}

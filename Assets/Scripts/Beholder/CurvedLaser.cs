using UnityEngine;

public class CurvedLaser : MonoBehaviour
{
    public float radius = 2f; // Raio do arco
    public float angularSpeed = 180f; // Velocidade angular
    private float angle = 0f; // Ângulo inicial do laser
    private Vector3 center;

    void Start()
    {
        // O centro é a posição inicial do Beholder
        center = transform.position;
    }

    void Update()
    {
        // Aumenta o ângulo ao longo do tempo, criando um movimento circular
        angle += angularSpeed * Time.deltaTime;

        // Quando o laser completar meio círculo (180º), ele será destruído
        if (angle >= 180f)
        {
            Destroy(gameObject); // Destroi o laser após ele completar o arco
            return;
        }

        // Converte o ângulo de graus para radianos para calcular a posição
        float rad = angle * Mathf.Deg2Rad;
        
        // Calcula as novas posições baseadas no ângulo
        float x = Mathf.Cos(rad) * radius; // Movimento horizontal
        float y = Mathf.Sin(rad) * radius; // Movimento vertical

        // Atualiza a posição do laser com base no arco
        transform.position = center + new Vector3(x, y, 0);
    }
}

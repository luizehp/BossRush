using UnityEngine;

public class DemonFB : MonoBehaviour
{
    public GameObject FB;        // your fireball prefab
    public Transform FBPos;      // spawn point

    // how often to fire the volley
    public float volleyCooldown = 6f;
    private float timer = 0f;

    public float[] extraDelays = new float[] { 0f, 0.3f, 0.6f };

    private Vector3[] offsets = new Vector3[]
    {
        new Vector3( 0f,  0f),
        new Vector3( 2f, -0.5f),
        new Vector3(-2f, -0.5f)
    };

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= volleyCooldown)
        {
            timer = 0f;
            SpawnVolley();
        }
    }

    void SpawnVolley()
    {
        for (int i = 0; i < offsets.Length; i++)
        {
            GameObject fbInstance = Instantiate(FB,
                                                FBPos.position + offsets[i],
                                                Quaternion.identity);

            EnemyBulletScript ebs = fbInstance.GetComponent<EnemyBulletScript>();
            if (ebs != null && i < extraDelays.Length)
                ebs.launchDelay = extraDelays[i];
        }
    }
}

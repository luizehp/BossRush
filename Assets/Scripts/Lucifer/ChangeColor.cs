using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeColor : MonoBehaviour
{
    public Light2D bossLight;

    public Color phaseColor = Color.blue;

    public ParticleSystem[] redFlames;
    public ParticleSystem[] blueFlames;
    public Light2D[] lights;
    public void Change()
    {
        if (bossLight != null)
            bossLight.color = phaseColor;

        int count = Mathf.Max(
            redFlames.Length,
            blueFlames.Length,
            lights.Length
        );

        for (int i = 0; i < count; i++)
        {
            var redEmission = redFlames[i].emission;
            redEmission.enabled = false;
            var blueEmission = blueFlames[i].emission;
            blueEmission.enabled = true;
            lights[i].color = phaseColor;
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lava : MonoBehaviour
{
    public Material lavaMaterial;
    public void ChangeColor()
    {
        lavaMaterial.SetColor("_Color", Color.blue);
        lavaMaterial.SetColor("_HighlightColor", Color.cyan);
    }
    public void StartLava()
    {
        lavaMaterial.SetColor("_Color", Color.red);
        lavaMaterial.SetColor("_HighlightColor", Color.yellow);
    }


}

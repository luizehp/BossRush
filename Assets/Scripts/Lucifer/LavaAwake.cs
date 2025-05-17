using UnityEngine;

public class LavaAwake : MonoBehaviour
{
    public Material lavaMaterial;
    void Start()
    {
        lavaMaterial.SetColor("_Color", Color.red);
        lavaMaterial.SetColor("_HighlightColor", Color.yellow);
    }
}

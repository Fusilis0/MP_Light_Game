using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KeyManager : MonoBehaviour
{
    public List<Material> keyColors;
    public MeshRenderer mesh;
    public int colorNumber;

    public void Update()
    {
        mesh.material = keyColors[colorNumber];
    }

}

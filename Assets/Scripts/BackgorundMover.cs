using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgorundMover : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //arka plandaki resmin hareket etmesini sağladık;
        //offset x değerini değiştirerek;
        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(Time.deltaTime * 0.1f, 0);
    }
}

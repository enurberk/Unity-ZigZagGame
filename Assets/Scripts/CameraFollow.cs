using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 diffVector;         //kamera ve oyuncu arasındaki fark vektörü
    Transform player;           //oyuncunun yeri;
    
    void Start()                // Start is called before the first frame update
    {
        //playercontrollerı olan nesneyi bul ve onun transformunu al.
        player = FindObjectOfType<PlayerController>().transform;
        //fark vektörü = oyuncunun konumu - kameranın konumu;
        diffVector = player.position - transform.position;
    }

    // LateUpdate kullanıcaz çünkü daha sonra çağrılacak;
    void LateUpdate()
    {
        //kameranın yeni konumunu hesaplıyor. 
        //oyuncunun konumu - kamera ile oyuncunun arasındaki fark vektörü;
        transform.position = player.position - diffVector;
    }
}

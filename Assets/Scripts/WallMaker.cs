
using UnityEngine;

public class WallMaker : MonoBehaviour
{
    public Transform lastWall;      //son duvarın transformu;
    public GameObject wallPrefab;   //klonlanacak duvarın prefabı;
    Vector3 lastPos;                //duvarın son konumu;
    Camera cam;                     //kamera;
    PlayerController player;        //playerın örneğini oluşturduk;

    void Start()                    // Start is called before the first frame update
    {
        //duvarın konumunu transfromdan ürettiğimiz örnekten aldık;
        lastPos = lastWall.position;
        //playerı oluşturduk;
        player = FindObjectOfType<PlayerController>();
        //main camerayı aldık;
        cam = Camera.main;
        //duvar oluşturma;
        InvokeRepeating("CreateWalls", 1, 0.1f);
    }
    private void CreateWalls()
    {
        //oyuncu ile en son block arasındaki fark
        float distance = Vector3.Distance(lastPos, player.transform.position);
        //eğer oyuncu ile son_block arasındaki mesafe cameranın ekranından büyük ise block oluşturmasın;
        //fazla block oluşturmayı engelledik;
        if (distance > cam.orthographicSize * 2) return;
        //yeni konum;
        Vector3 newPos = Vector3.zero;
        //pc 0,11 arası random sayı üretecek;
        int rand = Random.Range(0, 11);
        //sayı 5 ten küçükse sola duvar örülecek;
        if(rand <= 5)
            newPos = new Vector3(lastPos.x - 0.707f, lastPos.y, lastPos.z + 0.707f);
        //sayı beşten büyükse düz devam edecek;
        else
            newPos = new Vector3(lastPos.x + 0.70711f, lastPos.y, lastPos.z + 0.70711f);
        //yukarıda belirtilen konumlara üretilecek olan küpleri üretme;
        GameObject newBlock = Instantiate(wallPrefab, newPos, Quaternion.Euler(0, 45, 0), transform);
        //elmasların random oluşması için; 
        //ilk bloğun çocuğunu aldık ve aktif etmek için rand fonk kullandık;
        newBlock.transform.GetChild(0).gameObject.SetActive(rand % 3 == 2);
        //son duvarın konumunu güncelliyor;
        lastPos = newBlock.transform.position;
    }
}

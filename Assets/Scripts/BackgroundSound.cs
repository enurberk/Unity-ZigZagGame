using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Amaç bir tane örnek oluşturmak ve bu örnek(ses dosyası) üzerinden devam etmek. 
//Bunu yapmazsak her sahne için yeni bir örnek olulturuuyor.

public class BackgroundSound : MonoBehaviour
{
    static BackgroundSound instance;         //static bir örnek oluşturduk;

    void Start()                            // Start is called before the first frame update
    {
        //eğer bu nesne yok ise, oluştur, this->bu clastan demek;
        if (!instance)
        {
            instance = this;
        }
        //eğer örnek bu klastan değilse = yani yeni üretilmişse yok et;
        //yani yeni ses dosyası üretilmeyecek aynı dosya üzerinden işler devam edecek;
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        //bu method her sahne yenilendiğinde istemediğiniz objeleri yok etmenizi engeller;
        //mesela burada bu kodu kullanmadığımız takdirde player her düştüğünde müzik en baştan başlıyor
        //ve sıkıcı bir hal alıyor;
        DontDestroyOnLoad(instance);
    }
}

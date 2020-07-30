using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    delegate void TurnDelegate();             //delegate ile platforma göre dokunmatik method 
    TurnDelegate turnDelegate;                //yada klavye methodu seçmemizi sağlayacağız;


    public float moveSpeed = 2;               //hız;
    bool lookingRight = true;                 //karakter hangi yöne dönecek bunu belirlemek için boolean kullanıcaz;
    GameManager gameManager;                  //GameManager den örnek aldık;
    Animator anim;                            //animasyonu almak için animatör tanımlıyoruz;
    public Transform rayOrigin;               //animasyondan ışın göndermek için ışının başlangıç noktasını oluşturduk.       
    public Text scoreTxt, hScoreTxt;          //oyun skorunu için;
    public int Score { get; private set; }    //skor değerleri için int değişken oluşturduk;
    public int HScore { get; private set; }   //skor değerleri için int değişken oluşturduk;
    public ParticleSystem effect;             //elmasları aldığında çıkan duman efekti;
   
    void Start()                              // Start is called before the first frame update
    {
        //platform;
        #region PLATFORM FOR TURNİNG
            #if UNITY_EDITOR
                turnDelegate = TurnUsingKeyboard;
            #endif
            #if UNITY_ANDROID
                turnDelegate = TurnUsingTouch;
            #endif
            #if UNITY_IOS
                turnDelegate = TurnUsingTouch;
            #endif
        #endregion

        //oyunun başlamasını kontrol etmek için oyun dosyalarında 
        //GameManager tipli objenin buunmasını sağladık;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        anim = gameObject.GetComponent<Animator>();
        LoadHighScore();
    }   
    //yüksek skoru hafızada tutmak ve yazdırmak için bu methodu kullanıcaz;
    private void LoadHighScore()
    {
        HScore = PlayerPrefs.GetInt("hiscore");
        hScoreTxt.text = HScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameStarted) return;
        //oyun başlangı. animsyonunu çağırdık;
        anim.SetTrigger("gameStarted");
        //playerın zamanla hızlanmasını sağlayan kod;
        moveSpeed *= 1.0001f;
        //Debug.Log(moveSpeed);
        //karakterin düz gitmesini sağlayan iki alternatif kod;
        //transform.position += transform.forward * Time.deltaTime * moveSpeed;
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * moveSpeed);
        //platforma göre oyunu oynama yöntemini belirleyen delegate kullanımı;
        turnDelegate();
        //düşmeyi kontrol et
        CheckFalling();
    }
    //bilgisayar (boşluk tuşu) için method;
    private void TurnUsingKeyboard()
    {
        //klavyeden boşluk tuşuna basıldığında dön;
        if (Input.GetKeyDown(KeyCode.Space))
            Turn();
    }
    //dokunmatik ekran için method;
    private void TurnUsingTouch()
    {
        //klavyeden boşluk tuşuna basıldığında dön;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            Turn();
    }

    //update methodunda checkfalling her framede çağırılacak bunu azaltmak için
    //geçen süreyi saniyede 5 kez çağırılacak şekilde ayarlıyoruz;
    float elapsedTime = 0;
    //saniyede 5 kez;
    float freg = 1 / 5f;
    private void CheckFalling()
    {
        //geçen süre +zaman yani toplam geçen süre 0.2 frekansından küçükse saniyede 5 kez kontrol edecek.
        if ((elapsedTime += Time.deltaTime) > freg)
        {
            //eğer aşağıda bir şey yok ise;
            //aşağı yönde ışın gönderiyoruz. eğer bir şeye çarpmıyorsa ışın aşağıda bişi yok demektir;
            if (!Physics.Raycast(rayOrigin.position, new Vector3(0, -1, 0)))
            {
                //düşme animasyonunu göster ve oyunu baştan başlat;
                anim.SetTrigger("falling");
                gameManager.RestartGame();
                //geçen süreyi sıfırla
                elapsedTime = 0;
            }
        }
    }

    private void Turn()
    {
        //sağa bakıyorsa sola çevir;
        if (lookingRight)
        {
            transform.Rotate(new Vector3(0, 1, 0), -90);
        }
        //sola bakıyorsa sağa çevir;
        else
        {
            transform.Rotate(new Vector3(0, 1, 0), 90);
        }
        //ilk sağa bakıyordu sonra sola döndü, yani sağa bakmıyor;
        lookingRight = !lookingRight;
    }
    //elmasların içinden geçme;
    private void OnTriggerEnter(Collider other)
    {
        //eğer crystal tagli nesnenin içinden geçerse;
        if (other.tag.Equals("crystal"))
        {
            MakeScore();
            //elması aldıktan sonraki efekti yapacak method;
            CreateEffect();
            //skoru artırıp elması yok edecek;
            Destroy(other.gameObject);
        }
    }
    //eski blokları yok etme;
    private void OnCollisionExit(Collision collision)
    {
        Destroy(collision.gameObject, 3f);   
    }
    //elması alıncaki efekt, 1sn sonra yok olacak
    private void CreateEffect()
    {
        //local değişken old için variable değeri oluşturduk;
        //transform(dönüşüm) player'ın pozisyonu vs;
        var vfx = Instantiate(effect, transform);
        //efekti 1 sn sonra yok et;
        Destroy(vfx, 1f);
    }

    //skoru ekrana yazdırma ve yüksek skoru hafızada tutma;
    private void MakeScore()
    {
        Score++;
        scoreTxt.text = Score.ToString();
        if (Score > HScore)
        {
            HScore = Score;
            PlayerPrefs.SetInt("hiscore", HScore);
            hScoreTxt.text = HScore.ToString();
        }
    }
}

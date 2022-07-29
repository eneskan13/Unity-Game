//-----------------------------------------------
//
//This is  Game Manager that is responsible for manage Coins,Distance,Records and fuel
//Orginally writed for My game In Summer 2016   ALIyerEdon Unity 5.3.6f1234
//-----------------------------------------------/
using UnityEngine;
using System.Collections;


//include this for UI usage
using UnityEngine.UI;
//-----------------------------------------------
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
//Game Manager is responsible for Coins,Distance,Fuel...
public class GameManager : MonoBehaviour
{

    [Header("Informations")]
    // UI-----------------------------------------------
    // For display Coins,Fuel , Record and distance
    public Text DistanceTXT;
    public Text RecordTXT;
    public Text CoinTXT;
    public Text FuelTXT;
    public Text LastRecord;
    public Text ammoTXT;
    public Text dolarTxt;
    public GameObject fuelHandle;
    [Header("Sliders")]
    // SHow fuel amount
    public Slider FuelSlider;
    // Show passed distance on down of the screen
    public Slider DitanceSlider;
    //-----------------------------------------------
    public Animator bulletPlayerAnimator;
    //Manager-----------------------------------------------
    //player transform to read position as distance
    Transform player;
    //is game started?
    bool Started;

    //----------------------------------------------------

    [Header("Fuel")]
    //fuel-----------------------------------------------
    // Total fuel
    public float TotalFuel = 100f;
    public float maxFuel = 100f;
    // Fuel reducing time step (delay in coroutine)
    public float FuelTime = .17f;
    // Fuel reduced amount by time
    float FuelVal = .3f;
    //-----------------------------------------------

    //Coin Manager-----------------------------------------------
    // Game total coins
    int Coins;
    //-----------------------------------------------
    public GameObject bullet;
    [Header("Coins And Awards")]
    //Get Coin Audio Source-----------------------------------------------
    //Receiving coind sound effect
    public AudioSource coinSound;

    public static GameManager instance;

    // Awarded coin box (award based on distance)
    public GameObject coinAwardedBox;

    // Award text
    public Text awardedText;

    // Award box animator
    public Animator awardAnimator;


    [Header("Complete Level")]
    // Win and lose windows
    public GameObject youWinMenu;
    public GameObject youLostMenu;
    // End of the level (Level length)
    public float levelLength = 3700f;

    // How much coins give to player when finish the level?
    public int winnerAwardedCoins = 1000;
    [Header("Low")]
    public Image lowFuel;
    public Image lowNitro;
    [SerializeField] private int minFuelError;
    [SerializeField] private int minNitroError;




    #region nitroVariable
    [FoldoutGroup("Nitro")]
    public float MaxNitro;
    [FoldoutGroup("Nitro")]
    public float mainNitro;
    [FoldoutGroup("Nitro")]
    public float negativeNitro = 5;
    private bool isNitroPlaying;
    [FoldoutGroup("Nitro")]
    [SerializeField] private GameObject nitroHandle;
    #endregion
    //Start-----------------------------------------------
    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }
    IEnumerator Start()
    {
        //Coins Initialization-----------------------------------------------
        Coins = PlayerPrefs.GetInt("Coins");   //read total scror from saved Coins
        CoinTXT.text = Coins + " $";   // Display total coins on Start
                                       //-----------------------------------------------
        mainNitro = MaxNitro;
        nitroHandleSetter();
        //Start Main Game   -----------------------------------------------
        yield return new WaitForEndOfFrame();   //Player is Spawned afer milisecond. we wait .3 and then find it
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Started = true;   // The game is now started. you can run your codes on update function
                          //-----------------------------------------------
                          ////-----------------------------------------------
        /// 
        // Read if distance based award is already gived for current level, Set it to gived
        if (PlayerPrefs.GetInt("c500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c500 = true;
            LastRecord.text = "Record:500";
        }
        if (PlayerPrefs.GetInt("c1000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c1000 = true;
            LastRecord.text = "Record:1000";
        }
        if (PlayerPrefs.GetInt("c1500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c1500 = true;
            LastRecord.text = "Record:1500";
        }
        if (PlayerPrefs.GetInt("c2000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c2000 = true;
            LastRecord.text = "Record:2000";
        }
        if (PlayerPrefs.GetInt("c2500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c2500 = true;
            LastRecord.text = "Record:2500";
        }
        if (PlayerPrefs.GetInt("c3000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c3000 = true;
            LastRecord.text = "Record:3000";
        }
        if (PlayerPrefs.GetInt("c3500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c3500 = true;
            LastRecord.text = "Record:3500";
        }
        if (PlayerPrefs.GetInt("c4000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c4000 = true;
            LastRecord.text = "Record:4000";
        }




        //Fuel Decreso  r//-----------------------------------------------
        while (true)
        {//responsible to decrese fuel amount by   time and value read from upgrade   menu
            yield return new WaitForSeconds(FuelTime);
            TotalFuel -= FuelVal;
            //FuelSlider.value = TotalFuel;


            if (minFuelError - 5 >= TotalFuel && lowFuel.gameObject.activeInHierarchy == false)
            {
                InvokeRepeating("setFuelErrorColor", 0.2f, 0.5f);
                lowFuel.gameObject.SetActive(true);
            }
            else if (minFuelError + 5 <= TotalFuel && lowFuel.gameObject.activeInHierarchy == true)
            {
                lowFuel.gameObject.SetActive(false);
                CancelInvoke("setFuelErrorColor");

            }



            if (TotalFuel >= 0)
            {
                //FuelTXT.text = Mathf.Floor(TotalFuel).ToString();
                fuelHandle.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(-90 + ((100 - TotalFuel) / 0.55f), -90, 90));
            }
            else if (TotalFuel < 0 && TotalFuel <= 0)
            {
                //youLostMenu.SetActive (true);
                //Time.timeScale = 0;
                fuelFinished = true;
                StartFuelFinish();

            }
        }

    }
    #region nitro
    void nitroUpdate()
    {
        Mathf.Clamp(mainNitro -= negativeNitro, 0, MaxNitro);
        CarController.instance.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(CarController.instance.gameObject.GetComponent<Rigidbody2D>().transform.right.x * nitroMainController.instance.nitroPower * Time.deltaTime + CarController.instance.gameObject.GetComponent<Rigidbody2D>().velocity.x, CarController.instance.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        CarController.instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(CarController.instance.gameObject.GetComponent<Rigidbody2D>().velocity, 25);
        print(CarController.instance.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
        nitroHandleSetter();
    }
    public void nitroHandleSetter()
    {
        nitroHandle.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(-90 + ((MaxNitro - mainNitro) / (MaxNitro / 180)), -90, 90));
    }
    public void startNitros()
    {
        nitroMainController.instance.PlayParticle();
        isNitroPlaying = true;
    }
    public void stopNitros()
    {
        nitroMainController.instance.stopParticle();
        isNitroPlaying = false;
    }
    public void shot()
    {
        if (carMainController.instance.bulletValue > 0)
        {
            bulletPlayerAnimator.SetTrigger("hit");
            carMainController.instance.bulletValue -= 1;
            NewMethod();
            GameObject newBullet = Instantiate(bullet, carMainController.instance.shotPos.position, Quaternion.identity);
            newBullet.GetComponent<bulletController>().trans = (carMainController.instance.shotDirection.position - carMainController.instance.shotPos.position).normalized;
        }
    }

    private void NewMethod()
    {
        ammoTXT.text = carMainController.instance.bulletValue.ToString();
    }
    #endregion
    //-----------------------------------------------
    public void setFuelErrorColor()
    {

        if (lowFuel.color == Color.white)
        {
            lowFuel.color = Color.yellow;
        }
        else
        {
            lowFuel.color = Color.white;
        }
    }
    public void setNitroErrorColor()
    {

        if (lowNitro.color == Color.white)
        {
            lowNitro.color = Color.yellow;
        }
        else
        {
            lowNitro.color = Color.white;
        }
    }
    // Update -----------------------------------------------
    float DistanceTemp;

    void Update()
    {

        if (Started)
        {
            CoinDistance();
            if (player.position.x >= DistanceTemp)
            {
                DistanceTXT.text = Mathf.Floor(player.position.x).ToString();
                DistanceTemp = player.position.x;
                DitanceSlider.value = player.position.x;
            }
            if (isNitroPlaying && mainNitro > 0)
                nitroUpdate();
            else if (isNitroPlaying && mainNitro <= 0)
                stopNitros();
            if (minNitroError - 5 >= mainNitro && lowNitro.gameObject.activeInHierarchy == false)
            {
                InvokeRepeating("setNitroErrorColor", 0.2f, 0.5f);
                lowNitro.gameObject.SetActive(true);
            }
            else if (minNitroError + 5 <= mainNitro && lowNitro.gameObject.activeInHierarchy == true)
            {
                lowNitro.gameObject.SetActive(false);
                CancelInvoke("setNitroErrorColor");
            }
        }
    }
    //-----------------------------------------------

    //Add Coin-----------------------------------------------
    public void AddCoin(int value)
    {//add Coin called from coins trigger

        StartCoroutine(TakeCoins());

        CoinTXT.transform.localScale = new Vector3(CoinTXT.transform.localScale.x, CoinTXT.transform.localScale.y + 0.7f,
            CoinTXT.transform.localScale.z);

        if (coinSound)
            coinSound.Play();
        Coins += value;
        CoinTXT.text = Coins + " $";
        PlayerPrefs.SetInt("Coins", Coins);
    }
    //-----------------------------------------------

    IEnumerator TakeCoins()
    {
        yield return new WaitForSeconds(0.03f);
        CoinTXT.transform.localScale = new Vector3(CoinTXT.transform.localScale.x, CoinTXT.transform.localScale.y - 0.7f,
            CoinTXT.transform.localScale.z);
    }
    //Add Fuel-----------------------------------------------
    public void AddFuel(int value)
    {//add fuel called from Fuel Trigger  s
        if (coinSound)
            coinSound.Play();
        TotalFuel = value;
        //CoinTXT.text = Coins.ToString ();
        //PlayerPrefs.SetInt ("Coins", Coins);
    }
    //-----------------------------------------------


    bool c500, c1000, c1500, c2000, c2500, c3000, c3500, c4000;

    // Distance based award
    void CoinDistance()
    {
        if (!c500)
        {
            if (player.transform.position.x >= 500 && player.transform.position.x < 1000)
            {
                AddCoin(1000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "1000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c500 = true;
                PlayerPrefs.SetInt("c500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:500";
            }
        }
        if (!c1000 && c500)
        {
            if (player.transform.position.x >= 1000 && player.transform.position.x < 1500)
            {
                AddCoin(2000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "2000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c1000 = true;
                PlayerPrefs.SetInt("c1000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:1000";
            }
        }
        if (!c1500 && c1000)
        {
            if (player.transform.position.x >= 1500 && player.transform.position.x < 2000)
            {
                AddCoin(3000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "3000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c1500 = true;
                PlayerPrefs.SetInt("c1500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:1500";
            }
        }
        if (!c2000 && c1500)
        {
            if (player.transform.position.x >= 2000 && player.transform.position.x < 2500)
            {
                AddCoin(4000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "4000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c2000 = true;
                PlayerPrefs.SetInt("c2000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:2000";
            }
        }
        if (!c2500 && c2000)
        {
            if (player.transform.position.x >= 2500 && player.transform.position.x < 3000)
            {
                AddCoin(5000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "5000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c2500 = true;
                PlayerPrefs.SetInt("c2500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:2500";
            }
        }
        if (!c3000 && c2500)
        {
            if (player.transform.position.x >= 3000 && player.transform.position.x < 3500)
            {
                AddCoin(6000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "6000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c3000 = true;
                PlayerPrefs.SetInt("c3000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:3000";
            }
        }
        if (!c3500 && c3000)
        {
            if (player.transform.position.x >= 3500 && player.transform.position.x < 4000)
            {
                AddCoin(7000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "7000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c3500 = true;
                PlayerPrefs.SetInt("c3500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:3500";
            }
        }
        if (!c4000 && c3500)
        {
            if (player.transform.position.x >= 3700 && player.transform.position.x < 4500)
            {
                AddCoin(8000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "8000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c4000 = true;
                PlayerPrefs.SetInt("c4000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);// 3 => true | 0 => false
                LastRecord.text = "Record:4000";
                youWinMenu.SetActive(true);
                print("coin");
                AddCoin(winnerAwardedCoins);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }


        // End of the level    
        if (player.transform.position.x >= levelLength && statik.levelComplated != true)
        {
            youWinMenu.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic = true;
            Time.timeScale = 0;
            if (SceneManager.GetActiveScene().buildIndex != 6)
            {
                PlayerPrefs.SetInt("SelectedCar", SceneManager.GetActiveScene().buildIndex);
                PlayerPrefs.SetInt("Car" + SceneManager.GetActiveScene().buildIndex, 3);
                statik.levelComplated = true;
            }
            adsManager.instance.interShower();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

    }



    IEnumerator Awardfalse()
    {

        yield return new WaitForSeconds(1f);
        awardAnimator.SetBool("Award", false);
        yield return new WaitForSeconds(1f);
        coinAwardedBox.SetActive(false);
    }

    [HideInInspector]
    public bool isDead;

    public void StartDead()
    {
        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            youLostMenu.SetActive(true);
            adsManager.instance.interShower();
            Time.timeScale = 0;
        }

    }

    [HideInInspector]
    public bool fuelFinished;


    public void StartFuelFinish()
    {
        StartCoroutine(DeadFuel());
    }
    IEnumerator DeadFuel()
    {
        yield return new WaitForSeconds(1f);
        if (fuelFinished)
        {
            youLostMenu.SetActive(true);
            adsManager.instance.interShower();
            Time.timeScale = 0;
        }

    }
}
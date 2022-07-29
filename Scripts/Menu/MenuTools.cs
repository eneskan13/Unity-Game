using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuTools : MonoBehaviour
{



    // Player starting game in first time score
    public int startScore;

    public Text CoinsTXT;
    public Text CashText;
    [Header("Menu Resolution")]
    public int ResolutionX = 1920;
    public int ResolutionY = 1080;
    public static MenuTools instance;
    public GameObject manuMusic;

    void Start()
    {
        instance = this;
        if (GameObject.Find("LevelMusic(Clone)"))
            Destroy(GameObject.Find("LevelMusic(Clone)"));

        if (!GameObject.Find("MenuMusic(Clone)"))
            Instantiate(manuMusic, Vector3.zero, Quaternion.identity);

        if (PlayerPrefs.GetString("FirstRun") != "True")
        {

            PlayerPrefs.SetString("FirstRun", "True");
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + startScore);

            PlayerPrefs.SetInt("Resolution", 2);// 3 => true | 0 => false

            PlayerPrefs.SetFloat("EngineVolume", 0.1f);
            PlayerPrefs.SetFloat("MusicVolume", 1f);
            PlayerPrefs.SetInt("ShowDistance", 3);
            PlayerPrefs.SetInt("CoinAudio", 3);

            PlayerPrefs.SetInt("Car0", 3);// 3 => true | 0 => false
            PlayerPrefs.SetInt("Level0", 3);// 3 => true | 0 => false

        }

        /*if (PlayerPrefs.GetString ("Update") != "True") {
			PlayerPrefs.SetString ("FirstRun", "True");
			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + startScore);
		}*/
        if (PlayerPrefs.GetInt("FirstPlay") == 0)
        {
            PlayerPrefs.SetInt("FirstPlay", 1);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + startScore);
        }
        if (CoinsTXT != null)
        {
            CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
        }
        CashText.text = PlayerPrefs.GetInt("Coins").ToString();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPrefs.DeleteAll();
            CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
#if UNITY_EDITOR
            Debug.Log("PlayerPrefs.DeleteAll");
#endif

        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + startScore);
            CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
#if UNITY_EDITOR
            Debug.Log(PlayerPrefs.GetInt("Coins").ToString());
#endif

        }
    }

    public void OpenGooglePlay(string packageName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL("market://details?id=" + packageName);
        }
        else
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + packageName);
        }
    }

    public void RateAPP(string packageName)
    {
        OpenGooglePlay(packageName);
    }

    public void SetTrue(GameObject target)
    {
        target.SetActive(true);
    }
    public void SetFalse(GameObject target)
    {
        target.SetActive(false);
    }
    public void ToggleObject(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    public void LoadLevelAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void cashTextSet()
    {
        CashText.text = PlayerPrefs.GetInt("Coins").ToString();
    }
}

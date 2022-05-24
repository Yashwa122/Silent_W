using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    // Ressources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public FloatingTextManager floatingTextManager;

    // Logic
    public int shmekls;
    public int exp;

    //Floating text
    public void ShowText(string message, int fontSize, Color colour, Vector3 position, Vector3 motion, float duration) 
    {
        floatingTextManager.Show(message, fontSize, colour, position, motion, duration);
    }

    public void SaveState()
    {
        string save = "";

        save += shmekls.ToString() + "|";
        save += exp.ToString() + "|";
        save += "0";

        PlayerPrefs.GetString("SaveState", save);
    }
    public void LoadState(Scene save, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        shmekls = int.Parse(data[1]);
        exp = int.Parse(data[2]);

        Debug.Log("LoadState");
    }
}

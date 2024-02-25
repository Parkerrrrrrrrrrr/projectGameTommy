using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private void Awake()
    { if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(door);

            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deatMenuAnim;
    public GameObject hud;
    public GameObject menu;
    public DigitalDisplay door;
    public MainMenu exit;
   
    //Logic
    public int pesos;
    public int experience;

   

    //Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);

    }

    //Upgarde Weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLavel)
            return false;
        if (pesos >= weaponPrices[weapon.weaponLavel])
        {
            pesos -= weaponPrices[weapon.weaponLavel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1,ratio,1);
    }

    // Expreicnce System
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;
            if (r == xpTable.Count) // Max Level
                return r;

        }
        return r;
    
    }
    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if(currLevel < GetCurrentLevel())
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        Debug.Log("Level Up!");
        player.OnLevelUp();
        OnHitpointChange();
    }

    //On Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawPoint").transform.position;
        
        
    }

    //Death Menu and Respawn
    public void Respawn()
    {
        deatMenuAnim.SetTrigger("Hide");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        player.Respawn();
        //Application.Quit();
        player.transform.position = GameObject.Find("SpawPoint").transform.position;
        SaveState();
    }

    //Save state
    /*
     * INT preferedskin
     * INT pesos
     * INT expreience
     * INT weaponLevel
     */
    public void SaveState()
    {
        
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLavel.ToString();
     

        PlayerPrefs.SetString(Web.ServerResponse, s); // SaveState
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (!PlayerPrefs.HasKey(Web.ServerResponse))
            return;

        string[] data = PlayerPrefs.GetString(Web.ServerResponse).Split('|');

        //Change player skin
        pesos = int.Parse(data[1]);

        // Experienc 
        experience = int.Parse(data[2]);
        if(GetCurrentLevel()!=1)
            player.SetLevel(GetCurrentLevel());

        // Change the weapon Level
        weapon.SetWeaponLevel(int.Parse(data[3]));

    }




}



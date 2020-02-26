using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Win,
    Lose,
    Play
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    [SerializeField]
    Player playerPrefab;
    [SerializeField]
    private Enemy sliderPrefab;
    [SerializeField]
    private Enemy flyerPrefab;
    [SerializeField]
    private Enemy bossPrefab;
    [SerializeField]
    private GameObject[] spawnPos;
    [SerializeField]
    private GameObject loseWindow;
    [SerializeField]
    private GameObject winWindow;
    [SerializeField]
    private GameObject enemysContainer;
    Scene activeScene;
    //private Enemy[] spawnedEnemies;
    private List<Enemy> spawnedEnemies;
    private SaveHandler saveHandler;

    private void Awake()
    {
        instance = this;


    }

    // Start is called before the first frame update
    private void Start()
    {
        saveHandler = GetComponent<SaveHandler>();
        activeScene = SceneManager.GetActiveScene();
        spawnedEnemies = /*new Enemy[5]*/ new List<Enemy>();
        gameState = GameState.Play;
        EnemySpawner();
        saveHandler.Load();
        saveHandler.Save();
        Player.instance.healthBar.SetMaxHealth(Player.instance.playerHP);
    }

    private void Update()
    {
        if (gameState != GameState.Play && gameState == GameState.Lose)
        {
            LoseState();
        }
        else
        {
            WinState();
        }
    }

    private void EnemySpawner()
    {
        if (activeScene == SceneManager.GetSceneByBuildIndex(0))
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 randomSpawnPos = spawnPos[Random.Range(0, spawnPos.Length)].transform.position;
                Enemy spawnedEnemy = Instantiate(sliderPrefab, randomSpawnPos, Quaternion.identity, enemysContainer.transform);
                spawnedEnemies.Add(spawnedEnemy);
                spawnedEnemies[i].enemyType = sliderPrefab.name;
            }
            for (int j = 3; j < 5; j++)
            {
                Vector2 randomSpawnPos = spawnPos[Random.Range(0, spawnPos.Length)].transform.position;
                Enemy spawnedEnemy = Instantiate(flyerPrefab, randomSpawnPos, Quaternion.identity, enemysContainer.transform);
                spawnedEnemies.Add(spawnedEnemy);
                spawnedEnemies[j].enemyType = flyerPrefab.name;
            }
        }
        else
        {
            Enemy boss = Instantiate(bossPrefab, enemysContainer.transform);
            spawnedEnemies.Add(boss);
            boss.enemyType = bossPrefab.name;
        }
    }

    public void SetPlayerParameters(int _playerHP, float _playerSpeed)
    {
        playerPrefab.playerHP = _playerHP;
        playerPrefab.playerSpeed = _playerSpeed;
    }

    public void SetEnemiesParameters(int _slyderHealth, int _flyerHealth, int _bossHealth, float _slyderSpeed, float _flyerSpeed, float _bossSpeed)
    {
        foreach (var item in spawnedEnemies)
        {
            if (item.enemyType.Equals("Flyer"))
            {
                item.enemyHP = _flyerHealth;
                item.enemySpeed = _flyerSpeed;
            }
            if (item.enemyType.Equals("Slider"))
            {
                item.enemyHP = _slyderHealth;
                item.enemySpeed = _slyderSpeed;
            }
            if (item.enemyType.Equals("Boss"))
            {
                item.enemyHP = _bossHealth;
                item.enemySpeed = _bossSpeed;
            }
        }
    }

    private void WinState()
    {
        if (IsArrayEmpty() && activeScene == SceneManager.GetSceneByBuildIndex(0))
        {
            gameState = GameState.Win;
            spawnedEnemies.Clear();
            StartCoroutine("NewSceneLoad");
        }
        else if (IsArrayEmpty() && activeScene == SceneManager.GetSceneByBuildIndex(1))
        {
            gameState = GameState.Win;
            spawnedEnemies.Clear();
            StartCoroutine("WinGameCo");
        }
    }

    void LoseState()
    {
        if (gameState == GameState.Lose)
        {
            StartCoroutine("LoseGameCo");
        }
    }

    private IEnumerator NewSceneLoad()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

    IEnumerator LoseGameCo()
    {
        yield return new WaitForSeconds(.5f);
        loseWindow.SetActive(true);
    }

    IEnumerator WinGameCo()
    {
        yield return new WaitForSeconds(2f);
        winWindow.SetActive(true);
    }

    private bool IsArrayEmpty()
    {
        return spawnedEnemies.TrueForAll(x => x == null);
    }

    public void ReatartButton()
    {
        SceneManager.LoadScene(0);
    }
}

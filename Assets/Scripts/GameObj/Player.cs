using UnityEngine;

public enum PlayerState
{
    Move,
    Stand
}

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerState playerState;
    private Animator playerAnim;
    [SerializeField]
    private Shell shell;
    private bool callInvoke = true;
    private GameObject playerShells;
    public int playerHP;
    public float playerSpeed;
    public HealthBar healthBar;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        playerAnim = GetComponent<Animator>();
        playerShells = GameObject.FindGameObjectWithTag("Player shells");
    }

    private void Start()
    {
        playerState = PlayerState.Stand;
        PlayerBehavior();
    }

    private void Update()
    {
        PlayerBehavior();
    }

    public void PlayerBehavior()
    {
        if (GameManager.instance.gameState != GameState.Lose)
        {
            if (playerState == PlayerState.Stand)
            {
                playerAnim.Play("Player_Idle");
                playerAnim.SetBool("isMoving", false);
                if (callInvoke)
                {
                    InvokeRepeating("PlayerShooting", .3f, 1f);
                    callInvoke = false;
                }
            }
            else
            {
                playerAnim.Play("Player_Move");
                playerAnim.SetBool("isMoving", true);
                CancelInvoke("PlayerShooting");
                callInvoke = true;
            }
            if (playerHP <= 0)
            {
                GameManager.instance.gameState = GameState.Lose;
                gameObject.SetActive(false);
                CancelInvoke("PlayerShooting");
            }
        }
    }

    private void PlayerShooting()
    {
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            Shell castedShell = Instantiate(shell, gameObject.transform.position, Quaternion.identity, playerShells.transform);
            castedShell.casterType = gameObject.tag;
        }
    }
}

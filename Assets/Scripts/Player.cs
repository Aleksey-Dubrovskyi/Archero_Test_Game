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
    GameObject playerShells;

    // Start is called before the first frame update
    private void Start()
    {
        
        instance = this;
        playerState = PlayerState.Stand;
        playerAnim = GetComponent<Animator>();
        PlayerBehavior();
        playerShells = GameObject.FindGameObjectWithTag("Player shells");
    }

    private void Update()
    {
        PlayerBehavior();
    }

    public void PlayerBehavior()
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
    }

    private void PlayerShooting()
    { 
        Shell castedShell =  Instantiate(shell, gameObject.transform.position, Quaternion.identity, playerShells.transform);
        castedShell.casterType = gameObject.tag;
    }
}

using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Unit unitScript;
    private int enemyDamage;
    public int enemyHP = 10;
    public float enemySpeed = 3;
    private float angleDir = 0f;
    public string enemyType;
    private bool rootIsFinished;
    private Animator enemyAnim;
    private GameObject enemyShells;
    [SerializeField]
    private Shell shell;
    public HealthBar healthBar;


    // Start is called before the first frame update
    private void Start()
    {
        unitScript = GetComponent<Unit>();
        enemyAnim = GetComponent<Animator>();
        enemyShells = GameObject.FindGameObjectWithTag("Enemy shells");
        healthBar.SetMaxHealth(enemyHP);
        switch (enemyType)
        {
            case "Flyer":
                FlyerBehaviour();
                break;
            case "Slider":
                SlyderBehavoiur();
                break;
            case "Boss":
                BossBehaviour();
                break;
            default:
                Debug.Log("There is no such enemy");
                break;
        }
    }

    private void Update()
    {
        if (enemyType != "Slider")
        {
            EnemyAmitaion();
        }
        if (enemyHP <= 0)
        {
            EnemyDestroy();
        }
    }

    private void EnemyAmitaion()
    {
        if (DOTween.IsTweening(transform))
        {
            SelectAnim();
        }
        else
        {
            if (unitScript != null)
            {
                if (unitScript.target != null)
                {
                    AngleDirCalc(unitScript.target.position);
                    SelectAnim();
                }
            }
        }

    }

    void SelectAnim()
    {
        if (angleDir > -45 && angleDir <= 45)
        {
            enemyAnim.Play("Enemy_Move_Right");
        }
        else if (angleDir > 135 || angleDir <= -135)
        {
            enemyAnim.Play("Enemy_Move_Left");
        }
        else if (angleDir > 45 && angleDir <= 135)
        {
            enemyAnim.Play("Enemy_Move_Backward");
        }
        else if (angleDir < -45 && angleDir >= -135)
        {
            enemyAnim.Play("Enemy_Move_Forward");
        }
    }

    #region Flyer section

    private void FlyerBehaviour()
    {
        StartCoroutine("FlyerBehaviourCo");
    }

    private IEnumerator FlyerBehaviourCo()
    {
        while (enemyHP > 0 && GameManager.instance.gameState != GameState.Lose)
        {
            StartCoroutine(EnemyRandomDestination(enemySpeed));
            yield return new WaitWhile(() => rootIsFinished != true);
            yield return new WaitForSeconds(1f);
            Shell castedShell = Instantiate(shell, gameObject.transform.position, Quaternion.identity, enemyShells.transform);
            castedShell.casterType = gameObject.tag;
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region Slyder section

    private void SlyderBehavoiur()
    {
        if (GameManager.instance.gameState != GameState.Lose)
        {
            if (enemyHP > 0)
            {
                unitScript.target = GameObject.FindGameObjectWithTag("Player").transform;
                unitScript.speed = enemySpeed;
            }
        }
    }

    #endregion

    #region BossBehaviour

    private void BossBehaviour()
    {
        if (GameManager.instance.gameState != GameState.Lose)
        {
            if (enemyHP > 0)
            {
                string[] bossStates = { "State1", "State2", "State3" };
                string bossCurrentState = bossStates[Random.Range(0, bossStates.Length)];
                switch (bossCurrentState)
                {
                    case "State1":
                        StartCoroutine("StateOneCo");
                        break;
                    case "State2":
                        StartCoroutine("StateTwoCo");
                        break;
                    case "State3":
                        StartCoroutine("StateThreeCo");
                        break;
                    default:
                        Debug.LogError("State error! Check switch statement");
                        break;
                }
            }
        }
    }

    private IEnumerator StateOneCo()
    {
        unitScript.target = GameObject.FindGameObjectWithTag("Player").transform;
        unitScript.speed = enemySpeed * 2;
        yield return new WaitForSeconds(2f);
        unitScript.speed = enemySpeed;
        unitScript.target = null;
        yield return new WaitForSeconds(.5f);
        BossBehaviour();
    }

    private IEnumerator StateTwoCo()
    {
        int shellQue = 0;
        while (shellQue < 3)
        {
            Shell castedShell = Instantiate(shell, gameObject.transform.position, Quaternion.identity, enemyShells.transform);
            castedShell.offsetRotation = 0;
            castedShell.casterType = gameObject.tag;
            shellQue++;
            yield return new WaitForSeconds(.2f);
        }

        StartCoroutine(EnemyRandomDestination(enemySpeed));
        yield return new WaitWhile(() => rootIsFinished != true);
        yield return new WaitForSeconds(.5f);
        BossBehaviour();
    }

    private IEnumerator StateThreeCo()
    {
        Shell[] castedShells = new Shell[3];
        float[] shellOffsetAngle = { -30f, 0f, 30f };
        for (int i = 0; i < castedShells.Length; i++)
        {
            castedShells[i] = shell;
            castedShells[i].casterType = gameObject.tag;
            castedShells[i] = Instantiate(castedShells[i], gameObject.transform.position, Quaternion.identity, enemyShells.transform);
            castedShells[i].offsetRotation = shellOffsetAngle[i];
            castedShells[i].bossStateThree = true;
        }
        yield return new WaitForSeconds(1f);
        BossBehaviour();
    }

    #endregion

    private IEnumerator EnemyRandomDestination(float enemySpeed)
    {
        Vector2 EnemyChosedPos = Vector2.zero;
        Vector2 screenBounds;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        EnemyChosedPos.x = Random.Range(-screenBounds.x, screenBounds.x);
        EnemyChosedPos.y = Random.Range(-screenBounds.y, screenBounds.y);
        AngleDirCalc(EnemyChosedPos);
        rootIsFinished = false;
        transform.DOMove(EnemyChosedPos, enemySpeed).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() => rootIsFinished = true);
        yield return null;
    }

    private void AngleDirCalc(Vector2 gameObjectPos)
    {
        angleDir = Mathf.Atan2(gameObjectPos.y - transform.position.y, gameObjectPos.x - transform.position.x) * 180 / Mathf.PI;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            StartCoroutine(MeleDamageCo(player));
        }
    }

    private IEnumerator MeleDamageCo(Player player)
    {
        DamageManager.instance.TakeMeleDamage(player.playerHP);
        player.playerHP = DamageManager.instance.UpdateHpInfo();
        player.healthBar.SetHealth(player.playerHP);
        yield return new WaitForSeconds(1f);
    }

    private void EnemyDestroy()
    {
        Destroy(this.gameObject);
    }
}

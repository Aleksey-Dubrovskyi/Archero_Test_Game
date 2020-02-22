using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private Unit unitScript;
    private int enemyDamage;
    private int enemyHP = 10;
    private float enemySpeed = 3;
    float angleDir = 0f;
    private string enemyType;
    Animator enemyAnim;
    private GameObject enemyShells;
    [SerializeField]
    private Shell shell;


    // Start is called before the first frame update
    private void Start()
    {
        unitScript = GetComponent<Unit>();
        enemyAnim = GetComponent<Animator>();
        enemyShells = GameObject.FindGameObjectWithTag("Enemy shells");
        enemyType = gameObject.name;
        switch (enemyType)
        {
            case "Flyer":
                FlyerBehaviour();
                break;
            case "Slider":
                break;
            case "Boss":
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (enemyType != "Slider")
        {
            EnemyAmitaion();
        }
    }

    void EnemyAmitaion()
    {
        if (DOTween.IsTweening(transform))
        {            
            if (angleDir > -45 && angleDir <= 45)
            {
                //enemyAnim.SetTrigger("Go_Right");
                enemyAnim.Play("Enemy_Move_Right");
            }
            else if (angleDir > 135 || angleDir <= -135)
            {
                //enemyAnim.SetTrigger("Go_Left");
                enemyAnim.Play("Enemy_Move_Left");
            }
            else if (angleDir > 45 && angleDir <= 135)
            {
                //enemyAnim.SetTrigger("Go_Back");
                enemyAnim.Play("Enemy_Move_Backward");
            }
            else if (angleDir < -45 && angleDir >= -135)
            {
                enemyAnim.Play("Enemy_Move_Forward");
                //enemyAnim.SetTrigger("Go_Forward");
            }
        }
    }

    private void FlyerBehaviour()
    {
        StartCoroutine("FlyerBehaviourCo");
    }

    private IEnumerator FlyerBehaviourCo()
    {
        while (enemyHP > 0)
        {
            Vector2 flyerChosedDestination = Vector2.zero;
            Vector2 screenBounds;
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            flyerChosedDestination.x = Random.Range(-screenBounds.x, screenBounds.x);
            flyerChosedDestination.y = Random.Range(-screenBounds.y, screenBounds.y);
            angleDir = Mathf.Atan2(flyerChosedDestination.y - transform.position.y, flyerChosedDestination.x - transform.position.x) * 180 / Mathf.PI;
            bool rootIsFinished = false;
            transform.DOMove(flyerChosedDestination, enemySpeed).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() => rootIsFinished = true);
            yield return new WaitWhile(() => rootIsFinished != true);
            yield return new WaitForSeconds(1f);
            Shell castedShell = Instantiate(shell, gameObject.transform.position, Quaternion.identity, enemyShells.transform);
            castedShell.casterType = gameObject.tag;
            yield return new WaitForSeconds(1f);
        }
    }


}

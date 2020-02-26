using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    //Set this to true state when we are touching the screen
    private bool touchStart = false;
    //A place of our first touch
    private Vector2 pointA;
    //A place of our swipe
    private Vector2 pointB;

    [SerializeField]
    private Transform circle;
    [SerializeField]
    private Transform outerCircle;

    private void Update()
    {
        if (GameManager.instance.gameState == GameState.Play)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

                circle.transform.position = pointA;
                outerCircle.transform.position = pointA;
                circle.GetComponent<SpriteRenderer>().enabled = true;
                outerCircle.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (Input.GetMouseButton(0))
            {
                touchStart = true;
                pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            }
            else
            {
                touchStart = false;
            }
        }
        else
        {
            touchStart = false;
        }

    }
    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            moveCharacter(direction);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);
        }
        else
        {
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
            Player.instance.playerState = PlayerState.Stand;
        }
        if (pointB.x >= 0.01f)
        {
            player.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (pointB.x <= -0.01f)
        {
            player.localScale = new Vector3(-1f, 1f, 1f);
        }

    }

    private void moveCharacter(Vector2 direction)
    {
        player.Translate(direction * Player.instance.playerSpeed * Time.deltaTime);
        Player.instance.playerState = PlayerState.Move;
    }
}
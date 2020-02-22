using UnityEngine;

public class Boundaries : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;
    Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;


    private void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }


    private void LateUpdate()
    {
        Vector3 objectPos = transform.position;
        objectPos.x = Mathf.Clamp(objectPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        objectPos.y = Mathf.Clamp(objectPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = objectPos;
    }
}
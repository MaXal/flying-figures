using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Sprite> shapes;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 1f;

    private int currentShapeIndex;
    private float xMax;
    private float xMin;
    private float yMax;
    private float yMin;

    private void Start()
    {
        SetUpMoveBoundaries();
    }

    private void Update()
    {
        Move();
        ChangeShape();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }

    private void ChangeShape()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<SpriteRenderer>().sprite = shapes[currentShapeIndex];
            currentShapeIndex++;
            if (currentShapeIndex >= shapes.Count) currentShapeIndex = 0;
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.smoothDeltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.smoothDeltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        var gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
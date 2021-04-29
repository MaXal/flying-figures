using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 1f;
    [SerializeField] private ColorManager colorManager;

    private Color color;

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
        if (other.gameObject.CompareTag("Tile") && other.gameObject.GetComponent<Tile>().Color != color)
            DestroyPlayer();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tile")) OnPlayerPassedWall?.Invoke();
    }

    public event Action OnPlayerPassedWall;

    public static event Action OnPlayerDestroy;

    private void DestroyPlayer()
    {
        OnPlayerDestroy?.Invoke();
        Destroy(gameObject);
    }

    private void ChangeShape()
    {
        if (!Input.GetButtonDown("Jump")) return;
        currentShapeIndex++;
        if (currentShapeIndex >= colorManager.GetNumberOfPlayerColors()) currentShapeIndex = 0;
        GetComponent<SpriteRenderer>().sprite = colorManager.GetPlayerSprite(currentShapeIndex);
        color = ColorManager.GetColorByIndex(currentShapeIndex);
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.smoothDeltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.smoothDeltaTime * moveSpeed;

        var position = transform.position;
        var newXPos = Mathf.Clamp(position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        var gameCamera = Camera.main;
        if (gameCamera == null) return;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
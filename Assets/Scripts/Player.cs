using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 0.7f;
    [SerializeField] private float paddingInsideTile = 0.1f;
    [SerializeField] private SpriteManager spriteManager;

    private Color color;

    private int currentShapeIndex = -1;

    private int enteredIntoTiles;
    private float xMax;
    private float xMin;
    private float yMax;
    private float yMin;

    private void Start()
    {
        SetUpWorldMoveBoundaries();
    }

    private void Update()
    {
        Move();
        ChangeShape();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tile") && other.gameObject.GetComponent<Tile>().Color != color)
        {
            DestroyPlayer();
        }
        else
        {
            enteredIntoTiles++;
            SetUpTileBoundaries(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Tile")) return;
        SetUpWorldMoveBoundaries();
        enteredIntoTiles--;
        if (enteredIntoTiles == 0) OnPlayerPassedWall?.Invoke();
    }

    private void SetUpTileBoundaries(GameObject tile)
    {
        var position = tile.transform.position;
        yMin = position.y - paddingInsideTile;
        yMax = position.y + paddingInsideTile;
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
        if (currentShapeIndex >= spriteManager.GetNumberOfPlayerColors()) currentShapeIndex = 0;
        GetComponent<SpriteRenderer>().sprite = spriteManager.GetPlayerSprite(currentShapeIndex);
        color = (Color) currentShapeIndex;
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

    private void SetUpWorldMoveBoundaries()
    {
        var gameCamera = Camera.main;
        if (gameCamera == null) return;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 2 * padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Color color = Color.Green;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 0.7f;
    [SerializeField] private float paddingInsideTile = 0.1f;
    [SerializeField] private SpriteManager spriteManager;
    [SerializeField] private float delayForWallDestruction = 0.1f;


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
        if (GameManager.GameState != GameState.Play) return;

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
        StartCoroutine(DestroyWall(other.gameObject.transform.parent.gameObject));
    }

    private IEnumerator DestroyWall(GameObject wall)
    {
        yield return new WaitForSeconds(delayForWallDestruction);
        Destroy(wall);
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
        GameManager.GameState = GameState.GameOver;
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
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

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
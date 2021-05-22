using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 0.7f;
    [SerializeField] private float paddingInsideTile = 0.1f;
    [SerializeField] private SpriteManager spriteManager;
    [SerializeField] private int initLifeCount = 3;

    private bool invincible;
    private float xMax;
    private float xMin;
    private float yMax;
    private float yMin;
    private SpriteRenderer spriteRenderer;

    public static int Life { get; private set; }

    public static event Action<GameObject> OnPlayerPassedWall;
    public static event Action OnPlayerLostLife;
    public static event Action OnPlayerDestroy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Life = initLifeCount;
    }

    private void Start()
    {
        SetColor(color);
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
        if (!other.gameObject.CompareTag("Tile")) return;
        if (other.gameObject.GetComponent<Tile>().Color == color) return;
        if (invincible) return;
            
        Life--;
        StartCoroutine(TemporaryInvincible());
        OnPlayerLostLife?.Invoke();
        if (Life <= 0) DestroyPlayer();
    }

    private IEnumerator TemporaryInvincible()
    {
        invincible = true;
        
        yield return new WaitForSecondsRealtime(2);

        invincible = false;
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Tile")) return;
        
        SetUpWorldMoveBoundaries();
        
        if (!(transform.position.x > other.transform.position.x) ||
            other.GetComponentInParent<Wall>().PassedByPlayer) return;
        
        var wall = other.transform.parent.gameObject;
        OnPlayerPassedWall?.Invoke(wall);
    }

    private void DestroyPlayer()
    {
        GameManager.GameState = GameState.GameOver;
        OnPlayerDestroy?.Invoke();
        Destroy(gameObject);
    }

    private void ChangeShape()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SetColor(Color.Blue);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            SetColor(Color.Green);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SetColor(Color.Yellow);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SetColor(Color.Red);
        }
    }

    private void SetColor(Color c)
    {
        color = c;
        spriteRenderer.sprite = spriteManager.GetPlayerSprite(color);
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
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private List<Vector3> _spawnPos;

    [HideInInspector]
    public int SpriteId;

    [HideInInspector]
    public Score NextScore;
    // Create an instance of the GameplayManager class
    GameplayManager gameplayManager;
    GameObject gameplayManagerObject;

    private static Object pivotLock = new Object();
    private static bool pivot = false;
    private int direction;

    private static Object stackLock = new Object();
    private static int gameLevel = 0;
    private static Stack<int> randomStack;

    private void Awake()
    {
        lock (pivotLock)
        {
            pivot = !pivot;
            direction = pivot ? 1 : -1;
        }

        lock (stackLock)
        {
            // get component GameplayManager from object GameplayManager
            gameplayManagerObject = GameObject.Find("GameplayManager");
            gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();
            if (gameLevel < gameplayManager.game_level || randomStack.Count == 0)
            {
                if (gameLevel < gameplayManager.game_level) gameLevel++;
                randomStack = new Stack<int>();
                int SpriteCount = Mathf.Min(GameplayManager.Instance.Sprites.Count - 20 + gameplayManager.game_level, GameplayManager.Instance.Sprites.Count);
                while (randomStack.Count < SpriteCount)
                {
                    int num = Random.Range(0, SpriteCount);
                    if (!randomStack.Contains(num)) randomStack.Push(num);
                }
            }

            SpriteId = randomStack.Pop();
        }
        // _moveSpeed = random 2 5
        _moveSpeed = Random.Range(1.5f, 5f);
        hasGameFinished = false;
        transform.position = new Vector3(direction * (-48.1f), Random.Range(-7f, 18f), 0);
        // scale object random -game_level * 0.1f,0
        // transform.localScale += new Vector3(Random.Range(-game_level * 0.2f, 0), Random.Range(-game_level * 0.2f, 0), 0);
        // player random rotation x axis game_level * 5
        // player.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, game_level * 10));
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, Random.Range(0.5f, 1f));
        var sizeModifier = Random.Range(0.3f, 0.8f);
        GetComponent<SpriteRenderer>().size *= sizeModifier;
        GetComponent<BoxCollider2D>().size *= sizeModifier;
    }

    private void FixedUpdate()
    {
        if (hasGameFinished || GameplayManager.Instance.isPaused) return;
        transform.Translate(direction * _moveSpeed * Time.fixedDeltaTime * Vector3.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            GameplayManager.Instance.GameEnded();
        }
    }

    private void OnEnable()
    {
        GameplayManager.Instance.GameEnd += GameEnded;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.GameEnd -= GameEnded;
    }

    private bool hasGameFinished;

    private void GameEnded()
    {
        hasGameFinished = true;
    }
}

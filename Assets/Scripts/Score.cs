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

    private void Awake()
    {
        // _moveSpeed = random 2 5
        _moveSpeed = Random.Range(1.5f, 4.5f);
        hasGameFinished = false;
        transform.position = new Vector3(-48.1f, Random.Range(-14f, 18f), Random.Range(0, _spawnPos.Count));
        int SpriteCount = GameplayManager.Instance.Sprites.Count;
        SpriteId = Random.Range(0, SpriteCount);
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }

    private void FixedUpdate()
    {
        if (hasGameFinished) return;
        transform.Translate(_moveSpeed * Time.fixedDeltaTime * Vector3.right);
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

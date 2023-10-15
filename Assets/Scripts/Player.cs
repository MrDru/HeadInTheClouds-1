using UnityEngine;

public class Player : MonoBehaviour
{
    public int SpriteId;
    GameplayManager gameplayManager;

    private void Awake()
    {
        gameplayManager = new GameplayManager();
        int SpriteCount = Mathf.Min(GameplayManager.Instance.Sprites.Count - 21 + gameplayManager.game_level, GameplayManager.Instance.Sprites.Count);
        SpriteId = Random.Range(0, SpriteCount);
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }
}

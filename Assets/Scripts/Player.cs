using UnityEngine;

public class Player : MonoBehaviour
{
    public int SpriteId;
    GameplayManager gameplayManager = new GameplayManager();

    private void Awake()
    {
        int SpriteCount = GameplayManager.Instance.Sprites.Count - 10 + gameplayManager.game_level;
        SpriteId = Random.Range(1, SpriteCount);
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }
}

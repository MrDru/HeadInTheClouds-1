using UnityEngine;

public class Player : MonoBehaviour
{
    public int SpriteId;
    public int num_sprites = 23;
    private void Awake()
    {
        SpriteId = Random.Range(1, num_sprites);
        Debug.Log("SpriteId: " + SpriteId);
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }
}

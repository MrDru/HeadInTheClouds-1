using System.Collections;
using UnityEngine;

public class ScoreEffect : MonoBehaviour
{
    //[SerializeField] private float _destroyTime;
    //private Sprite currentSprite;

    public void Init(Sprite col)
    {
      //  currentSprite = col;

        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {

        //    float timeElapsed = 0f;
        //    float speed = 1 / _destroyTime;
        //    Vector3 startScale = Vector3.one * 0.64f;
        //    Vector3 endScale = Vector3.one * 1.32f;
        //    Vector3 scaleOffset = endScale - startScale;
        //    Vector3 currentScale = startScale;

        //    Sprite startSprite = currentSprite;
        //    startSprite.a = 0.8f;
        //    Sprite endSprite = currentSprite;
        //    endSprite.a = 0.2f;
        //    Sprite SpriteOffset = endSprite - startSprite;
        //    Sprite c = startSprite;
        //    SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //    sr.Sprite = c;

        //    while (timeElapsed < 1f)
        //    {
        //        timeElapsed += speed * Time.deltaTime;

        //        currentScale = startScale + timeElapsed * scaleOffset;
        //        transform.localScale = currentScale;

        //        c = startSprite + timeElapsed * SpriteOffset;
        //        sr.Sprite = c;

        //        yield return null;
        //    }
        yield return null;
        Destroy(gameObject);
    }
}

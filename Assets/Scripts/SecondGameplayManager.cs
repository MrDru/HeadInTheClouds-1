using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SecondGameplayManager : MonoBehaviour
{
    [Header ("Sprites Handeler")]
    public List<Sprite> theCorrectTrioSprites;

    [SerializeField] private Image spriteToGuess1;

    [SerializeField] private Image spriteToGuess2;

    [SerializeField] private Image spriteToGuess3;

    [SerializeField] private int correctCounter = 0;

    public void Generate3NewCorrectTrio()
    {
        theCorrectTrioSprites.Clear();
        int  randNum = Random.Range(0, TheListOfAllSprites.theStaticListOFTheSprites.Count-1);
        theCorrectTrioSprites.Add(TheListOfAllSprites.theStaticListOFTheSprites[randNum]);
        spriteToGuess1.sprite = theCorrectTrioSprites[0];

        randNum = Random.Range(0, TheListOfAllSprites.theStaticListOFTheSprites.Count-1 );
        theCorrectTrioSprites.Add(TheListOfAllSprites.theStaticListOFTheSprites[randNum]);
        spriteToGuess2.sprite = theCorrectTrioSprites[1];

        randNum = Random.Range(0, TheListOfAllSprites.theStaticListOFTheSprites.Count-1);
        theCorrectTrioSprites.Add(TheListOfAllSprites.theStaticListOFTheSprites[randNum]);
        spriteToGuess3.sprite = theCorrectTrioSprites[2];


    }

    // Start is called before the first frame update
    void Start()
    {
        theCorrectTrioSprites = new List<Sprite>(3);
        Generate3NewCorrectTrio();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider)
            {

                var iscorrect = CheckIfClickedCorrect(hit.collider.gameObject);
                if(iscorrect)
                {
                    hit.collider.gameObject.transform.DOScale(1.2f, 1).OnComplete(Dissapear);
                }

            }
        }
    }
    public void Dissapear()
    {

    }
    public bool CheckIfClickedCorrect(GameObject colliderObject)
    {
        foreach(Sprite asptrite in TheListOfAllSprites.theStaticListOFTheSprites)
        {
            if(asptrite == colliderObject.GetComponent<SpriteRenderer>().sprite)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}

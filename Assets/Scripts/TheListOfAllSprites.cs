using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheListOfAllSprites : MonoBehaviour
{
    [SerializeField] public static List<Sprite> theStaticListOFTheSprites;

    [SerializeField] public List<Sprite> editbleSpritesList;

    private  void Start()
    {
        theStaticListOFTheSprites = editbleSpritesList;
        Debug.Log("you got here:"+ theStaticListOFTheSprites [0]);
    }

}

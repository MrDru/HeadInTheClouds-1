using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThelistOfAllSprites : MonoBehaviour
{

    public static ThelistOfAllSprites Instance;
    public static List<Sprite> staticSpriteList;
    public  List<Sprite> editableList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            staticSpriteList = editableList;
            DontDestroyOnLoad(gameObject);

            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    
}

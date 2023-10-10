using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovementManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;

    [SerializeField] private Vector3 finishPos;

    [SerializeField] private float SpriteSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isGamePaused== false)
        {
            transform.Translate(Vector3.right * SpriteSpeed * Time.deltaTime);
            if (transform.position.x >= finishPos.x)
            {
                transform.position = startingPos;
            }

        }
    }
}

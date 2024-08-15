using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer sprite;
    public bool isCorrect;
    public bool isRevealed;
    public bool isPicked;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteColor(Color color)
    {
        sprite.color = color;
    }
}

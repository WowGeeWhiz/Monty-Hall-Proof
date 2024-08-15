using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    Door[] doors;
    public Color hiddenColor;
    public Color pickedColor;
    public Color wrongColor;
    public Color correctColor;
    System.Random rand;
    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        doors = GetComponentsInChildren<Door>();
    }

    public void ResetDoors()
    {
        int correct = rand.Next(0, doors.Length);
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetSpriteColor(hiddenColor);
            if (i == correct) doors[i].isCorrect = true;
            else doors[i].isCorrect = false;
        }
    }
}

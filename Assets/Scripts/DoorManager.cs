using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DoorManager : MonoBehaviour
{
    Door[] doors;
    public Color hiddenColor;
    public Color pickedColor;
    public Color wrongColor;
    public Color correctColor;
    System.Random rand;
    public TextMeshProUGUI swapStat;
    public TextMeshProUGUI stickStat;
    public TextMeshProUGUI swapDisplay;
    int pickedDoor;
    int swappedCorrect;
    int stickedCorrect;
    int swappedTestMax;
    int stickedTestMax;
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
            doors[i].isPicked = doors[i].isRevealed = false;
            if (i == correct) doors[i].isCorrect = true;
            else doors[i].isCorrect = false;    
        }
    }

    public void StartTest()
    {
        swappedCorrect = 0;
        stickedCorrect = 0;
        swappedTestMax = 0;
        stickedTestMax = 0;
        for (int i = 0; i < 500; i++)
        {
            TestCycle(false);
            TestCycle(true);
        }
    }

    public void TestCycle(bool swap)
    {
        ResetDoors();
        pickedDoor = rand.Next(0, doors.Length);
        doors[pickedDoor].SetSpriteColor(pickedColor);
        RevealDoor();
        if (swap)
        {
            swappedTestMax++;
            SwapDoor();
            if (doors[pickedDoor].isCorrect)
            {
                swappedCorrect++;
                swapDisplay.text += "\nYou picked right!";
            }
            else swapDisplay.text += "\n You picked wrong!";

        }
        else
        {
            stickedTestMax++;
            swapDisplay.text = $"You did not swap from door {pickedDoor + 1}";
            if (doors[pickedDoor].isCorrect)
            {
                stickedCorrect++;
                swapDisplay.text += "\nYou picked right!";
            }
            else swapDisplay.text += "\n You picked wrong!";
        }
        RevealCorrect();
        UpdateDisplays();
    }
    
    void UpdateDisplays()
    {
        swapStat.text = $"{swappedCorrect}/{swappedTestMax}";
        stickStat.text = $"{stickedCorrect}/{stickedTestMax}";
    }

    void RevealCorrect()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].isCorrect)
            {
                doors[i].SetSpriteColor(correctColor);
                break;
            }
        }
    }

    void RevealDoor()
    {
        Door toReveal;
        do
        {
            toReveal = doors[rand.Next(0, doors.Length)];
        } while (toReveal.isCorrect || toReveal.isPicked);
        toReveal.SetSpriteColor(wrongColor);
        toReveal.isRevealed = true;
    }

    void SwapDoor()
    {
        int oldPick = pickedDoor;
        do
        {
            pickedDoor = rand.Next(0, doors.Length);
        } while (doors[pickedDoor].isPicked || doors[pickedDoor].isRevealed);
        doors[oldPick].isPicked = false;
        doors[oldPick].SetSpriteColor(hiddenColor);
        doors[pickedDoor].isPicked = true;
        doors[pickedDoor].SetSpriteColor(pickedColor);
        swapDisplay.text = $"You swapped from door  {oldPick + 1} to door {pickedDoor + 1}";
    }
}

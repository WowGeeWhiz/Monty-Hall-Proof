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
    public TextMeshProUGUI oddsDisplay;
    public TextMeshProUGUI proofStatement;
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

    public void StartTest(int testTimes = 500)
    {
        swappedCorrect = 0;
        stickedCorrect = 0;
        swappedTestMax = 0;
        stickedTestMax = 0;
        for (int i = 0; i < testTimes; i++)
        {
            TestCycle(false);
            TestCycle(true);
        }
        proofStatement.gameObject.SetActive(true);
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
        float swapFloat;
        if (swappedTestMax != 0) swapFloat = (float)swappedCorrect / (float)swappedTestMax;
        else swapFloat = 0;
        float stickFloat;
        if (stickedTestMax != 0) stickFloat = (float)stickedCorrect / (float)stickedTestMax;
        else stickFloat = 0;
        Debug.Log("before x100: " + swapFloat);
        swapFloat *= 100;
        stickFloat *= 100;
        Debug.Log("before round: " + swapFloat);
        swapFloat = Mathf.Round(swapFloat);
        stickFloat = Mathf.Round(stickFloat);
        Debug.Log("after round: " + swapFloat);
        oddsDisplay.text = $"Success probability: {swapFloat}%\nSuccess probability: {stickFloat}%";
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

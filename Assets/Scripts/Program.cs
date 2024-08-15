/*using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;

namespace MontyHallProof
{
    internal class Program
    {
        // number of doors to choose from
        static int doors;
        // which door is correct
        static int correctDoor;
        // the random value generator
        static Random rand;
        // the chosen door
        static int pickedDoor;
        // the door that has been revealed
        static int revealedDoor;
        // the number of correct tries after a swap
        static int correctTrackerSwap;
        // the number of correct tries without a swap
        static int correctTrackerStick;
        // the number of times a given door was the correct door
        static int[] correctDoorTracker;

        static void Main(string[] args)
        {
            // value to reuse the number of tests from previous run through
            int forceTests = -1;
            // value to stop the program loop
            bool stop = false;
            do
            {
                // run the actual program and store the number of tests run
                int lastTests = Run(forceTests);
                Console.WriteLine("\n\nPress Enter to start over, or enter 'r' to redo previous test, or enter 'q' to quit:");
                // store user input
                string response = Console.ReadLine();
                // quit on q
                if (response == "q") break;
                // reuse test number from last run
                if (response == "r") forceTests = lastTests;
                // otherwise just restart program normally
                else forceTests = -1;
            } while (!stop);

            Console.WriteLine("\n\n\n\n-----------------------------------------------------------\nThanks for testing!\nWritten by Ethan Johnson");
        }

        // actual program
        static int Run(int forceTests = -1)
        {
            // init values
            rand = new Random();
            doors = 3;
            correctDoorTracker = new int[doors];
            correctTrackerStick = correctTrackerSwap = 0;
            int numTestsToRun = 0;

            // if not forcing the same number of tests
            if (forceTests == -1)
            {
                // prepare values for getting test number input
                bool validNum = false;
                string input = "";
                // loop
                do
                {
                    // get input
                    Console.WriteLine("Enter the number of tests to run with swaps and without swaps:");
                    input = Console.ReadLine();
                    // ensure not given something other than a number
                    try
                    {
                        // convert input to int
                        int temp = Convert.ToInt32(input);
                        numTestsToRun = temp;
                        // break loop
                        validNum = true;
                    }
                    catch (Exception ex)
                    {
                        validNum = false;
                    }
                } while (!validNum);
            }
            // or reuse last number
            else numTestsToRun = forceTests; 


            Console.WriteLine("\nTesting with swaps...\n-------------------------------------------------------");
            for (int i = 0; i < numTestsToRun; i++)
            {
                SetCorrectDoor();
                SelectRandDoor();
                DisplaySetup();
                RevealDoor(i + 1, forceSwap: true);
            }

            Console.WriteLine("\nTesting without swaps...\n-------------------------------------------------------");
            for (int i = 0; i < numTestsToRun; i++)
            {
                SetCorrectDoor();
                SelectRandDoor();
                DisplaySetup();
                RevealDoor(i + 1, forceStick: true);
            }

            // display stats for swaps
            DisplayStats(true, numTestsToRun);
            // display stats without swaps
            DisplayStats(false, numTestsToRun);
            // display stats for which door was right door
            DisplayCorrectDoorStats(numTestsToRun * 2);

            return numTestsToRun;
        }

        // pick a random door to be correct
        static void SetCorrectDoor()
        {
            correctDoor = rand.Next(1,doors + 1);
            correctDoorTracker[correctDoor - 1]++;
        }

        // display the setup
        static void DisplaySetup()
        {
            Console.WriteLine($"Number of doors: {doors}\nPicked door: {pickedDoor}");

        }

        // display the stats
        static void DisplayStats(bool didSwap, int numTestsRun)
        {
            Console.WriteLine("------------------------------------------------");
            // display for when there were swaps
            if (didSwap)
            {
                Console.WriteLine($"Total number of tests with swaps: {numTestsRun}");
                Console.WriteLine($"Times correct: {correctTrackerSwap}/{numTestsRun}");
            }
            // display for when there were no swaps
            else
            {
                Console.WriteLine($"Total number of tests without swaps: {numTestsRun}");
                Console.WriteLine($"Times correct: {correctTrackerStick}/{numTestsRun}");
            }

        }

        // display how many times each door was actually the correct door
        static void DisplayCorrectDoorStats(int numTestsRun)
        {
            Console.WriteLine("\nCorrect door stats from all tests:");
            for (int i = 0; i < doors; i++)
            {
                Console.WriteLine($"Door {i + 1}: {correctDoorTracker[i]}/{numTestsRun}");
            }
        }

        // choose a random door for initial selection
        static void SelectRandDoor()
        {
            pickedDoor = rand.Next(1, doors + 1);
        }

        // reveal an incorrect door
        static void RevealDoor(int currentTrial, bool forceSwap = false, bool forceStick = false)
        {
            // select a random door until one that is incorrect and is not the selected door is chosen
            int toReveal = 0;
            do
            {
                toReveal = rand.Next(1, doors + 1);
            } while (toReveal == pickedDoor || toReveal == correctDoor);

            // reveal selected door
            revealedDoor = toReveal;
            Console.WriteLine($"Door {revealedDoor} was not the right door. Would you like to switch your door now? y/n");
            string response = "";
            bool validResponse = false;

            // if forcing swap (not allowing user input) set value
            if (forceSwap)
            {
                response = "y";
                validResponse = true;
            }
            // if forcing not swap (not allowing user input) set value
            else if (forceStick)
            {
                response = "n";
                validResponse = true;
            }

            // if user input allowed, loop until valid
            while (!validResponse)
            {
                //get user input
                response = Console.ReadLine();
                if (response == "y")
                {
                    validResponse = true;
                }
                else if (response == "n")
                {
                    validResponse = true;
                }
            }

            // if swapping
            if (response == "y")
            {
                //swap to the unrevealed door
                int oldPick = pickedDoor;
                for (int j = 1; j <= doors; j++)
                {
                    if (j != oldPick && j != revealedDoor)
                    {
                        // reveal which door was correct
                        pickedDoor = j;
                        Console.WriteLine($"You have swapped from door {oldPick} to door {pickedDoor}");
                        RevealCorrectDoor(true, currentTrial);
                        return;
                    }
                }
            }
            // if not swapping
            else if (response == "n")
            {
                 //reveal which door was correct
                Console.WriteLine($"You stuck with door {pickedDoor}");
                RevealCorrectDoor(false, currentTrial);
                return;
            }

        }

        //reveal which door was correct
        static void RevealCorrectDoor(bool swapped, int currentTrial)
        {
            // display which door was correct
            Console.WriteLine($"Door {correctDoor} was the correct door. Your final pick was door {pickedDoor}.");
            // if the right door was picked
            if (correctDoor == pickedDoor)
            {
                // track correct, display info
                if (swapped)
                {
                    correctTrackerSwap++;
                    Console.WriteLine($"You were correct! ({correctTrackerSwap}/{currentTrial})");
                }
                else
                {
                    correctTrackerStick++;
                    Console.WriteLine($"You were correct! ({correctTrackerStick}/{currentTrial})");
                }
            }
            // if wrong door picked
            else
            {
                Console.WriteLine("You were incorrect! Try again!");
            }
            Console.WriteLine();
        }


    }
}*/

namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class StartUp
    {
        public const int PLAYFIELD_HEIGHT = 50;
        public const int PLAYFIELD_WIDTH = 30;
        public const int PLAYFIELD_UI = 10;

        public static int floorElementsLenght = 11;

        static void Main()
        {

            Console.BufferHeight = Console.WindowHeight = PLAYFIELD_HEIGHT;
            Console.BufferWidth = Console.WindowWidth = PLAYFIELD_WIDTH + PLAYFIELD_UI;
            Console.CursorVisible = false;

            Floor floor = new Floor();
            Tower tower = new Tower();
            UI drawUI = new UI();

            while (true)
            {

                floor.Move();
                floor.Draw();
                tower.Draw();
                drawUI.Draw();
                floor.InputHandler();
                floor.DrawFloors();
                Thread.Sleep(40);

                floor.Delete();
                floor.DeleteFloors();
                drawUI.Delete();
            }

            //maybe we need to add a bool to end the game and enter in the following method:
            GameOverScreen();

        }

        private static void GameOverScreen()
        {
            //alphabet letters:
            int alphabetLettersCount = 26;

            //creating a char array for the letters:
            char[] alphabet = new char[alphabetLettersCount];

            //counter to serve as an index to the array
            int counter = 0;

            //fill the array:
            for (char i = 'A'; i < '['; i++)
            {
                alphabet[counter] = i;
                counter++;
            }

            /* dictionary to hold:  
             * key      -> int, the score
             * value    -> string, the name (consisting of three letters) (for now just AAA)
             * */
            Dictionary<int, string> scoreboard = new Dictionary<int, string>();

            //and we need a string to keep the current combination of letters
            string currentLetterCombination = string.Empty;

            //fill the dictionary up
            for (int i = 1; i < 10; i++)
            {
                scoreboard[i] = "AAA";
            }

            //clear the screen to display the game over screen
            Console.Clear();

            //placing the cursor in the middle of the field
            int widthMessageDisplay = 1;
            int heightMessageDisplay = 16;
            Console.SetCursorPosition(widthMessageDisplay, heightMessageDisplay);

            //asking the user to input his name:
            Console.Write("Great job, man! Now enter your name by    pressing Up, Down and Enter on the                desired letter!");
            //^ maybe the text needs to be readjusted to appear better

            //moving the cursor position
            int widthLettersDisplay = 18;
            int heightLettersDisplay = 20;
            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);

            //showing the cursor, for old school style name input :P
            Console.CursorVisible = true;

            //current letter int, we start at A, so -> 0
            int currentLetter = 0;

            //display the first letter
            Console.Write(alphabet[currentLetter]); //maybe zero should be an int?

            //we need three letters, so counter again from 0
            int letterCounter = 0;

            //while cycle until we get the desired count (3) letters
            while (letterCounter < 3)
            {
                //reading the key
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey();
                    }

                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        //check if overflooding the alphabet
                        if (currentLetter < 27)
                        {
                            currentLetter++;
                            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);
                            Console.Write(alphabet[currentLetter]);
                        }
                    }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        //check if underflooding the alphabet
                        if (currentLetter > 0)
                        {
                            currentLetter--;
                            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);
                            Console.Write(alphabet[currentLetter]);
                        }
                    }
                    else if (userInput.Key == ConsoleKey.Enter)
                    {
                        //add the current letter to the string builder
                        currentLetterCombination += alphabet[currentLetter];

                        //increase the letters count:
                        letterCounter++;

                        //returning the alphabet back to zero
                        currentLetter = 0;

                        //shifting the next letter one position to the right
                        widthLettersDisplay++;

                        //displaying the next letter if it's less than 4
                        if (letterCounter < 3)
                        {
                            Console.SetCursorPosition(widthLettersDisplay, heightLettersDisplay);
                            Console.Write(alphabet[currentLetter]);
                        }

                    }
                }
            }

            //we need a score integer, for now I will just input a random score int to test it
            int score = 20;

            //adding the current letters and score to the scoreboard
            scoreboard[score] = currentLetterCombination;

            //displaying the score in the UI
            int scoreUIwidthDisplay = 30;
            int scoreUIheightDisplay = 19;

            //this one is for increasing the height of the cursor and the numeration of the top 9 scorers
            int increaserAndDisplayer = 1;


            //this is for displaying the current ladder of the highest score users,
            //it should be used in the PLAYFIELD_UI
            foreach (var kvp in scoreboard.OrderByDescending(x => x.Key).Take(9))
            {
                Console.SetCursorPosition(scoreUIwidthDisplay, scoreUIheightDisplay + increaserAndDisplayer);
                Console.Write($"{increaserAndDisplayer} {kvp.Value} - {kvp.Key}");
                increaserAndDisplayer++;
            }
        }
    }
}
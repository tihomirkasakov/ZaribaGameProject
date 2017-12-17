namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using static Floor;
    using static RandomMoveFloor;

    public class StartUp
    {
        public const int PLAYFIELD_HEIGHT = 50;
        public const int PLAYFIELD_WIDTH = 30;
        public const int PLAYFIELD_UI = 17;

        public static int currentRow = 1;
        public static int floorElementsLenght = 10;
        public static bool isGameOver = false;
        public static bool keyPressed = true;
        public static int score = 0;
        public static bool rerun = true;
        public static bool loadLevel = true;

        public enum Difficulty
        {
            Tower,
            Owert,
            Toooower
        }

        public static Difficulty difficulty;

        /* dictionary to hold:  
        * key      -> int, the score
        * value    -> string, the name (consisting of three letters) (for now just AAA)
        * */
        public static Dictionary<int, string> leaderboard = new Dictionary<int, string>();

        static void Main()
        {
            Console.BufferHeight = Console.WindowHeight = PLAYFIELD_HEIGHT;
            Console.BufferWidth = Console.WindowWidth = PLAYFIELD_WIDTH + PLAYFIELD_UI;

            //fill the dictionary up with 9 scores, all names AAA
            for (int i = 1; i < 10; i++)
            {
                leaderboard[i] = "AAA";
            }

            while (rerun)
            {

                Console.CursorVisible = false;
                Console.Clear();
                ChooseDifficultyScreen();

                UI drawUI = new UI();

                //there is no need to put the Draw method in the while cycle, just update it
                drawUI.Draw(score, leaderboard, difficulty);

                while (!isGameOver && difficulty == Difficulty.Tower)
                {
                    if (loadLevel)
                    {
                        LoadLevel(Elements);
                        loadLevel = false;
                    }

                    InputHandler();
                    if (keyPressed)
                    {
                        GenerateFloor();
                        keyPressed = false;
                    }
                    MoveFloor();
                    DrawFloor();

                    Thread.Sleep(40);

                    DeleteFloor();
                    drawUI.UpdateUI(score, difficulty);
                }

                while (!isGameOver && difficulty == Difficulty.Owert)
                {
                    if (loadLevel)
                    {
                        LoadLevel(RandomElements);
                        loadLevel = false;
                    }

                    RandomInputHandler();
                    if (keyPressed)
                    {
                        RandomMoveGenerateFloor();
                        keyPressed = false;
                    }
                    RandomMoveMoveFloor();
                    RandomMoveDrawFloor();

                    Thread.Sleep(40);

                    RandomMoveDeleteFloor();
                    drawUI.UpdateUI(score, difficulty);
                }

                GameOverScreen();
                Thread.Sleep(10000);

                currentRow = 1;
                floorElementsLenght = 10;
                isGameOver = false;
                keyPressed = true;
                score = 0;
                loadLevel = true;
            }
        }

        private static void ChooseDifficultyScreen()
        {
            int displayDifficultyWidth = 10;
            int displayDifficultyHeight = 20;

            Console.SetCursorPosition(displayDifficultyWidth, displayDifficultyHeight);
            Console.Write("Please select difficulty: ");

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 2);
            Console.Write(Difficulty.Tower);

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 3);
            Console.Write(Difficulty.Owert);

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 4);
            Console.Write(Difficulty.Toooower);

            int selectorHeight = 22;
            Console.SetCursorPosition(displayDifficultyWidth + 7, selectorHeight);
            Console.Write(">");

            bool isDifficultySeleted = false;

            while (!isDifficultySeleted)
            {
                if (Console.KeyAvailable)
                {
                    //the following line is used because otherwise it eats up letters, idk why, 
                    //kind of a feature, not a bug :D
                    Console.SetCursorPosition(displayDifficultyWidth + 7, selectorHeight + 5);

                    ConsoleKeyInfo userInput = Console.ReadKey();

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey();
                    }

                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (selectorHeight == 22)
                        {
                            continue;
                        }
                        else
                        {
                            Console.SetCursorPosition(displayDifficultyWidth + 7, selectorHeight);
                            Console.Write(" ");
                            selectorHeight--;
                            Console.SetCursorPosition(displayDifficultyWidth + 7, selectorHeight);
                            Console.Write(">");
                        }
                    }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (selectorHeight == 24)
                        {
                            continue;
                        }
                        else
                        {
                            Console.SetCursorPosition(displayDifficultyWidth + 7, selectorHeight);
                            Console.Write(" ");
                            selectorHeight++;
                            Console.SetCursorPosition(displayDifficultyWidth + 7, selectorHeight);
                            Console.Write(">");
                        }
                    }
                    else if (userInput.Key == ConsoleKey.Enter)
                    {
                        isDifficultySeleted = true;
                        Console.Clear();
                        if (selectorHeight == 22)
                        {
                            difficulty = Difficulty.Tower;
                        }
                        else if (selectorHeight == 23)
                        {
                            difficulty = Difficulty.Owert;
                        }
                        else if (selectorHeight == 24)
                        {
                            difficulty = Difficulty.Toooower;
                        }
                    }
                }
            }
        }

        public static void ClearLastTwoRows()
        {
            for (int i = PLAYFIELD_HEIGHT - 2; i < PLAYFIELD_HEIGHT; i++)
            {
                for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(' ');
                }
            }
        }

        public static void LoadLevel(int[,] elements)
        {
            string[] input = File.ReadAllLines("level.txt");

            for (int row = 0; row < input.Length; row++)
            {
                for (int symbol = 0; symbol < input[row].Length; symbol++)
                {
                    switch (difficulty)
                    {
                        case Difficulty.Tower:
                            if (int.Parse(input[row][symbol].ToString()) != 0)
                            {
                                Elements[row, symbol] = int.Parse(input[row][symbol].ToString());
                            }
                            else
                            {
                                Elements[row, symbol] = 0;
                            }
                            break;
                        case Difficulty.Owert:
                            if (int.Parse(input[row][symbol].ToString()) != 0)
                            {
                                RandomElements[row, symbol] = int.Parse(input[row][symbol].ToString());
                            }
                            else
                            {
                                RandomElements[row, symbol] = 0;
                            }
                            break;
                        case Difficulty.Toooower:
                            break;
                    }
                }
            }
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

            //and we need a string to keep the current combination of letters
            string currentLetterCombination = string.Empty;

            //clear the screen to display the game over screen
            Console.Clear();

            //placing the cursor in the middle of the field
            int widthMessageDisplay = 0;
            int heightMessageDisplay = 16;
            Console.SetCursorPosition(widthMessageDisplay, heightMessageDisplay);

            //asking the user to input his name:
            Console.Write("\tGreat job, man! \n\tNow enter your name by    \n\tpressing Up, Down and Enter \n\ton the desired letter!");
            //^ maybe the text needs to be readjusted to appear better

            //moving the cursor position
            int widthLettersDisplay = 18;
            int heightLettersDisplay = 25;
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
            int hiScore = score;

            //adding the current letters and score to the scoreboard
            leaderboard[hiScore] = currentLetterCombination;
        }
    }
}
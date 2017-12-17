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

    public class StartUp
    {
        public const int PLAYFIELD_HEIGHT = 50;
        public const int PLAYFIELD_WIDTH = 30;
        public const int PLAYFIELD_UI = 17;

        public static int floorElementsLenght = 10;
        public static bool isGameOver = false;
        public static bool keyPressed = true;
        public static int score = 0;

        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
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
            Console.CursorVisible = false;

            //fill the dictionary up with 9 scores, all names AAA
            for (int i = 1; i < 10; i++)
            {
                leaderboard[i] = "AAA";
            }

            //adding the home screen that displays a tower, team name and it's on thread sleep 5000 for viewing
            IntroScreen();

            ChooseDifficultyScreen();

            UI drawUI = new UI();
            LoadLevel(Elements);

            //there is no need to put the Draw method in the while cycle, just update it
            drawUI.Draw(score, leaderboard, difficulty);

            while (!isGameOver)
            {
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
            GameOverScreen();
            Thread.Sleep(10000);
        }

        //this method shows up first when you boot the game. it displays a tower sign, a tower with 
        //team 6 in it and it is built from the bottom to the top slowly, so that it creates kind
        //of a 'intro' feeling. after that the thread sleep is set to 5000, you can lower it if you
        //don't wish to wait that much.
        private static void IntroScreen()
        {
            string[] introTower = new string[]
            {
                "                   *                       ",
                "                   :                       ",
                "                   |                       ",
                "                   |                       ",
                "                   |                       ",
                "                  :|:                      ",
                "                  |||                      ",
                "             _____|||_____                 ",
                "            /=============\\               ",
                "        ---<~~~~~~~~~~~~~~~>---            ",
                "            \\-------------/               ",
                "             \\___________/                ",
                "               \\||:::||/                  ",
                "                ||:::||                    ",
                "                ||:::||                    ",
                "                ||:::||                    ",
                "                ||ooo||                    ",
                "                ||___||                    ",
                "                ||:::||                    ",
                "                ||:::||                    ",
                "                ||:::||                    ",
                "                ||:::||                    ",
                "                ||:::||                    ",
                "               /||:::||\\                  ",
                "              / ||:::|| \\                 ",
                "             /  ||:::||  \\                ",
                "            /   ||:::||   \\               ",
                "        ___/____||:::||____\\____          ",
                "       /~~~~~~~~ TEAM 6 ~~~~~~~~\\         ",
                "      /   |~~~~~~~~|  _____      \\        ",
                "      |   |________| |  |  |     |          ",
                "______|______________|__|__|_____|_________",
                "                                             ",
                "     _____ _____  _    _ ___________  ",
                "    |_   _|  _  || |  | |  ___| ___ \\ ",
                "      | | | | | || |  | | |__ | |_/ / ",
                "      | | | | | || |/\\| |  __||    /  ",
                "      | | \\ \\_/ /\\  /\\  / |___| |\\ \\  ",
                "      \\_/  \\___/  \\/  \\/\\____/\\_| \\_| ",
            };

            int startDisplayingWidth = 3;
            int startDisplayingHeight = 5;

            for (int i = introTower.Length - 1; i >= 0; i--)
            {
                Thread.Sleep(50);
                Console.SetCursorPosition(startDisplayingWidth, startDisplayingHeight + i);
                Console.Write(introTower[i]);
            }
            Thread.Sleep(5000);
            Console.Clear();
        }

        //simple method about creating the difficulty screen, with positioning of the words, selecting
        //and displaying
        private static void ChooseDifficultyScreen()
        {
            int displayDifficultyWidth = 10;
            int displayDifficultyHeight = 20;

            Console.SetCursorPosition(displayDifficultyWidth, displayDifficultyHeight);
            Console.Write("Please select difficulty: ");

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 2);
            Console.Write(Difficulty.Easy);

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 3);
            Console.Write(Difficulty.Medium);

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 4);
            Console.Write(Difficulty.Hard);

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
                            difficulty = Difficulty.Easy;
                        }
                        else if (selectorHeight == 23)
                        {
                            difficulty = Difficulty.Medium;
                        }
                        else if (selectorHeight == 24)
                        {
                            difficulty = Difficulty.Hard;
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
                    if (int.Parse(input[row][symbol].ToString()) != 0)
                    {
                        Elements[row, symbol] = int.Parse(input[row][symbol].ToString());
                    }
                    else
                    {
                        Elements[row, symbol] = 0;
                    }
                }
            }
        }

        private static void GameOverScreen()
        {
            //clear the screen to display the game over screen
            Console.Clear();

            //fill array with logo
            string[] buildingDisplay = new string[]
            {
                "   _________    __  _________",
                "  / ____/   |  /  |/  / ____/",
                " / / __/ /| | / /|_/ / __/   ",
                "/ /_/ / ___ |/ /  / / /___   ",
                "\\____/_/  |_/_/  /_/_____/  ",
                "                             ",
                "   ____ _    ____________    ",
                "  / __ \\ |  / / ____/ __ \\ ",
                " / / / / | / / __/ / /_/ /   ",
                "/ /_/ /| |/ / /___/ _, _/    ",
                "\\____/ |___/_____/_/ |_|    ",
            };

            int gameOverWidth = 8;
            int gameOverHeight = 5;

            for (int i = 0; i < buildingDisplay.Length; i++)
            {
                Console.SetCursorPosition(gameOverWidth, gameOverHeight + i);
                Console.WriteLine(buildingDisplay[i]);
            }

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

            //placing the cursor in the middle of the field
            int widthMessageDisplay = 0;
            int heightMessageDisplay = 18;
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
            Console.Write(alphabet[currentLetter]);

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
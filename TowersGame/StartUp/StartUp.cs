namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Speech.Synthesis;
    using System.Text;
    using System.Threading;
    using static Floor;
    using static RandomMoveFloor;
    using static SkipRowsFloor;

    public class StartUp
    {
        public const int PLAYFIELD_HEIGHT = 50;
        public const int PLAYFIELD_WIDTH = 30;
        public const int PLAYFIELD_UI = 17;

        public static bool isGameOver = false;
        public static bool rerun = true;
        public static bool loadLevel = true;
        public static int score = 0;
        public static int currentRow = 1;
        public static int floorElementsLenght = 10;

        public static bool keyPressed = true;
        public static int difficultySpeed = 100;
        public static int floorsCount = 0;

        public enum Difficulty
        {
            Swing,
            Glitch,
            Overfall
        }

        public static Difficulty difficulty;

        /* dictionary to hold:  
        * key      -> int, the score
        * value    -> string, the name (consisting of three letters) (for now just AAA)
        * */
        public static Dictionary<int, string> swingLeaderBoard = new Dictionary<int, string>();
        public static Dictionary<int, string> glitchLeaderBoard = new Dictionary<int, string>();
        public static Dictionary<int, string> overfallLeaderBoard = new Dictionary<int, string>();

        static void Main()
        {
            Console.BufferHeight = Console.WindowHeight = PLAYFIELD_HEIGHT;
            Console.BufferWidth = Console.WindowWidth = PLAYFIELD_WIDTH + PLAYFIELD_UI;
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;

            //this reads the \bin\Debug\swing.txt, so that we can keep previous scores
            string[][] swingLines = File.ReadAllLines(@"swing.txt")
                .Select(s => s.Split(' '))
                .ToArray();

            //this fills the dictionary that we have with the scores from the txt file
            for (int i = 0; i < swingLines.Length; i++)
            {
                swingLeaderBoard[int.Parse(swingLines[i][0])] = swingLines[i][1];
            }


            //this reads the \bin\Debug\glitch.txt, so that we can keep previous scores
            string[][] glitchLines = File.ReadAllLines(@"glitch.txt")
                .Select(s => s.Split(' '))
                .ToArray();

            //this fills the dictionary that we have with the scores from the txt file
            for (int i = 0; i < glitchLines.Length; i++)
            {
                glitchLeaderBoard[int.Parse(glitchLines[i][0])] = glitchLines[i][1];
            }


            //this reads the \bin\Debug\overfall.txt, so that we can keep previous scores
            string[][] overfallLines = File.ReadAllLines(@"overfall.txt")
                .Select(s => s.Split(' '))
                .ToArray();

            //this fills the dictionary that we have with the scores from the txt file
            for (int i = 0; i < overfallLines.Length; i++)
            {
                overfallLeaderBoard[int.Parse(overfallLines[i][0])] = overfallLines[i][1];
            }
            //adding the home screen that displays a tower, team name and it's on thread sleep 5000 for viewing
            //oh, and also - sound :P
            IntroScreen();

            while (rerun)
            {
                Console.CursorVisible = false;
                Console.Clear();
                ChooseDifficultyScreen();

                UI drawUI = new UI();

                //there is no need to put the Draw method in the while cycle, just update it
                drawUI.Draw(score, swingLeaderBoard, difficulty);

                while (!isGameOver && difficulty == Difficulty.Swing)
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

                    if (floorsCount == 10)
                    {
                        difficultySpeed = 70;
                    }
                    else if (floorsCount == 20)
                    {
                        difficultySpeed = 30;
                    }

                    MoveFloor();

                    DrawFloor();

                    Thread.Sleep(difficultySpeed);

                    DeleteFloor();

                    drawUI.UpdateUI(score, floorsCount, difficulty);
                }

                while (!isGameOver && difficulty == Difficulty.Glitch)
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

                    if (floorsCount == 10)
                    {
                        difficultySpeed = 70;
                    }
                    else if (floorsCount == 20)
                    {
                        difficultySpeed = 30;
                    }

                    RandomMoveMoveFloor();

                    RandomMoveDrawFloor();

                    Thread.Sleep(difficultySpeed);

                    RandomMoveDeleteFloor();

                    drawUI.UpdateUI(score, floorsCount, difficulty);
                }

                while (!isGameOver && difficulty == Difficulty.Overfall)
                {
                    if (loadLevel)
                    {
                        LoadLevel(SkipElements);
                        loadLevel = false;
                    }

                    SkipRowInputHandler();

                    if (keyPressed)
                    {
                        SkipRowGenerateFloor();
                        keyPressed = false;
                    }

                    if (floorsCount == 10)
                    {
                        difficultySpeed = 70;
                    }
                    else if (floorsCount == 20)
                    {
                        difficultySpeed = 30;
                    }

                    SkipRowMoveFloor();

                    SkipRowDrawFloor();

                    Thread.Sleep(difficultySpeed);

                    SkipRowDeleteFloor();

                    drawUI.UpdateUI(score, floorsCount, difficulty);
                }

                GameOverScreen();
                currentRow = 1;
                floorElementsLenght = 10;
                isGameOver = false;
                keyPressed = true;
                score = 0;
                loadLevel = true;
                floorsCount = 0;
                difficultySpeed = 100;
            }
        }

        //this method shows up first when you boot the game. it displays a tower sign, a tower with 
        //team 6 in it and it is built from the bottom to the top slowly, so that it creates kind
        //of a 'intro' feeling. after that the thread sleep is set to 2000, you can lower it if you
        //don't wish to wait that much.
        public static void IntroScreen()
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

            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.
            synth.Speak("Team 6 to the game presents");
            synth.Speak("TOWER");

            Thread.Sleep(2000);
            Console.Clear();
        }

        //simple method about creating the difficulty screen, with positioning of the words, selecting
        //and displaying
        public static void ChooseDifficultyScreen()
        {
            string[] frame = new string[]
            {
                "     ___     ___     ___     ___     ___     ",
                " ___/   \\___/   \\___/   \\___/   \\___/   \\___ ",
                "/   \\___/   \\___/   \\___/   \\___/   \\___/   \\",
                "\\___/   \\___/   \\___/   \\___/   \\___/   \\___/",
                "/   \\___/                           \\___/   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\                                   /   \\",
                "\\___/                                   \\___/",
                "/   \\___                             ___/   \\",
                "\\___/   \\___     ___     ___     ___/   \\___/",
                "/   \\___/   \\___/   \\___/   \\___/   \\___/   \\",
                "\\___/   \\___/   \\___/   \\___/   \\___/   \\___/",
                "    \\___/   \\___/   \\___/   \\___/   \\___/    ",
            };

            for (int i = 0; i < frame.Length; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.WriteLine(frame[i]);
            }

            int displayDifficultyWidth = 10;
            int displayDifficultyHeight = 20;

            Console.SetCursorPosition(displayDifficultyWidth, displayDifficultyHeight);
            Console.Write("Please select difficulty: ");

            //it is absurd to write every new width and height as a separate integer,
            //that's why I adjust them by hand, so a lot of pluses to follow:
            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 2);
            Console.Write(Difficulty.Swing);

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 3);
            Console.Write(Difficulty.Glitch);

            Console.SetCursorPosition(displayDifficultyWidth + 9, displayDifficultyHeight + 4);
            Console.Write(Difficulty.Overfall);

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
                            difficulty = Difficulty.Swing;
                        }
                        else if (selectorHeight == 23)
                        {
                            difficulty = Difficulty.Glitch;
                        }
                        else if (selectorHeight == 24)
                        {
                            difficulty = Difficulty.Overfall;
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
                        case Difficulty.Swing:
                            if (int.Parse(input[row][symbol].ToString()) != 0)
                            {
                                Elements[row, symbol] = int.Parse(input[row][symbol].ToString());
                            }
                            else
                            {
                                Elements[row, symbol] = 0;
                            }
                            break;
                        case Difficulty.Glitch:
                            if (int.Parse(input[row][symbol].ToString()) != 0)
                            {
                                RandomElements[row, symbol] = int.Parse(input[row][symbol].ToString());
                            }
                            else
                            {
                                RandomElements[row, symbol] = 0;
                            }
                            break;
                        case Difficulty.Overfall:
                            if (int.Parse(input[row][symbol].ToString()) != 0)
                            {
                                SkipElements[row, symbol] = int.Parse(input[row][symbol].ToString());
                            }
                            else
                            {
                                SkipElements[row, symbol] = 0;
                            }
                            break;
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

            if (difficulty == Difficulty.Swing)
            {
                //adding the current letters and score to the scoreboard
                swingLeaderBoard[hiScore] = currentLetterCombination;

                //exporting the current result set to the txt file
                File.WriteAllLines("swing.txt", swingLeaderBoard.Select(x => x.Key + " " + x.Value).ToArray());
            }

            else if (difficulty == Difficulty.Glitch)
            {
                //adding the current letters and score to the scoreboard
                glitchLeaderBoard[hiScore] = currentLetterCombination;

                //exporting the current result set to the txt file
                File.WriteAllLines("glitch.txt", glitchLeaderBoard.Select(x => x.Key + " " + x.Value).ToArray());
            }

            else if (difficulty == Difficulty.Overfall)
            {
                //adding the current letters and score to the scoreboard
                overfallLeaderBoard[hiScore] = currentLetterCombination;

                //exporting the current result set to the txt file
                File.WriteAllLines("overfall.txt", overfallLeaderBoard.Select(x => x.Key + " " + x.Value).ToArray());
            }
        }
    }
}
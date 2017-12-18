namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static StartUp;

    public class RandomMoveFloor
    {
        public static int[,] RandomElements = new int[PLAYFIELD_HEIGHT, PLAYFIELD_WIDTH];

        public static bool moveLeft = true;
        public static Random rng = new Random();
        public static int currentRow = 1;
        public static int startingRow;
        public static bool changeDirection = false;

        public static void RandomInputHandler()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo userInput = Console.ReadKey();

                while (Console.KeyAvailable)
                {
                    Console.ReadKey();
                }

                if (userInput.Key == ConsoleKey.Spacebar)
                {
                    //increase row while tower is lower than 18lvls
                    int currentLenght = 0;

                    keyPressed = true;

                    if (currentRow <= 18)
                    {
                        currentRow++;
                    }

                    //move floors down
                    else
                    {
                        for (int i = PLAYFIELD_HEIGHT - 1; i >= 1; i--)
                        {
                            for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                            {
                                RandomElements[i, j] = RandomElements[i - 1, j];
                            }
                        }

                    }

                    //check for right place
                    for (int i = PLAYFIELD_HEIGHT - 1; i >= PLAYFIELD_HEIGHT - 2 - currentRow; i--)
                    {
                        for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                        {
                            if ((RandomElements[i - 1, j] != RandomElements[i, j] && RandomElements[i - 1, j] == 1) &&
                                ((RandomElements[i - 1, j] != RandomElements[i, j] - 1 && RandomElements[i - 1, j] == 1)))
                            {
                                RandomElements[i - 1, j] = 0;
                            }
                        }
                    }

                    //check lenght for next floor
                    for (int i = 0; i < PLAYFIELD_WIDTH; i++)
                    {
                        if (RandomElements[PLAYFIELD_HEIGHT - 1 - currentRow, i] == 1)
                        {
                            currentLenght++;
                        }
                    }

                    floorElementsLenght = currentLenght;
                    score += currentLenght;

                    //increase the number of floors amassed for the UI
                    floorsCount++;

                    if (currentLenght == 0)
                    {
                        isGameOver = true;
                    }

                    currentLenght = 0;
                }
            }
        }

        public static void RandomMoveGenerateFloor()
        {
            startingRow = PLAYFIELD_HEIGHT - 2 - currentRow;
            int startingPosition = rng.Next(1, PLAYFIELD_WIDTH - floorElementsLenght - 1);

            for (int i = startingPosition; i < startingPosition + floorElementsLenght; i++)
            {
                RandomElements[startingRow, i] = 1;
            }
        }

        public static void RandomMoveMoveFloor()
        {
            int moveDirection = rng.Next(0, 50);
            if (moveDirection == 2 && moveLeft && RandomElements[startingRow,0]==0)
            {
                moveLeft = false;
            }
            else if (moveDirection == 14 && !moveLeft && RandomElements[startingRow, PLAYFIELD_WIDTH-1] == 0)
            {
                moveLeft = true;
            }

            if (moveLeft)
            {

                if (RandomElements[startingRow, 0] == 0)
                {

                    for (int i = 0; i < PLAYFIELD_WIDTH - 1; i++)
                    {

                        if (RandomElements[startingRow, i + 1] == 1)
                        {
                            RandomElements[startingRow, i] = 1;
                        }
                        else
                        {
                            RandomElements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = false;
                    RandomElements[startingRow, 0] = 0;
                    RandomElements[startingRow, floorElementsLenght] = 1;
                }
            }

            else
            {
                if (RandomElements[startingRow, PLAYFIELD_WIDTH - 1] == 0)
                {

                    for (int i = PLAYFIELD_WIDTH - 1; i > 0; i--)
                    {
                        if (i == 1)
                        {
                            RandomElements[startingRow, 0] = 0;
                        }
                        if (RandomElements[startingRow, i - 1] == 1)
                        {
                            RandomElements[startingRow, i] = 1;
                        }
                        else
                        {
                            RandomElements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = true;
                    RandomElements[startingRow, PLAYFIELD_WIDTH - 1] = 0;
                    RandomElements[startingRow, PLAYFIELD_WIDTH - floorElementsLenght - 1] = 1;
                }
            }
            changeDirection = false;
        }

        public static void RandomMoveDrawFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (RandomElements[row, col] == 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('\uFDFC');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (RandomElements[row, col] == 2)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write('\uFDFC');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }

        public static void RandomMoveDeleteFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (RandomElements[row, col] == 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(' ');
                    }
                    else if (RandomElements[row, col] == 2)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(' ');
                    }
                }
            }
        }
    }
}
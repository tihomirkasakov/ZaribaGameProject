namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using static StartUp;

    public class SkipRowsFloor
    {
        public static int[,] SkipElements = new int[PLAYFIELD_HEIGHT, PLAYFIELD_WIDTH];

        public static bool moveLeft = true;
        public static Random rng = new Random();
        public static int startingRow;
        public static int skipRows = 5;

        public static void SkipRowInputHandler()
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
                                SkipElements[i, j] = SkipElements[i - 1, j];
                            }
                        }
                    }

                    for (int i = 1; i <= skipRows; i++)
                    {
                        for (int j = 0; j < PLAYFIELD_WIDTH - 1; j++)
                        {
                            SkipElements[SkipRowsFloor.startingRow + i, j] = SkipElements[SkipRowsFloor.startingRow + i - 1, j];
                            SkipElements[SkipRowsFloor.startingRow + i - 1, j] = 0;
                        }
                        SkipRowDrawFloor();
                        Thread.Sleep(100);
                        SkipRowDeleteFloor();
                    }


                    //check for right place
                    for (int i = PLAYFIELD_HEIGHT - 1; i >= PLAYFIELD_HEIGHT - 2 - currentRow; i--)
                    {
                        for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                        {
                            if ((SkipElements[i - 1, j] != SkipElements[i, j] && SkipElements[i - 1, j] == 1) &&
                                ((SkipElements[i - 1, j] != SkipElements[i, j] - 1 && SkipElements[i - 1, j] == 1)))
                            {
                                SkipElements[i - 1, j] = 0;
                            }
                        }
                    }

                    //check lenght for next floor
                    for (int i = 0; i < PLAYFIELD_WIDTH; i++)
                    {
                        if (SkipElements[PLAYFIELD_HEIGHT - 1 - currentRow, i] == 1)
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

        public static void SkipRowGenerateFloor()
        {
            startingRow = PLAYFIELD_HEIGHT - 2 - currentRow-skipRows;
            int startingPosition = rng.Next(1, PLAYFIELD_WIDTH - floorElementsLenght - 1);

            for (int i = 0; i < PLAYFIELD_WIDTH-1; i++)
            {
                SkipElements[startingRow, i] = 0;
            }

            for (int i = startingPosition; i < startingPosition + floorElementsLenght; i++)
            {
                SkipElements[startingRow, i] = 1;
            }
        }

        public static void SkipRowMoveFloor()
        {
            if (moveLeft)
            {
                if (SkipElements[startingRow, 0] == 0)
                {

                    for (int i = 0; i < PLAYFIELD_WIDTH - 1; i++)
                    {

                        if (SkipElements[startingRow, i + 1] == 1)
                        {
                            SkipElements[startingRow, i] = 1;
                        }
                        else
                        {
                            SkipElements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = false;
                    SkipElements[startingRow, 0] = 0;
                    SkipElements[startingRow, floorElementsLenght] = 1;
                }
            }

            else
            {
                if (SkipElements[startingRow, PLAYFIELD_WIDTH - 1] == 0)
                {
                    for (int i = PLAYFIELD_WIDTH - 1; i > 0; i--)
                    {
                        if (i == 1)
                        {
                            SkipElements[startingRow, 0] = 0;
                        }
                        if (SkipElements[startingRow, i - 1] == 1)
                        {
                            SkipElements[startingRow, i] = 1;
                        }
                        else
                        {
                            SkipElements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = true;
                    SkipElements[startingRow, PLAYFIELD_WIDTH - 1] = 0;
                    SkipElements[startingRow, PLAYFIELD_WIDTH - floorElementsLenght - 1] = 1;
                }
            }
        }

        public static void SkipRowDrawFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (SkipElements[row, col] == 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('\uFDFC');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (SkipElements[row, col] == 2)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write('\uFDFC');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }

        public static void SkipRowDeleteFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (SkipElements[row, col] == 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(' ');
                    }
                    else if (SkipElements[row, col] == 2)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(' ');
                    }
                }
            }
        }
    }
}

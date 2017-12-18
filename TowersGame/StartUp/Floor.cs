namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static StartUp;

    public class Floor
    {
        public static int[,] Elements = new int[PLAYFIELD_HEIGHT, PLAYFIELD_WIDTH];

        public static bool moveLeft = true;
        public static Random rng = new Random();
        public static int startingRow;

        public static void InputHandler()
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
                        for (int i = PLAYFIELD_HEIGHT-1; i >= 1; i--)
                        {
                            for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                            {
                                Elements[i, j] = Elements[i - 1, j];
                            }
                        }

                    }

                    //check for right place
                    for (int i = PLAYFIELD_HEIGHT - 1; i >= PLAYFIELD_HEIGHT - 2 - currentRow; i--)
                    {
                        for (int j = 0; j < PLAYFIELD_WIDTH; j++)
                        {
                            if ((Elements[i - 1, j] != Elements[i, j] && Elements[i - 1, j] == 1)&&
                                ((Elements[i - 1, j] != Elements[i, j]-1 && Elements[i - 1, j] == 1)))
                            {
                                Elements[i - 1, j] = 0;
                            }
                        }
                    }

                    //check lenght for next floor
                    for (int i = 0; i < PLAYFIELD_WIDTH; i++)
                    {
                        if (Elements[PLAYFIELD_HEIGHT-1-currentRow,i]==1)
                        {
                            currentLenght++;
                        }
                    }

                    floorElementsLenght = currentLenght;
                    score += currentLenght;

                    //increase the number of floors amassed for the UI
                    floorsCount++;

                    if (currentLenght==0)
                    {
                        isGameOver = true;
                    }

                    currentLenght = 0;
                }
            }
        }

        public static void GenerateFloor()
        {
            startingRow = PLAYFIELD_HEIGHT - 2 - currentRow;
            int startingPosition = rng.Next(1, PLAYFIELD_WIDTH - floorElementsLenght - 1);

            for (int i = startingPosition; i < startingPosition + floorElementsLenght; i++)
            {
                Elements[startingRow, i] = 1;
            }
        }

        public static void MoveFloor()
        {
            if (moveLeft)
            {

                if (Elements[startingRow, 0] == 0)
                {

                    for (int i = 0; i < PLAYFIELD_WIDTH - 1; i++)
                    {

                        if (Elements[startingRow, i + 1] == 1)
                        {
                            Elements[startingRow, i] = 1;
                        }
                        else
                        {
                            Elements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = false;
                    Elements[startingRow, 0] = 0;
                    Elements[startingRow, floorElementsLenght] = 1;
                }
            }

            else
            {
                if (Elements[startingRow, PLAYFIELD_WIDTH - 1] == 0)
                {
                    for (int i = PLAYFIELD_WIDTH - 1; i > 0; i--)
                    {
                        if (i == 1)
                        {
                            Elements[startingRow, 0] = 0;
                        }
                        if (Elements[startingRow, i - 1] == 1)
                        {
                            Elements[startingRow, i] = 1;
                        }
                        else
                        {
                            Elements[startingRow, i] = 0;
                        }
                    }
                }
                else
                {
                    moveLeft = true;
                    Elements[startingRow, PLAYFIELD_WIDTH - 1] = 0;
                    Elements[startingRow, PLAYFIELD_WIDTH - floorElementsLenght - 1] = 1;
                }
            }
        }

        public static void DrawFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (Elements[row, col] == 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('\uFDFC');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (Elements[row, col] == 2)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write('\uFDFC');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }

        public static void DeleteFloor()
        {
            for (int row = 0; row < PLAYFIELD_HEIGHT; row++)
            {
                for (int col = 0; col < PLAYFIELD_WIDTH; col++)
                {
                    if (Elements[row, col] == 1)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(' ');
                    }
                    else if (Elements[row, col] == 2)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write(' ');
                    }
                }
            }
        }
    }
}
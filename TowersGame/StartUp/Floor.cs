namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static StartUp;
    using static Tower;

    public class Floor
    {
        //public List<Vector2> Elements;
        public static int[,] Elements = new int[PLAYFIELD_HEIGHT, PLAYFIELD_WIDTH];

        public static bool moveLeft = true;
        public static Random rng = new Random();
        public static int currentRow = 1;
        public static int startingRow = PLAYFIELD_HEIGHT - 2 - currentRow;
        //public List<Vector2> TowerElements;

        //public Floor()
        //{
        //    this.Elements = new List<Vector2>();
        //    this.TowerElements = new List<Vector2>();

        //    for (int i = 0; i < floorElementsLenght; i++)
        //    {
        //        Elements.Add(new Vector2(PLAYFIELD_WIDTH/2- floorElementsLenght / 2+i, PLAYFIELD_HEIGHT-2-currentRow));
        //    }
        //}

        //public void Move()
        //{
        //    if (!moveRight)
        //    {
        //        if (this.Elements[0].X > 0)
        //        {
        //            for (int i = 0; i < Elements.Count; i++)
        //            {
        //                this.Elements[i].X -= 1;
        //            }
        //        }
        //        else if (this.Elements[0].X == 0)
        //        {
        //            moveRight = true;
        //        }
        //    }
        //    else
        //    {
        //        if (this.Elements.Last().X < PLAYFIELD_WIDTH - 1)
        //        {
        //            for (int i = 0; i < Elements.Count; i++)
        //            {
        //                this.Elements[i].X += 1;
        //            }
        //        }
        //        else if (this.Elements.Last().X == PLAYFIELD_WIDTH-1)
        //        {
        //            moveRight = false;
        //        }
        //    }
        //}

        //public void Draw()
        //{
        //    for (int i = 0; i < Elements.Count; i++)
        //    {
        //        Console.SetCursorPosition(this.Elements[i].X, this.Elements[i].Y);
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.Write('@');
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //}

        //public void Delete()
        //{
        //    for (int i = 0; i < Elements.Count; i++)
        //    {
        //        Console.SetCursorPosition(this.Elements[i].X, this.Elements[i].Y);
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.Write(' ');
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //}

        //public void DrawFloors()
        //{
        //    for (int i = 0; i < TowerElements.Count; i++)
        //    {
        //        Console.SetCursorPosition(this.TowerElements[i].X, this.TowerElements[i].Y);
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.Write('X');
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //}

        //public void DeleteFloors()
        //{
        //    for (int i = 0; i < TowerElements.Count; i++)
        //    {
        //        Console.SetCursorPosition(this.TowerElements[i].X, this.TowerElements[i].Y);
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.Write(' ');
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //}

        //public void InputHandler()
        //{
        //    if (Console.KeyAvailable)
        //    {
        //        ConsoleKeyInfo userInput = Console.ReadKey();

        //        while (Console.KeyAvailable)
        //        {
        //            Console.ReadKey();
        //        }

        //        if (userInput.Key == ConsoleKey.Spacebar)
        //        {

        //            if (currentRow <= 18)
        //            {
        //                for (int i = 0; i < floorElementsLenght; i++)
        //                {
        //                    TowerElements.Add(new Vector2(this.Elements[i].X, this.Elements[i].Y));
        //                }

        //                Elements.Clear();

        //                for (int i = 0; i < floorElementsLenght; i++)
        //                {
        //                    Elements.Add(new Vector2(this.TowerElements[i].X, this.TowerElements[i].Y - currentRow));
        //                }

        //                currentRow++;
        //            }

        //            else
        //            {
        //                TowerElements.RemoveAll(e => e.Y == 47);

        //                for (int i = 0; i < TowerElements.Count; i++)
        //                {
        //                    TowerElements[i].Y += 1;
        //                }

        //                for (int i = 0; i < Elements.Count; i++)
        //                {
        //                    Elements[i].Y += 1;
        //                }

        //                for (int i = 0; i < floorElementsLenght; i++)
        //                {
        //                    TowerElements.Add(new Vector2(this.Elements[i].X, this.Elements[i].Y));
        //                }

        //                Elements.Clear();

        //                for (int i = 0; i < floorElementsLenght; i++)
        //                {
        //                    Elements.Add(new Vector2(this.TowerElements[i].X, this.TowerElements[i].Y - currentRow));
        //                }

        //            }
        //        }
        //    }
        //}

        public static void GenerateFloor()
        {
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
                }
            }

            else
            {
                if (Elements[startingRow, PLAYFIELD_WIDTH - 1] == 0)
                {
                    for (int i = PLAYFIELD_WIDTH - 1; i > 0; i--)
                    {
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
                        Console.Write('@');
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
                }
            }
        }
    }
}

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
        public List<Vector2> Elements;
        public static bool moveRight = false;
        public static int row = 1;
        public List<Vector2> TowerElements;

        public Floor()
        {
            this.Elements = new List<Vector2>();
            this.TowerElements = new List<Vector2>();

            for (int i = 0; i < floorElementsLenght; i++)
            {
                Elements.Add(new Vector2(PLAYFIELD_WIDTH/2- floorElementsLenght / 2+i, PLAYFIELD_HEIGHT-2-row));
            }
        }

        public void Move()
        {
            if (!moveRight)
            {
                if (this.Elements[0].X > 0)
                {
                    for (int i = 0; i < Elements.Count; i++)
                    {
                        this.Elements[i].X -= 1;
                    }
                }
                else if (this.Elements[0].X == 0)
                {
                    moveRight = true;
                }
            }
            else
            {
                if (this.Elements.Last().X < PLAYFIELD_WIDTH - 1)
                {
                    for (int i = 0; i < Elements.Count; i++)
                    {
                        this.Elements[i].X += 1;
                    }
                }
                else if (this.Elements.Last().X == PLAYFIELD_WIDTH-1)
                {
                    moveRight = false;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Console.SetCursorPosition(this.Elements[i].X, this.Elements[i].Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write('@');
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void Delete()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Console.SetCursorPosition(this.Elements[i].X, this.Elements[i].Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(' ');
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void DrawFloors()
        {
            for (int i = 0; i < TowerElements.Count; i++)
            {
                Console.SetCursorPosition(this.TowerElements[i].X, this.TowerElements[i].Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write('X');
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void DeleteFloors()
        {
            for (int i = 0; i < TowerElements.Count; i++)
            {
                Console.SetCursorPosition(this.TowerElements[i].X, this.TowerElements[i].Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(' ');
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void InputHandler()
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

                    if (row <= 18)
                    {
                        for (int i = 0; i < floorElementsLenght; i++)
                        {
                            TowerElements.Add(new Vector2(this.Elements[i].X, this.Elements[i].Y));
                        }

                        Elements.Clear();

                        for (int i = 0; i < floorElementsLenght; i++)
                        {
                            Elements.Add(new Vector2(this.TowerElements[i].X, this.TowerElements[i].Y - row));
                        }

                        row++;
                    }

                    else
                    {
                        TowerElements.RemoveAll(e => e.Y == 47);

                        for (int i = 0; i < TowerElements.Count; i++)
                        {
                            TowerElements[i].Y += 1;
                        }

                        for (int i = 0; i < Elements.Count; i++)
                        {
                            Elements[i].Y += 1;
                        }

                        for (int i = 0; i < floorElementsLenght; i++)
                        {
                            TowerElements.Add(new Vector2(this.Elements[i].X, this.Elements[i].Y));
                        }

                        Elements.Clear();

                        for (int i = 0; i < floorElementsLenght; i++)
                        {
                            Elements.Add(new Vector2(this.TowerElements[i].X, this.TowerElements[i].Y - row));
                        }

                    }
                }
            }
        }


    }
}

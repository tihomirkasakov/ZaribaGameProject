namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using static Floor;

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
        }

    }
}

namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static StartUp;
    using static Floor;

    public class Tower:Floor
    {
        public static List<string> Elements;
        public Vector2 Position;
        public List<Vector2> Floors;


        public Tower():base()
        {
            Elements = new List<string>
            {
                "XXXXXXXXXXX",
                @"/ \ / \ / \"
            };

            this.Position = new Vector2(PLAYFIELD_WIDTH / 2 - Elements[0].Length/2, PLAYFIELD_HEIGHT - Elements.Count);
        }

        public void Draw()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Console.SetCursorPosition(this.Position.X, this.Position.Y+i);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Elements[i]);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        //public void DrawTowerFloors()
        //{
        //    for (int i = 0; i < this.Floors.Count; i++)
        //    {
        //        Console.SetCursorPosition(this.Floors[i].X, this.Floors[i].Y);
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.Write('@');
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //}

    }
}

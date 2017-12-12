namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static StartUp;

    public class UI
    {
        public List<Vector2> Elements;

        public UI()
        {
            this.Elements = new List<Vector2>();

            for (int i = 0; i < PLAYFIELD_HEIGHT; i++)
            {
                Elements.Add(new Vector2(PLAYFIELD_WIDTH,i));
            }
        }

        public void Draw()
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                Console.SetCursorPosition(Elements[i].X, Elements[i].Y);
                Console.Write("|");
            }

            for (int i = 0; i < this.Elements.Count-2; i++)
            {
                Console.SetCursorPosition(Elements[i].X+1, Elements[i].Y);
                Console.Write(Elements.Count-i-2);
            }
        }

        public void Delete()
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                Console.SetCursorPosition(Elements[i].X, Elements[i].Y);
                Console.Write(" ");
            }
        }
    }
}

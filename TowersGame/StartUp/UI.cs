namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static StartUp;

    public class UI
    {
        public List<Vector2> Elements;

        public UI()
        {
            this.Elements = new List<Vector2>();

            for (int i = 0; i < PLAYFIELD_HEIGHT; i++)
            {
                Elements.Add(new Vector2(PLAYFIELD_WIDTH + 1, i));
            }
        }

        string[] theWholeUI = new string[]
        {
            "/^^^^^^^^^^^^^^\\",
            "| ___________  |",
            "| \\__    ___/  |",
            "|   |    |     |",
            "|   |    |     |",
            "|   |____|     |",
            "|    ____      |",
            "|   /  _ \\     |",
            "|  (  <_> )    |",
            "|   \\____/     |",
            "|  __      __  |",
            "| /  \\    /  \\ |",
            "| \\   \\/\\/   / |",
            "|  \\        /  |",
            "|   \\__/\\  /   |",
            "|        \\/    |",
            "| ___________  |",
            "| \\_   _____/  |",
            "|  |    __)_   |",
            "|  |        \\  |",
            "| /_______  /  |",
            "|         \\/   |",
            "| __________   |",
            "| \\______   \\  |",
            "|  |       _/  |",
            "|  |    |   \\  |",
            "|  |____|_  /  |",
            "|         \\/   |",
            "|              |",
            "|    Score:    |",
            "|              |",
            "|              |",
            "|   Floors:    |",
            "|              |",
            "|              |",
            "|  Difficulty: |",
            "|              |",
            "|              |",
            "| Leaderboard: |",
            "|              |",
            "|              |",
            "|              |",
            "|              |",
            "|              |",
            "|              |",
            "|              |",
            "|              |",
            "|              |",
            "\\vvvvvvvvvvvvvv/"
        };

        public void Draw(int score, Dictionary<int, string> leaderboard, Difficulty difficulty)
        {
            for (int i = 0; i < this.Elements.Count - 1; i++)
            {
                Console.SetCursorPosition(Elements[i].X, Elements[i].Y);
                Console.Write(theWholeUI[i]);
            }

            //displaying the score in the UI
            int scoreUIwidthDisplay = PLAYFIELD_WIDTH + 4;
            int scoreUIheightDisplay = 38;

            //this one is for increasing the height of the cursor and the numeration of the top 9 scorers
            int increaserAndDisplayer = 1;

            if (difficulty == Difficulty.Swing)
            {
                //this is for displaying the current ladder of the highest score users,
                //it should be used in the PLAYFIELD_UI
                foreach (var kvp in swingLeaderBoard.OrderByDescending(x => x.Key).Take(9))
                {
                    Console.SetCursorPosition(scoreUIwidthDisplay, scoreUIheightDisplay + increaserAndDisplayer);
                    Console.Write($"{increaserAndDisplayer}. {kvp.Value} - {kvp.Key}");
                    increaserAndDisplayer++;
                }
            }

            else if (difficulty == Difficulty.Glitch)
            {
                //this is for displaying the current ladder of the highest score users,
                //it should be used in the PLAYFIELD_UI
                foreach (var kvp in glitchLeaderBoard.OrderByDescending(x => x.Key).Take(9))
                {
                    Console.SetCursorPosition(scoreUIwidthDisplay, scoreUIheightDisplay + increaserAndDisplayer);
                    Console.Write($"{increaserAndDisplayer}. {kvp.Value} - {kvp.Key}");
                    increaserAndDisplayer++;
                }
            }

            else if (difficulty == Difficulty.Overfall)
            {
                //this is for displaying the current ladder of the highest score users,
                //it should be used in the PLAYFIELD_UI
                foreach (var kvp in overfallLeaderBoard.OrderByDescending(x => x.Key).Take(9))
                {
                    Console.SetCursorPosition(scoreUIwidthDisplay, scoreUIheightDisplay + increaserAndDisplayer);
                    Console.Write($"{increaserAndDisplayer}. {kvp.Value} - {kvp.Key}");
                    increaserAndDisplayer++;
                }
            }
        }

        public void UpdateUI(int score, int floors, Difficulty difficulty)
        {
            // 8 is used to adjust the position of the score - width
            // 30 is used to adjust the position of the score - height
            Console.SetCursorPosition(PLAYFIELD_WIDTH + 8, 30);
            Console.Write(score);

            // 8 is used to adjust the position of the score - width
            // 33 is used to adjust the position of the score - height
            Console.SetCursorPosition(PLAYFIELD_WIDTH + 8, 33);
            Console.Write(floors);

            // 7 is used to adjust the position of the difficulty - width
            // 36 is used to adjust the position of the difficulty - height
            Console.SetCursorPosition(PLAYFIELD_WIDTH + 7, 36);
            Console.Write(difficulty);
        }

        //I dont think that we need to delete the UI, just update it
        public void Delete()
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                Console.SetCursorPosition(Elements[i].X, Elements[i].Y);
                Console.Write(new string(' ', PLAYFIELD_UI - 2));
            }
        }
    }
}
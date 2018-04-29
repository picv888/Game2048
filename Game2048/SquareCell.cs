using System;
using ConsoleUIKit;

namespace Game2048 {
    public class SquareCell : ConsoleSpirit {
        int score;
        public int Score {
            get {
                return score;
            }
            set {
                if (this.score != value)
                {
                    this.score = value;
                    _isNeedToDraw = true;
                }
            }
        }
        public GameType gameType;

        public SquareCell() { }

        public SquareCell(int x, int y, int width, int height, ConsoleColor backgroundColor) : base(x, y, width, height, backgroundColor) { }

        public override void DrawSelf() {
            string space = "".PadRight(this.Frame.size.width);
            int scoreY = this.Frame.size.height / 2;
            ConsoleColor originalBC = Console.BackgroundColor;
            ConsoleColor originalFC = Console.ForegroundColor;
            for(int y = 0; y < this.Frame.size.height; y++) {
                Console.SetCursorPosition(this.ConsoleX, this.ConsoleY + y);
                Console.BackgroundColor = this.getCellColor();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(space);

                if(y == scoreY) {
                    Console.SetCursorPosition(this.ConsoleX, this.ConsoleY + y);
                    Console.Write("".PadRight((this.Frame.size.width - ("" + this.Score).Length) / 2));
                    Console.Write(this.Score == 0 ? "" : "" + this.Score);
                }
            }
            Console.BackgroundColor = originalBC;
            Console.ForegroundColor = originalFC;
            this._isNeedToDraw = false;
        }

        public ConsoleColor getCellColor() {
            ConsoleColor color = this.Score == 0 ? ConsoleColor.Gray : (ConsoleColor)(Math.Log(this.Score, 2) % 16);
            return color;
        }
    }
}

using System;
using ConsoleUIKit;

namespace Game2048 {
    /// <summary>
    /// 主菜单页面
    /// </summary>
    class MainSence : ConsoleSence {
        ConsoleSpirit button1;
        ConsoleSpirit button2;
        ConsoleSpirit button3;
        GameType gameType = GameType.Classical;

        public MainSence() { }

        public override void SenceDidLoad() {
            //游戏规则视图
            ConsoleSpirit ruleSp = new ConsoleSpirit((ConsoleApplication.appWidth - 36) / 2, 1, 36, 25, ConsoleColor.Gray, ConsoleColor.Black);
            ruleSp.FileName = "gameRule.txt";
            this.addSpirit(ruleSp);

            button1 = new ConsoleSpirit(13, 19, 9, 1, ConsoleColor.Gray);
            button1.Text = new string[] { "4 X 4 模式" };
            ruleSp.AddSubSpirit(button1);

            button2 = new ConsoleSpirit(13, 21, 9, 1, ConsoleColor.Gray);
            button2.Text = new string[] { "5 X 5 模式" };
            ruleSp.AddSubSpirit(button2);

            button3 = new ConsoleSpirit(13, 23, 9, 1, ConsoleColor.Gray);
            button3.Text = new string[] { "6 X 6 模式" };
            ruleSp.AddSubSpirit(button3);
            SetBackgroundColor();
        }

        public override void DidAcceptInput(ConsoleKeyInfo info) {
            base.DidAcceptInput(info);
            ConsoleKey key = info.Key;
            switch(key) {
                case ConsoleKey.Enter:
                    GameSence secondSence = new GameSence();
                    secondSence.gameType = this.gameType;
                    ConsoleApplication.PresentSence(secondSence);
                    break;
                case ConsoleKey.UpArrow:
                    this.gameType = (GameType)(((int)this.gameType - 1) < 0 ? 0 : (int)(this.gameType) - 1);
                    this.SetBackgroundColor();
                    break;
                case ConsoleKey.DownArrow:
                    this.gameType = (GameType)(((int)this.gameType + 1) > 2 ? 2 : (int)(this.gameType) + 1);
                    this.SetBackgroundColor();
                    break;
            }
        }

        void SetBackgroundColor() {
            button1.BackgroundColor = this.gameType == GameType.Classical ? ConsoleColor.Red : ConsoleColor.Gray;
            button2.BackgroundColor = this.gameType == GameType.FiveXFive ? ConsoleColor.Red : ConsoleColor.Gray;
            button3.BackgroundColor = this.gameType == GameType.SixXSix ? ConsoleColor.Red : ConsoleColor.Gray;
        }
    }
}

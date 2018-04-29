using System;
using System.Collections.Generic;
using ConsoleUIKit;

namespace Game2048 {
    class Program {
        static void Main(string[] args) {
            MainSence mainSence = new MainSence();
            ConsoleApplication.ShowRootSence(mainSence, 50, 25, args);
        }
    }
}

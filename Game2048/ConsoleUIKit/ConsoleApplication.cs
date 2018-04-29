using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleUIKit
{
    /// <summary>
    /// 模仿IOS的UIKit的UIApplication，管理事件循环，接收用户操作，给页面传递消息
    /// </summary>
    public static class ConsoleApplication
    {
        static List<ConsoleSence> sencesList;
        public static int appWidth;
        public static int appHeight;

        public static void ShowRootSence(ConsoleSence rootSence, int width, int height, string[] args)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = Console.BackgroundColor;
            appWidth = width;
            appHeight = height;
            //如果是window的控制台就设置窗口大小
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (width > Console.BufferWidth) //new Width is bigger then buffer
                {
                    Console.BufferWidth = width;
                    Console.WindowWidth = width;
                }
                else
                {
                    Console.WindowWidth = width;
                    Console.BufferWidth = width;
                }

                if (appHeight > Console.BufferWidth) //new Height is bigger then buffer
                {
                    Console.BufferHeight = appHeight;
                    Console.WindowHeight = height;
                }
                else
                {
                    Console.WindowHeight = height;
                    Console.BufferHeight = appHeight;
                }
            }

            sencesList = new List<ConsoleSence>(5);
            ConsoleApplication.PresentSence(rootSence);
            RunLoop();
        }

        /// <summary>
        /// 切换Sence，清屏，绘制目标Sence，目标Sence开始接收用户输入
        /// </summary>
        /// <param name="destinationSence">切换到的 sence.</param>
        public static void PresentSence(ConsoleSence destinationSence)
        {
            if (destinationSence == null)
            {
                throw new Exception("参数destinationSence不能为空");
            }

            //通知原Sence将要消失
            if (sencesList.Count > 0)
            {
                ConsoleSence originalSence = sencesList[sencesList.Count - 1];
                originalSence.SenceWillDisappear();
            }
            //显示目标Sence
            sencesList.Add(destinationSence);
            Console.Clear();
            destinationSence.SenceDidLoad();
            destinationSence.DrawSence();
        }

        public static void PopSence()
        {
            //通知原Sence将要消失
            if (sencesList.Count > 0)
            {
                ConsoleSence originalSence = sencesList[sencesList.Count - 1];
                originalSence.SenceWillDisappear();
                sencesList.RemoveAt(sencesList.Count - 1);
            }
            Console.Clear();
            //如果有上一个Sence的话，显示
            if (sencesList.Count > 0)
            {
                ConsoleSence previousSence = sencesList[sencesList.Count - 1];
                previousSence.SenceDidLoad();
                previousSence.DrawSence();
            }
        }

        static void RunLoop()
        {
            do
            {
                AcceptInput();
                RefreshSence();
            } while (true);
        }

        //接收用户输入
        static void AcceptInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo inputKeyInfo = Console.ReadKey(true);
                if (sencesList.Count > 0)
                {
                    ConsoleSence latestSence = sencesList[sencesList.Count - 1];
                    latestSence.DidAcceptInput(inputKeyInfo);
                }
            }
        }

        static void RefreshSence()
        {
            if (sencesList.Count > 0)
            {
                ConsoleSence latestSence = sencesList[sencesList.Count - 1];
                latestSence.RefreshSence();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleUIKit
{

    /// <summary>
    /// 模仿IOS的UIKit的UIView
    /// </summary>
    public class ConsoleSpirit
    {
        private Rect _frame;
        private int _consoleX;
        private int _consoleY;
        private Rect _oldFrame;
        private int _oldConsoleX;
        private int _oldConsoleY;
        private string[] _text;
        private string _fileName;//文件名，设置了后使用文件加载文本
        private ConsoleColor _backgroundColor;
        private ConsoleColor _foregroundColor;
        private List<ConsoleSpirit> _subSpirits = new List<ConsoleSpirit>(1);
        private ConsoleSpirit _superSpirit;

        protected bool _isNeedToDraw = false;
        protected bool _isNeedToClear = false;

        #region 属性
        public Rect Frame
        {
            set
            {
                int x = value.origin.x < 0 ? 0 : value.origin.x;
                int y = value.origin.y < 0 ? 0 : value.origin.y;
                Rect rectNew = new Rect(new Point(x, y), value.size);
                if (_frame != rectNew)
                {
                    _frame = rectNew;
                    _isNeedToDraw = true;
                    _isNeedToClear = true;
                }
                _consoleX = CalculateConsoleX(x);
                _consoleY = CalculateConsoleY(y);
            }
            get
            {
                return _frame;
            }
        }

        public Rect OldFrame
        {
            set
            {
                int x = value.origin.x < 0 ? 0 : value.origin.x;
                int y = value.origin.y < 0 ? 0 : value.origin.y;
                _oldFrame = new Rect(new Point(x, y), value.size);
                _oldConsoleX = CalculateConsoleX(x);
                _oldConsoleY = CalculateConsoleY(y);
            }
            get
            {
                return _frame;
            }
        }

        public int ConsoleX
        {
            get { return _consoleX; }
        }

        public int ConsoleY
        {
            get { return _consoleY; }
        }

        public int OldConsoleX
        {
            get { return _oldConsoleX; }
        }

        public int OldConsoleY
        {
            get { return _oldConsoleY; }
        }

        public string[] Text
        {
            set
            {
                if ((_text != value))
                {
                    _text = value;
                    _isNeedToDraw = true;
                }
                else if (_text != null && value != null)
                {
                    if (_text.Length != value.Length)
                    {
                        _text = value;
                        _isNeedToDraw = true;
                    }
                    else
                    {
                        for (int i = 0; i < value.Length; i++)
                        {
                            if (_text[i] != value[i])
                            {
                                _text = value;
                                _isNeedToDraw = true;
                            }
                        }
                    }
                }
            }
            get
            {
                return _text;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                string path = Environment.CurrentDirectory;
                //不同系统文件路径不一样
                if (System.Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    path += "\\" + value;
                }
                else
                {
                    path += "/" + value;
                }
                if (File.Exists(path))
                {
                    this.Text = System.IO.File.ReadAllLines(path);
                }

            }
        }

        public ConsoleColor BackgroundColor
        {
            set
            {
                if (_backgroundColor != value)
                {
                    _isNeedToDraw = true;
                    _backgroundColor = value;
                }
            }
            get { return _backgroundColor; }
        }

        public ConsoleColor ForegroundColor
        {
            set
            {
                if (_foregroundColor != value)
                {
                    _isNeedToDraw = true;
                    _foregroundColor = value;
                }
            }
            get { return _foregroundColor; }
        }

        #endregion

        #region 构造方法
        public ConsoleSpirit()
        {

        }

        public ConsoleSpirit(int x, int y, int width, int height, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Rect fr = new Rect(new Point(x, y), new Size(width, height));
            this.Frame = fr;
            this.OldFrame = fr;
            this.BackgroundColor = backgroundColor;
            this.ForegroundColor = foregroundColor;
        }

        public ConsoleSpirit(int x, int y, int width, int height, ConsoleColor backgroundColor) : this(x, y, width, height, backgroundColor, ConsoleColor.White) { }
        #endregion

        #region 和父精灵、子精灵相关的方法
        int CalculateConsoleX(int x)
        {
            int cx = x;
            ConsoleSpirit superSpirit = _superSpirit;
            while (superSpirit != null)
            {
                cx += superSpirit._frame.origin.x;
                superSpirit = superSpirit._superSpirit;
            }
            return cx;
        }

        int CalculateConsoleY(int y)
        {
            int cy = y;
            ConsoleSpirit superSpirit = _superSpirit;
            while (superSpirit != null)
            {
                cy += superSpirit._frame.origin.y;
                superSpirit = superSpirit._superSpirit;
            }
            return cy;
        }
        #endregion

        #region 关于绘制的方法
        public void DrawIfNeed()
        {
            if (_isNeedToDraw)
            {
                Draw();
            }
            else
            {
                //遍历子精灵绘制
                foreach (var spirit in _subSpirits)
                {
                    spirit.DrawIfNeed();
                }
            }
        }

        public void Draw()
        {
            if (_isNeedToClear)
            {
                Clear();
            }
            //绘制自身
            DrawSelf();
            //遍历子精灵绘制
            foreach (var spirit in _subSpirits)
            {
                spirit.Draw();
            }
            OldFrame = _frame;
        }

        public virtual void DrawSelf()
        {
            string space = "".PadRight(_frame.size.width);
            ConsoleColor originalBC = Console.BackgroundColor;
            ConsoleColor originalFC = Console.ForegroundColor;
            for (int y = 0; y < _frame.size.height; y++)
            {
                Console.SetCursorPosition(_consoleX, _consoleY + y);
                Console.BackgroundColor = _backgroundColor;
                Console.ForegroundColor = _foregroundColor;
                Console.Write(space);
                if (Text != null && Text.Length > y)
                {
                    Console.SetCursorPosition(_consoleX, _consoleY + y);
                    Console.Write(Text[y]);
                }
            }
            Console.BackgroundColor = originalBC;
            Console.ForegroundColor = originalFC;
            _isNeedToDraw = false;
        }

        public void Clear()
        {
            string space = "".PadRight(_oldFrame.size.width);
            ConsoleColor originalBC = Console.BackgroundColor;

            for (int y = 0; y < _oldFrame.size.height; y++)
            {
                Console.SetCursorPosition(_oldConsoleX, _oldConsoleY + y);
                Console.BackgroundColor = this._superSpirit == null ? ConsoleColor.Black : _superSpirit.BackgroundColor;
                Console.Write(space);
            }
            Console.BackgroundColor = originalBC;
            _isNeedToClear = false;
        }
        #endregion

        #region MyRegion
        public void AddSubSpirit(ConsoleSpirit subSpirit)
        {
            if (this == subSpirit)
                return;
            subSpirit._superSpirit = this;
            subSpirit.Frame = subSpirit.Frame;

            subSpirit.OldFrame = subSpirit.OldFrame;
            this._subSpirits.Add(subSpirit);
        }

        public void RemoveFromSuperSpirit()
        {
            ConsoleSpirit superSp = this._superSpirit;
            if (superSp != null)
            {
                superSp._subSpirits.Remove(this);
                this._superSpirit = null;
                superSp.DrawSelf();
            }
            else
            {
                this.Clear();
            }
        }
        #endregion
    }
}

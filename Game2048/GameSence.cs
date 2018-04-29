using System;
using ConsoleUIKit;
using System.Threading;

namespace Game2048 {
    public enum GameType {
        Classical,//经典模式
        FiveXFive,//5x5模式
        SixXSix,//6x6模式
    }

    struct Coordinate {
        public int x;
        public int y;
        public Coordinate(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    enum Direction {
        Up,
        Down,
        Left,
        Right,
    }

    public class GameSence : ConsoleSence {
        public GameType gameType = GameType.Classical;
        int row = 4;
        int column = 4;
        int[,] numbers;
        Random random = new Random();
        SquareCell[,] squareCells;
        ConsoleSpirit scoreSp;//积分板

        public GameSence() {
        }

        public override void SenceDidLoad() {
            base.SenceDidLoad();
            if(this.gameType == GameType.FiveXFive) {
                row = 5;
                column = 5;
            }
            else if(this.gameType == GameType.SixXSix) {
                row = 6;
                column = 6;
            }
            this.numbers = new int[column, row];
            this.squareCells = new SquareCell[column, row];

            //ESC
            ConsoleSpirit backSpirit = new ConsoleSpirit(1, 1, ConsoleApplication.appWidth - 1 - 1, 1, ConsoleColor.Gray);
            backSpirit.Text = new string[] { " << ESC键返回菜单  P键重新开始" };
            this.Spirits.Add(backSpirit);

            //计分板
            this.scoreSp = new ConsoleSpirit(33, 1, ConsoleApplication.appWidth - 1 - 33, 1, ConsoleColor.Gray, ConsoleColor.Black);
            this.scoreSp.Text = new string[] { "当前分数 : 0" };
            this.addSpirit(scoreSp);

            //方格
            for(int y = 0; y < row; y++) {
                for(int x = 0; x < column; x++) {
                    SquareCell cell = new SquareCell((ConsoleApplication.appWidth - 2*(column-1) - 6 * column) / 2 + x * 8, 3 + y * 4, 6, 3, ConsoleColor.Cyan);
                    cell.Score = this.numbers[x, y];
                    cell.gameType = this.gameType;
                    this.addSpirit(cell);
                    this.squareCells[x, y] = cell;
                }
            }

            AddNumber(numbers, 0, 2);
            AddNumber(numbers, 0, 2);
        }

        public override void DidAcceptInput(ConsoleKeyInfo info) {
            base.DidAcceptInput(info);
            ConsoleKey key = info.Key;
            switch(key) {
                case ConsoleKey.Escape:
                    ConsoleApplication.PopSence();
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    {
                        move(Direction.Up);
                        break;
                    }
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    {
                        move(Direction.Down);
                        break;
                    }
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    {
                        move(Direction.Left);
                        break;
                    }
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    {
                        move(Direction.Right);
                        break;
                    }
                case ConsoleKey.P:
                    {
                        numbers = new int[column, row];
                        for(int y = 0; y < row; y++) {
                            for(int x = 0; x < column; x++) {
                                squareCells[x, y].Score = numbers[x, y];
                            }
                        }
                        AddNumber(numbers, 0, 2);
                        AddNumber(numbers, 0, 2);
                        break;
                    }
            }
        }

        void move(Direction direction) {
            int[,] numbersNew = moveWithFold(numbers, direction);
            if(!IsIdentical(numbers, numbersNew)) {
                numbers = numbersNew;
                for(int y = 0; y < row; y++) {
                    for(int x = 0; x < column; x++) {
                        squareCells[x, y].Score = numbers[x, y];
                    }
                }
                AddNumber(numbers, 0, 2);
            }
            ////绘制
            this.Judge();
        }

        //判断是否输了
        void Judge() {
            int[,] numbers1 = moveWithFold(numbers, Direction.Up);
            int[,] numbers2 = moveWithFold(numbers, Direction.Left);
            int[,] numbers3 = moveWithFold(numbers, Direction.Right);
            int[,] numbers4 = moveWithFold(numbers, Direction.Down);
            if(IsIdentical(numbers, numbers1) && IsIdentical(numbers, numbers2) &&
                IsIdentical(numbers, numbers3) && IsIdentical(numbers, numbers4)) {
                this.scoreSp.Text = new string[] { "失败 当前分数 ： " + sum() };
            }
            else {
                this.scoreSp.Text = new string[] { "当前分数 ： " + sum() };
            }
        }

        //自动增加2，并刷新页面
        void AutoAdd2(object obj) {
            this.AddNumber(numbers, 0, 2);
        }

        //计算所有数字的和
        int sum() {
            int sum = 0;
            foreach(var num in numbers) {
                sum += num;
            }
            return sum;
        }

        //运动，合并相同的数字，返回数组
        int[,] moveWithFold(int[,] numberArray, Direction direction) {
            int[,] numberArrayNew = new int[column, row];

            int constraint = (direction == Direction.Up || direction == Direction.Down) ? column : row;
            int count = (direction == Direction.Up || direction == Direction.Down) ? row : column;

            for(int i = 0; i < constraint; i++) {
                //获取一列或一行的数字
                int[] arrayForRowOrColumn = new int[count];
                for(int j = 0; j < count; j++) {
                    int num = (direction == Direction.Up) ? numberArray[i, j] : (direction == Direction.Down) ? numberArray[i, count - 1 - j] :
                     (direction == Direction.Left) ? numberArray[j, i] : numberArray[count - 1 - j, i];
                    arrayForRowOrColumn[j] = num;
                }
                arrayForRowOrColumn = removeZeroAndFold(arrayForRowOrColumn);
                for(int k = 0; k < count; k++) {
                    if(direction == Direction.Up) {
                        numberArrayNew[i, k] = arrayForRowOrColumn[k];
                    }
                    else if(direction == Direction.Down) {
                        numberArrayNew[i, count - 1 - k] = arrayForRowOrColumn[k];
                    }
                    else if(direction == Direction.Left) {
                        numberArrayNew[k, i] = arrayForRowOrColumn[k];
                    }
                    else {
                        numberArrayNew[count - 1 - k, i] = arrayForRowOrColumn[k];
                    }
                }
            }
            return numberArrayNew;
        }

        //删除0合并相同数字然后返回新的数组
        int[] removeZeroAndFold(int[] numArray) {
            //去掉0
            int[] numArrayWithoutZero = new int[row];
            int count = 0;
            for(int i = 0; i < numArray.Length; i++) {
                int n = numArray[i];
                if(n != 0) {
                    numArrayWithoutZero[count] = n;
                    ++count;
                }
            }

            //合并相邻相同项
            int index = 0;
            int[] arrayAfterFold = new int[row];
            do {
                int num = numArrayWithoutZero[index];
                int num2 = numArrayWithoutZero[index + 1];
                if(num == num2) {
                    arrayAfterFold[index] = num * 2;
                    arrayAfterFold[index + 1] = 0;
                    index += 2;
                }
                else {
                    arrayAfterFold[index] = num;
                    index++;
                }
                if(index == count - 1) {
                    arrayAfterFold[index] = numArrayWithoutZero[count - 1];
                    break;
                }
                else if(index >= count) {
                    break;
                }
            } while(true);

            //去掉0
            int m = 0;
            int[] numArrayNew = new int[numArray.Length];
            for(int i = 0; i < row; i++) {
                if(arrayAfterFold[i] != 0) {
                    numArrayNew[m] = arrayAfterFold[i];
                    m++;
                }
            }
            return numArrayNew;
        }

        //判断两个数组是否一样
        bool IsIdentical(int[,] a, int[,] b) {
            if(a.GetLength(0) != b.GetLength(0)) {
                return false;
            }
            if(a.GetLength(1) != b.GetLength(1)) {
                return false;
            }
            for(int y = 0; y < a.GetLength(1); y++) {
                for(int x = 0; x < a.GetLength(0); x++) {
                    if(a[x, y] != b[x, y]) {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 在 numberArray 的所有元素里，随机选一个值为number的数字赋值为number2，并返回这个数字的坐标，若没有值为0的数字则返回坐标(-1, -1)
        /// </summary>
        /// <param name="numberArray">在 numberArray 的所有元素里</param>
        /// <param name="number">随机选一个值为number的数字</param>
        /// <param name="number2">赋值为number2</param>
        /// <returns>返回这个数字的坐标，若没有值为0的数字则返回坐标(-1, -1)</returns>
        Coordinate AddNumber(int[,] numberArray, int number, int number2) {
            Coordinate coordinate = GetRandomCoordinate(numberArray, number);
            if(coordinate.x != -1 && coordinate.y != -1) {
                numberArray[coordinate.x, coordinate.y] = number2;
                this.squareCells[coordinate.x, coordinate.y].Score = number2;
            }
            return coordinate;
        }

        //在 numberArray 的所有元素里，随机选一个值为num的，返回它的坐标，没有这个数字则返回坐标(-1, -1)
        Coordinate GetRandomCoordinate(int[,] numberArray, int number) {
            Coordinate coordinate = new Coordinate(-1, -1);
            int[] indexXArray = new int[row * column];
            int[] indexYArray = new int[row * column];
            int count = 0;

            for(int y = 0; y < row; y++) {
                for(int x = 0; x < column; x++) {
                    if(numberArray[x, y] == number) {
                        indexXArray[count] = x;
                        indexYArray[count] = y;
                        count++;
                    }
                }
            }
            int randomIndex = random.Next(0, count);
            coordinate.x = indexXArray[randomIndex];
            coordinate.y = indexYArray[randomIndex];
            return coordinate;
        }
    }
}

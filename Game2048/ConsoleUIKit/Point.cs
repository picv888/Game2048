﻿namespace ConsoleUIKit {
    public struct Point {
        public int x;
        public int y;
        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Point a, Point b) {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Point a, Point b) {
            return !(a == b);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
    }
}

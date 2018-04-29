namespace ConsoleUIKit {
    public struct Rect {
        public Point origin;
        public Size size;
        public Rect(Point origin, Size size) {
            this.origin = origin;
            this.size = size;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public static bool operator ==(Rect a, Rect b) {
            return a.origin == b.origin && a.size == b.size;
        }

        public static bool operator !=(Rect a, Rect b) {
            return !(a == b);
        }
    }
}

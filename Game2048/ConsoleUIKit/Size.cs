namespace ConsoleUIKit {
    public struct Size {
        public int width;
        public int height;
        public Size(int width, int height) {
            this.width = width;
            this.height = height;
        }

        public static bool operator ==(Size a, Size b) {
            return a.width == b.width && a.height == b.height;
        }

        public static bool operator !=(Size a, Size b) {
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

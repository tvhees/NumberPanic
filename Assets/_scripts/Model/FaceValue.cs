namespace Model
{
    [System.Serializable]
    public class FaceValue
    {
        public readonly int Value;
        public readonly string Text;

        public FaceValue(int value, string text)
        {
            Value = value;
            Text = text;
        }

        public FaceValue(int value)
        {
            Value = value;
            Text = value.ToString();
        }

        public static FaceValue Zero()
        {
            return new FaceValue(0);
        }

        protected bool Equals(FaceValue other)
        {
            return Value == other.Value && Text == other.Text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((FaceValue) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value * 397) ^ (Text != null ? Text.GetHashCode() : 0);
            }
        }

        public static bool operator ==(FaceValue x, FaceValue y)
        {
            return x.Value == y.Value && x.Text == y.Text;
        }

        public static bool operator !=(FaceValue x, FaceValue y)
        {
            return !(x == y);
        }
    }
}
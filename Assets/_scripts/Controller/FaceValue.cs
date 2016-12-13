using System.Diagnostics;
using JetBrains.Annotations;

namespace Assets._scripts.Controller
{
    [System.Serializable]
    public class FaceValue
    {
        protected bool Equals(FaceValue other)
        {
            return Value == other.Value && string.Equals(Text, other.Text);
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

        public int Value;
        public string Text;

        public static bool operator ==(FaceValue x, FaceValue y)
        {
            Debug.Assert(x != null, "x != null");
            Debug.Assert(y != null, "y != null");
            return x.Value == y.Value && x.Text == y.Text;
        }

        public static bool operator !=(FaceValue x, FaceValue y)
        {
            return !(x == y);
        }
    }
}
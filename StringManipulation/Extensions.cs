using System.Text;

namespace StringManipulation
{
    public static class Extensions
    {
        private static readonly char[] _UPPERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] _LOWERS = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        public static string Opposite(this string input)
        {
            return string.Join(null, input.ToCharArray().Select(c => ReverseChar(c)).ToArray());
        }

        private static char ReverseChar(char c)
        {
            if (_UPPERS.Contains(c)) return _UPPERS.ReverseInArray(c);
            if (_LOWERS.Contains(c)) return _LOWERS.ReverseInArray(c);
            return c;
        }

        public static T ReverseInArray<T>(this T[] array, T obj)
        {
            if (!array.Contains(obj)) return obj;
            int index = array.FindIndexOf(obj);
            return array[^(index + 1)];
        }

        public static int FindIndexOf<T>(this T[] source, T obj)
        {
            int returnVal = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Equals(obj))
                {
                    returnVal = i;
                    break;
                }
            }
            return returnVal;
        }

        public static int[] FindIdexesOf<T>(this T[] source, T obj)
        {
            List<int> returnVal = new List<int>();
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Equals(obj))
                {
                    returnVal.Add(i);
                }
            }
            return returnVal?.ToArray() ?? new int[0];
        }
    }
}
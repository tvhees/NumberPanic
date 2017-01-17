using System;

namespace Model
{
    public class Data {
        public NumberArrays Numbers = new NumberArrays();
        public TextArrays Texts = new TextArrays();

        public Data()
        {
            AssignArrays();
        }

        public void AssignArrays()
        {
            Numbers.Primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541 };
            Numbers.Fibbonaci = new int[100];
            Numbers.Fibbonaci[0] = 1;
            Numbers.Fibbonaci[1] = 2;
            for (var i = 2; i < 100; i++)
            {
                Numbers.Fibbonaci[i] = Numbers.Fibbonaci[i - 2] + Numbers.Fibbonaci[i - 1];
            }
            Numbers.Pi = new[]
            {
                3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8, 9, 7, 9, 3,
                2, 3, 8, 4, 6, 2, 6, 4, 3, 3, 8, 3, 2, 7, 9, 5, 0, 2, 8, 8, 4, 1, 9,
                7, 1, 6, 9, 3, 9, 9, 3, 7, 5, 1, 0, 5, 8, 2, 0, 9, 7, 4, 9, 4, 4, 5,
                9, 2, 3, 0, 7, 8, 1, 6, 4, 0, 6, 2, 8, 6, 2, 0, 8, 9, 9, 8, 6, 2, 8,
                0, 3, 4, 8, 2, 5, 3, 4, 2, 1, 1, 7, 0, 6, 7, 9
            };

            Texts.Alphabet = new[]
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p",
                "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
            };
            Texts.EnglishWords = new[] { "the", "be", "and", "of", "a", "in", "to", "have", "it", "I", "that", "for", "you", "he", "with", "on", "do", "say", "this", "they", "at", "but", "we", "his", "from", "not", "by", "she", "or", "as", "what", "go", "their", "can", "who", "get", "if", "would", "her", "all", "my", "make", "about", "know", "will", "up", "one", "time", "there", "year", "so", "think", "when", "which", "them", "some" };
            Texts.AusAnthem = new[] { "australians", "all", "let", "us", "rejoice", "for", "we", "are", "young", "and", "free",
                "we've", "golden", "soil", "and", "wealth", "for", "toil", "our", "home", "is", "girt", "by", "sea",
                "our", "land", "abounds", "in", "nature's", "gifts", "of", "beauty", "rich", "and", "rare",
                "in", "history's", "page", "let", "every", "stage", "advance", "australia", "fair",
                "in", "joyful", "strains", "then", "let", "us", "sing", "advance", "australia", "fair",
                "beneath", "our", "radiant", "southern", "cross", "we'll", "toil", "with", "hearts", "and", "hands",
                "to", "make", "this", "commonwealth", "of", "ours", "renowned", "of", "all", "the", "lands",
                "for", "those", "who've", "come", "across", "the", "seas", "we've", "boundless", "plains", "to", "share",
                "with", "courage", "let", "us", "all", "combine", "to", "advance", "australia", "fair",
                "in", "joyful", "strains", "then", "let", "us", "sing", "advance", "australia", "fair"
            };
        }
    }

    [Serializable]
    public class NumberArrays
    {
        public int[] Primes;
        public int[] Fibbonaci;
        public int[] Pi;
    }

    [Serializable]
    public class TextArrays
    {
        public string[] Alphabet;
        public string[] EnglishWords;
        public string[] AusAnthem;
    }
}
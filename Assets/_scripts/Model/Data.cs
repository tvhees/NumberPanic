using System;
using System.Collections;
using UnityEngine;

namespace _scripts.Model
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
            for (int i = 2; i < 100; i++)
            {
                Numbers.Fibbonaci[i] = Numbers.Fibbonaci[i - 2] + Numbers.Fibbonaci[i - 1];
            }

            Texts.EnglishWords = new string[] { "the", "be", "and", "of", "a", "in", "to", "have", "it", "I", "that", "for", "you", "he", "with", "on", "do", "say", "this", "they", "at", "but", "we", "his", "from", "not", "by", "she", "or", "as", "what", "go", "their", "can", "who", "get", "if", "would", "her", "all", "my", "make", "about", "know", "will", "up", "one", "time", "there", "year", "so", "think", "when", "which", "them", "some" };
            Texts.AusAnthem = new string[] { "australians", "all", "let", "us", "rejoice", "for", "we", "are", "young", "and", "free", "we've", "golden", "soil", "and", "wealth", "for", "toil", "our", "home", "is", "girt", "by", "sea" };
        }
    }

    [Serializable]
    public class NumberArrays
    {
        public int[] Primes;
        public int[] Fibbonaci;
    }

    [Serializable]
    public class TextArrays
    {
        public string[] EnglishWords;
        public string[] AusAnthem;
    }
}
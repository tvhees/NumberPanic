using UnityEngine;
using System;
using System.Collections;

public class Data {
   public NumberArrays numberArrays = new NumberArrays();
    public TextArrays textArrays = new TextArrays();

    public void AssignArrays()
    {
        numberArrays.primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541 };
        numberArrays.fibbonaci = new int[100];
        numberArrays.fibbonaci[0] = 1;
        numberArrays.fibbonaci[1] = 2;
        for (int i = 2; i < 100; i++)
        {
            numberArrays.fibbonaci[i] = numberArrays.fibbonaci[i - 2] + numberArrays.fibbonaci[i - 1];
        }

        textArrays.englishWords = new string[] { "the", "be", "and", "of", "a", "in", "to", "have", "it", "I", "that", "for", "you", "he", "with", "on", "do", "say", "this", "they", "at", "but", "we", "his", "from", "not", "by", "she", "or", "as", "what", "go", "their", "can", "who", "get", "if", "would", "her", "all", "my", "make", "about", "know", "will", "up", "one", "time", "there", "year", "so", "think", "when", "which", "them", "some" };
        textArrays.ausAnthem = new string[] { "australians", "all", "let", "us", "rejoice", "for", "we", "are", "young", "and", "free", "we've", "golden", "soil", "and", "wealth", "for", "toil", "our", "home", "is", "girt", "by", "sea" };
    }
}

[Serializable]
public class NumberArrays
{
    public int[] primes;
    public int[] fibbonaci;
}

[Serializable]
public class TextArrays
{
    public string[] englishWords;
    public string[] ausAnthem;
}

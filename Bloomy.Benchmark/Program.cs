using Bloomy.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Bloomy.Benchmark
{
    class Program
    {
        const int FilterWidth = 1000000;

        static List<string> LoadWords()
        {
            List<string> words = new List<string>();
            using (TextReader tr = new StreamReader(@"wordsList.txt"))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                    words.Add(line);
            }
            return words;
        }

        static List<string> GetRandom(int v, List<string> words)
        {
            List<string> randomWords = new List<string>();
            Random r = new Random();
            for (int i = 0; i < v; ++i)
                randomWords.Add(words[r.Next(0, words.Count)]);
            return randomWords;
        }

        static BasicFilter FilterLoadMurmur(List<string> words, ushort hashNumber)
        {
            var watch = new Stopwatch();
            BasicFilter filter = new BasicFilter(FilterWidth, HashFunc.Murmur3, hashNumber);
            watch.Start();
            foreach (string w in words)
                filter.Insert(w);
            watch.Stop();
            Console.WriteLine($"Murmur3 ({hashNumber} times): {(double)watch.ElapsedMilliseconds / 1000} sec");
            return filter;
        }

        static BasicFilter FilterLoadSha256(List<string> words, ushort hashNumber)
        {
            var watch = new Stopwatch();
            BasicFilter filter = new BasicFilter(FilterWidth, HashFunc.SHA256, hashNumber);
            watch.Start();
            foreach (string w in words)
                filter.Insert(w);
            watch.Stop();
            Console.WriteLine($"SHA256 ({hashNumber} times): {(double)watch.ElapsedMilliseconds / 1000} sec");
            return filter;
        }

        static BasicFilter FilterLoadSha512(List<string> words, ushort hashNumber)
        {
            var watch = new Stopwatch();
            BasicFilter filter = new BasicFilter(FilterWidth, HashFunc.SHA512, hashNumber);
            watch.Start();
            foreach (string w in words)
                filter.Insert(w);
            watch.Stop();
            Console.WriteLine($"SHA512 ({hashNumber} times): {(double)watch.ElapsedMilliseconds / 1000} sec");
            return filter;
        }

        static void FilterCheck(BasicFilter filter, List<string> words)
        {
            var watch = new Stopwatch();
            double p = 0.0;
            foreach (string w in words)
            {
                var res = filter.Check(w);
                p = res.Probability;
            }
            watch.Stop();
            Console.WriteLine($"{filter.HashFunction}, k={filter.HashNumber}, {words.Count} checks: {(double)watch.ElapsedMilliseconds / 1000} sec with p={p}");
        }


        static void Main(string[] args)
        {
            List<string> words = LoadWords();
            List<string> wordsSlice = GetRandom(50000, words);
            Console.WriteLine($"INSERTING {words.Count} WORDS");
            Console.WriteLine($"================================");
            var murmurFilter2 = FilterLoadMurmur(words, 2);
            var sha256Filter2 = FilterLoadSha256(words, 2);
            var sha512Filter2 = FilterLoadSha512(words, 2);
            Console.WriteLine($"================================");
            var murmurFilter5 = FilterLoadMurmur(words, 5);
            var sha256Filter5 = FilterLoadSha256(words, 5);
            var sha512Filter5 = FilterLoadSha512(words, 5);
            Console.WriteLine($"================================");
            var murmurFilter9 = FilterLoadMurmur(words, 9);
            var sha256Filter9 = FilterLoadSha256(words, 9);
            var sha512Filter9 = FilterLoadSha512(words, 9);

            Console.WriteLine($"\n\nCHECK EXISTING WORDS");
            Console.WriteLine($"===============================================================");
            FilterCheck(murmurFilter2, wordsSlice);
            FilterCheck(sha256Filter2, wordsSlice);
            FilterCheck(sha512Filter2, wordsSlice);
            Console.WriteLine($"===============================================================");
            FilterCheck(murmurFilter5, wordsSlice);
            FilterCheck(sha256Filter5, wordsSlice);
            FilterCheck(sha512Filter5, wordsSlice);
            Console.WriteLine($"===============================================================");
            FilterCheck(murmurFilter9, wordsSlice);
            FilterCheck(sha256Filter9, wordsSlice);
            FilterCheck(sha512Filter9, wordsSlice);
        }

    }
}

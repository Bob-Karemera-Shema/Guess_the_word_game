using System;
using System.IO;
using System.Collections.Generic;

namespace Word_Family_Guess_the_word_game_
{
    class Family
    {
        private static Random random = new Random();
        private static string input;
        private static bool win;
        private static List<string> gameAnswers; //list to hold dictionary words
        private static int wordSize;
        private static List<string> guessDisplay;
        private static List<string> match;
        private static List<string> notMatch;
        private static int nbrOfAttempts;
        private static int initialWordIndex;
        private static List<string> family1;
        private static List<string> family2;
        private static List<string> family3;
        private static List<string> family4;
        private static List<string> family5;
        private static List<string> family6;
        private static List<string> family7;

        public static void Initialise()
        {

            wordSize = random.Next(4, 12);
            gameAnswers = new List<string>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader("dictionary.txt"))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length == wordSize)
                        {
                            gameAnswers.Add(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            win = false;

            guessDisplay = new List<string>(wordSize);
            for (int i = 0; i < wordSize; i++)
            {
                guessDisplay.Add("_");
            }
            nbrOfAttempts = wordSize * 2;
            match = new List<string>();
            notMatch = new List<string>();
            initialWordIndex = random.Next(wordSize);

            GameIntro();
        }

        public static void GameIntro()
        {
            Console.WriteLine("Discover the word in " + nbrOfAttempts + " attempts by guessing using letters.");
            Console.WriteLine("This word has: " + wordSize + " letters. Good luck!");
            GamePlay();
        }

        public static void GameInput()
        {
            Console.Write("Player Guess: ");
            foreach (string letter in guessDisplay)
            {
                Console.Write(letter);
            }
            Console.WriteLine();
            Console.Write("Enter your guess: ");
            input = Console.ReadLine().ToUpper();
            Console.WriteLine();
        }

        public static void DictionaryCheck()
        {
            for (int x = 0; x < gameAnswers.Count; x++)
            {
                if (gameAnswers[x].Contains(input))
                {
                    match.Add(gameAnswers[x]);
                }
                else
                {
                    notMatch.Add(gameAnswers[x]);
                }
            }
        }

        public static void CheckFamilies()
        {
            int count = 0;
            string currentWord;

            for (int i = 0;i < match.Count;i++)
            {
                count = 0;
                currentWord = match[i];
                if (currentWord[initialWordIndex] == input[0])
                {
                    for (int j = 0; j < match[i].Length; j++)
                    {
                        if (currentWord[j].Equals(input[0]))
                        {
                            count++;
                        }
                        UpdateFamilies(count, gameAnswers[i]);
                    }
                }
            }
            BiggestFamily();
        }

        public static void UpdateFamilies(int count, string word)
        {
            if (count == 1) { family1.Add(word); }
            if (count == 2) { family2.Add(word); }
            if (count == 3) { family3.Add(word); }
            if (count == 4) { family4.Add(word); }
            if (count == 5) { family5.Add(word); }
            if (count == 6) { family6.Add(word); }
            if (count == 7) { family7.Add(word); }
        }

        public static void BiggestFamily()
        {
            int max = family1.Count;
            match.Clear();
            match = family1;

            if (max <= family2.Count) { max = family2.Count; match = family2; }

            if (max <= family3.Count) { max = family3.Count; match = family3; }

            if (max <= family4.Count) { max = family4.Count; match = family4; }

            if (max <= family5.Count) { max = family5.Count; match = family5; }

            if (max <= family6.Count) { max = family6.Count; match = family6; }

            if (max <= family7.Count) { max = family7.Count; match = family7; }
        }

        public static void GamePlay()
        {
            while (!win & nbrOfAttempts != 0)
            {
                match.Clear();
                notMatch.Clear();
                
                GameInput();
                DictionaryCheck();
                CheckFamilies();

                if (iteration > 0) { LetterFill(); }

                if (match.Count < notMatch.Count)
                {
                    gameAnswers.Clear();
                    UpdateWordFamily(match);

                    Console.WriteLine("Correct!");
                    char guess = input[0];
                    for (int i = 0; i < guessDisplay.Count; i++)
                    {
                        if (i == chosenIndex & guessDisplay[i] == "_")
                        {
                            guessDisplay[i] = input;
                        }
                    }

                    if (guessDisplay.Contains("_") == false)
                    {
                        win = true;
                        Console.WriteLine("The chosen word is " + gameAnswers[random.Next(gameAnswers.Count)]);
                    }
                }
                else
                {
                    gameAnswers.Clear();
                    UpdateWordFamily(notMatch);
                    Console.WriteLine("Incorrect guess! Try again");
                }
                nbrOfAttempts--;
            }

            GameConclude();
        }

        public static void UpdateWordFamily(List<string> newList)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                gameAnswers.Add(newList[i]);
            }
        }

        public static void LetterFill()
        {
            string word;
            bool correct = true;
            for (int i = 0; i < match.Count; i++)
            {
                word = match[i];
                for (int j = 0; j < guessDisplay.Count; j++)
                {
                    if (guessDisplay[j] != "_")
                    {
                        if (guessDisplay[j].Equals(word[j]))
                        { correct = true; }
                        else { correct = false; }
                    }
                }

                if (correct == false)
                {
                    notMatch.Add(word);
                    match.RemoveAt(i);
                }
            }
        }

        public static void GameConclude()
        {
            if (win)
            {
                Console.WriteLine("YOU WON! Press any key to quit.");
                Console.ReadKey();
                System.Environment.Exit(0);
            }
        }
    }

using System;
using System.IO;
using System.Collections.Generic;

namespace Word_Family_Guess_the_word_game_
{
    class WordFamily
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
        private static List<string> family1 = new List<string> {"a"};
        private static List<string> family2 = new List<string> { "a" };
        private static List<string> family3 = new List<string> { "a" };
        private static List<string> family4 = new List<string> { "a" };
        private static List<string> family5 = new List<string> { "a" };
        private static List<string> family6 = new List<string> { "a" };
        private static List<string> family7 = new List<string> { "a" };
        private static List<string> biggestFamilyCharRep = new List<string>();
        private static List<string> biggestIndexFamily = new List<string>();
        private static List<string> inputMatchProgress = new List<string>();
        private static int updateStatus = 0;

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
            for (int j = 0; j < wordSize; j++)
            {

            }

            nbrOfAttempts = wordSize * 2;
            match = new List<string>();
            notMatch = new List<string>();
            initialWordIndex = random.Next(wordSize);

            GameIntro();
        }

        public static void GameIntro()
        {
            Console.Clear();
            Console.WriteLine("Discover the word in " + nbrOfAttempts + " attempts by guessing using letters.");
            Console.WriteLine("This word has: " + wordSize + " letters. Good luck!");
            Console.WriteLine("Enter a letter between A-Z");
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

            if (((int)input[0] > 96 & (int)input[0] < 123))
            {
                Console.WriteLine("Your input was not a letter of the alphabet!");
                Console.WriteLine("Enter a letter between A-Z");
                GameInput();
            }
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
            int bigFamily = 0;
            int[] familyArray = new int[7];
            string currentWord;

            for (int x = 0;x < familyArray.Length;x++)
            {
                familyArray[x] = 0;
            }

            for (int i = 0; i < match.Count; i++)
            {
                count = 0;
                currentWord = match[i];
                for (int j = 0; j < match[i].Length; j++)
                {
                    if (currentWord[j].Equals(input[0]))
                    {
                        count++;
                    }
                }
                UpdateFamilies(count, ref familyArray);
            }

            bigFamily = GetMax(familyArray);
            UpdateBigCharFamily(bigFamily);
            CheckIndexFamily();
        }

        public static void UpdateFamilies(int count, ref int[] families)
        {
            if (count == 1) { families[count--] = count; }
            else if (count == 2) { families[count--] = count; }
            else if (count == 3) { families[count--] = count; }
            else if (count == 4) { families[count--] = count; }
            else if (count == 5) { families[count--] = count; }
            else if (count == 6) { families[count--] = count; }
            else { families[count--] = count; }
        }

        public static void CheckIndexFamily()
        {
            int maxIndex = 0;
            int[] indexArray = new int[guessDisplay.Count];

            for (int x = 0; x < wordSize; x++)
            {
                indexArray[x] = 0;
            }

            for (int i = 0; i < biggestFamilyCharRep.Count; i++)
            {
                for (int j = 0; j < guessDisplay.Count; j++)
                {
                    if (biggestFamilyCharRep[i][j].Equals(input[0]) & (guessDisplay[i] == "_"))
                    {
                        indexArray[i]++;
                    }
                }
            }

            maxIndex = GetMax(indexArray);
            UpdateBigIndexFamily(maxIndex);
        }

        public static int GetMax(int[] array)
        {
            int max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max) { max = array[i]; }
            }
            return max;
        }

        public static void UpdateBigCharFamily(int count)
        {
            int index = 0;

            for (int x = 0;x < match.Count; x++)
            {
                index = 0;
                for(int i = 0;i < match[x].Length; i++)
                {
                    if(match[x][i].Equals(input[0]))
                    { index++; }
                }

                if(index == count)
                { biggestFamilyCharRep.Add(match[x]);}
            }
        }

        public static void UpdateBigIndexFamily(int index)
        {
            string currentWord;

            for (int x = 0; x < wordSize; x++)
            {
                if (x == index)
                {
                    for (int i = 0; i < biggestFamilyCharRep.Count; i++)
                    {
                        currentWord = biggestFamilyCharRep[i];
                        if (currentWord[index].Equals(input[0]))
                        {
                            biggestIndexFamily.Add(currentWord);
                        }
                    }
                }
            }
        }

        public static void UpdateGuessProgress()
        {
            int count = 0;
            for(int x = 0; x<guessDisplay.Count; x++)
            {
                if(guessDisplay[x] == "_") 
                { count++; }
            }

            if (count == guessDisplay.Count)
            {
                for (int a = 0; a < biggestIndexFamily.Count; a++)
                {
                    for (int b = 0; b < guessDisplay.Count; b++)
                    {
                        if (biggestIndexFamily[a][b].Equals(input[0]))
                        {
                            guessDisplay[b] = input;
                        }
                    }
                }
            }

            else
            {
                for (int j = 0; j < biggestIndexFamily.Count; j++)
                {
                    for (int i = 0; i < guessDisplay.Count; i++)
                    {
                        if (guessDisplay[i] != "_")
                        {
                            if (biggestIndexFamily[j][i].Equals(guessDisplay[i]))
                            {
                                inputMatchProgress.Add(biggestIndexFamily[j]);
                            }
                        }
                    }
                }
            }

            for (int c = 0; c < inputMatchProgress.Count; c++)
            {
                for(int d = 0; d < guessDisplay.Count; d++)
                {
                    if(inputMatchProgress[c][d].Equals(input[0]))
                    { 
                        guessDisplay[d] = input;
                    }
                }
            }
        }

        public static void GamePlay()
        {
            while (!win & nbrOfAttempts != 0)
            {
                match.Clear();
                notMatch.Clear();

                GameInput();
                DictionaryCheck();

                if (match.Count < notMatch.Count)
                {
                    CheckFamilies();
                    UpdateGuessProgress();
                    updateStatus = 1;
                    UpdateGameAnswers(updateStatus);
                }
                else
                {
                    Console.WriteLine("Wrong Guess! Try again");
                    updateStatus = 2;
                    UpdateGameAnswers(updateStatus);
                }

                if (!guessDisplay.Contains("_")) { win = true; }

                nbrOfAttempts--;
            }
            GameConclude();
        }

        public static void UpdateGameAnswers(int status)
        {
            gameAnswers.Clear();

            if (status == 1) { gameAnswers = biggestIndexFamily; }
            else if (status == 2) { gameAnswers = notMatch; }
        }

        public static void GameConclude()
        {
            if (win)
            {
                Console.WriteLine();
                Console.WriteLine("YOU WON! Press any key to quit.");
                Console.ReadKey();
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("YOU LOST! Press any key to quit.");
                Console.ReadKey();
                System.Environment.Exit(0);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hangman!");
            WordFamily.Initialise();
        }
    }
}
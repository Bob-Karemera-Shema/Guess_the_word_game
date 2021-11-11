using System;
using System.IO;
using System.Collections.Generic;

namespace Word_Family_Guess_the_word_game_
{
    class WordFamily
    {
        private static Random random = new Random();
        private static int chosenIndex;
        private static string Input;
        private static bool win;
        private static List<string> gameAnswers; //list to hold dictionary words
        private static int wordSize;
        private static List<string> guessDisplay;
        private static List<string> match;
        private static List<string> notMatch;

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

            GameIntro();
        }

        public static void GameIntro()
        {
            //chosenWord = gameAnswers[random.Next(gameAnswers.Count)].ToUpper();
            //int length = wordSize;
            guessDisplay = new List<string>(wordSize);
            for (int i = 0; i < wordSize; i++)
            {
                guessDisplay.Add("_");
            }

            int nbrOfAttempts = wordSize * 2;

            Console.WriteLine("Discover the word in "+ nbrOfAttempts + " attempts by guessing using letters.");
            Console.WriteLine("This word has: " + wordSize + " letters. Good luck!");
            GamePlay();
        }
        
        public static void GamePlay()
        {
            int iteration;
            match = new List<string>();
            notMatch = new List<string>();
            while (!win)
            {
                iteration = 0;
                match.Clear();
                notMatch.Clear();
                Console.Write("Player Guess: ");
                foreach (string letter in guessDisplay)
                {
                    Console.Write(letter);
                }
                Console.WriteLine();
                Console.Write("Enter your guess: ");
                Input = Console.ReadLine().ToUpper();
                Console.WriteLine();

                for(int x=0;x<gameAnswers.Count;x++)
                {
                    if(gameAnswers[x].Contains(Input))
                    {
                        match.Add(gameAnswers[x]);
                    }
                    else
                    {
                        notMatch.Add(gameAnswers[x]);
                    }
                }

                for(int k=0;k<guessDisplay.Count;k++)
                {
                    if(guessDisplay[k]!="_")
                    { iteration++;}
                }

                if(iteration>0) { LetterFill();}

                if(match.Count<notMatch.Count)
                {
                    gameAnswers.Clear();
                    UpdateWordFamily(match);

                    Console.WriteLine("Correct!");
                    char guess = Input[0];
                    chosenIndex = random.Next(wordSize);

                    for (int i = 0; i < guessDisplay.Count; i++)
                    {
                        if (i == chosenIndex & guessDisplay[i] == "_")
                        {
                            guessDisplay[i] = Input;
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
            }

            GameConclude();
        }

        public static void UpdateWordFamily(List<string> newList)
        {
            for(int i=0;i<newList.Count;i++)
            {
                gameAnswers.Add(newList[i]);
            }
        }

        public static void LetterFill()
        {
            string word;
            bool correct = true;
            for(int i=0;i<match.Count;i++)
            {
                word = match[i];
                for (int j = 0; j < guessDisplay.Count; j++)
                {
                    if (guessDisplay[j].Equals(word[j]) & guessDisplay[j] != "_")
                    {
                        correct = true;
                    }
                    else
                    {
                        correct = false;
                        break;
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

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hangman!");
            WordFamily.Initialise();
        }
    }
}
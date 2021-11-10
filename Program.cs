using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Text;

namespace Word_Family_Guess_the_word_game_
{
    class WordFamily
    {
        private static Random random = new Random();
        private static string chosenWord;
        private static string Input;
        private static bool win;
        private static List<string> gameAnswers; //list to hold dictionary words
        private static int wordSize;
        private static List<string> guessDisplay;

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
            List<string> containGuess = new List<string>();
            List<string> notContainGuess = new List<string>();
            while (!win)
            {
                containGuess.Clear();
                notContainGuess.Clear();
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
                    if(gameAnswers[x].Contains(Input) & LetterFill(Input, guessDisplay, gameAnswers[x]))
                    {
                        containGuess.Add(gameAnswers[x]);
                    }
                    else
                    {
                        notContainGuess.Add(gameAnswers[x]);
                    }
                }

                if(containGuess.Count>=notContainGuess.Count)
                {
                    gameAnswers.Clear();
                    UpdateWordFamily(containGuess);

                    Console.WriteLine("Correct!");
                    char guess = Input[0];
                    chosenWord = gameAnswers[random.Next(gameAnswers.Count)].ToUpper();

                    for (int i = 0; i < chosenWord.Length; i++)
                    {
                        if (chosenWord[i].Equals(guess) == true)
                        {
                            guessDisplay[i] = Input;
                        }
                    }

                    if (guessDisplay.Contains("_") == false)
                    {
                        win = true;
                        Console.WriteLine("The chosen word is " + chosenWord);
                    }
                }
                else
                {
                    gameAnswers.Clear();
                    UpdateWordFamily(notContainGuess);
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

        public static bool LetterFill(string guess, List<string> guessProgress, string word)
        {
            bool match = true;
            for(int i=0;i<word.Length;i++)
            {
                if(guessProgress[i].Equals(word[i]) & guessProgress[i] != "_")
                {
                    match = true;
                }
                else
                {
                    match = false;
                    break;
                }
            }

            return match;
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
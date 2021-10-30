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
        private static string thisAnswer;
        private static string Input;
        private static bool win;
        private static List<string> GameAnswers;
        private static int wordSize;
        private static List<string> GuessDisplay;

        public WordFamily()
        {
            GameAnswers = new List<string>(); //list to hold dictionary words
            wordSize = random.Next(4, 12);
            win = false;
        }

        public static void GameIntro()
        {
            thisAnswer = GameAnswers[random.Next(GameAnswers.Count)].ToUpper();
            int length = thisAnswer.Length;
            GuessDisplay = new List<string>(thisAnswer.Length);
            for (int i = 0; i < thisAnswer.Length; i++)
            {
                GuessDisplay.Add("_");
            }

            int nbrOfAttempts = length * 2;

            Console.WriteLine("Discover the word in "+ nbrOfAttempts + " attempts by guessing using letters.");
            Console.WriteLine("This word has: " + length + " letters. Good luck!");
            GamePlay();
        }
        
        public static void GamePlay()
        {
            while(!win)
            {
                Console.Write("Player Guess: ");
                foreach (string letter in GuessDisplay)
                {
                    Console.Write(letter);
                }
                Console.WriteLine();
                Input = Console.ReadLine().ToUpper();

                if (thisAnswer.Contains(Input) == true)
                {
                    Console.WriteLine("Correct!");
                    char guess = Input[0];
                    
                    for (int i = 0; i < thisAnswer.Length; i++)
                    {
                        if (thisAnswer[i].Equals(guess) == true)
                        {
                            GuessDisplay[i] = Input;
                        }
                    }
                    
                    if (GuessDisplay.Contains("_") == false)
                    {
                        win = true;
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect guess! Try again");
                }
            }

            GameConclude();
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
            WordFamily.GameIntro();
        }
    }
}
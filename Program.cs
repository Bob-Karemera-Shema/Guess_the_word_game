﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Word_Family_Guess_the_word_game_
{
    class WordFamily
    {
        private static Random random = new Random();
        private static string input;
        private static bool win;
        private static List<string> gameAnswers = new List<string>(); //list to hold dictionary words
        private static int wordSize = 0;
        private static List<char> guessDisplay;
        private static List<string> contain = new List<string>();
        private static List<string> notContain = new List<string>();
        private static int nbrOfAttempts;
        private static int[] familyArray = new int[7];
        private static int bigFamily = 0;
        private static int maxIndex = 0;
        private static List<string> biggestFamilyCharRep = new List<string>();
        private static List<string> biggestIndexFamily = new List<string>();
        private static List<string> dictionaryMatchProgress = new List<string>();
        private static int updateStatus = 0;

        public static void Initialise()
        {

            wordSize = random.Next(4, 12);
            guessDisplay = new List<char>(wordSize);
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

            for (int i = 0; i < wordSize; i++)
            {
                guessDisplay.Add('_');
            }

            nbrOfAttempts = wordSize * 2;

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
            foreach (char letter in guessDisplay)
            {
                Console.Write(letter);
            }
            Console.WriteLine();
            Console.Write("Enter your guess: ");
            input = Console.ReadLine().ToLower();
            Console.WriteLine();

            if (!((int)input[0] > 96 & (int)input[0] < 123))
            {
                Console.WriteLine("Your input was not a letter of the alphabet!");
                Console.WriteLine("Enter a letter between A-Z");
                GameInput();
            }
        }

        public static void DictionaryMatchProgress()
        {
            int count = 0;
            List<bool> correct = new List<bool>();
            dictionaryMatchProgress = gameAnswers.ToList();

            for (int x = 0; x < guessDisplay.Count; x++)
            {
                if (guessDisplay[x] == '_')
                {
                    count++;
                }
                correct.Add(true); //initialise bool array
            }

            if (count < guessDisplay.Count)
            {
                for (int i = 0; i < dictionaryMatchProgress.Count; i++)
                {
                    for (int j = 0; j < guessDisplay.Count; j++)
                    {
                        for (int x = 0; x < correct.Count; x++)
                        {
                            if (guessDisplay[x] == '_')
                            {
                                correct[x] = true;
                            }
                        }

                        if (guessDisplay[j] != '_')
                        {
                            if (!(dictionaryMatchProgress[i][j].Equals(guessDisplay[j])))
                            {
                                correct[j] = false;
                            }
                        }
                    }

                    if (correct.Contains(false))
                    { dictionaryMatchProgress.RemoveAt(i); }
                }
            }
        }

        public static void DictionaryCheck()
        {
            contain = dictionaryMatchProgress.ToList();
            notContain = dictionaryMatchProgress.ToList();

            for (int x = 0; x < dictionaryMatchProgress.Count; x++)
            {
                if (gameAnswers[x].Contains(input))
                {
                    notContain.Remove(dictionaryMatchProgress[x]);
                }
                else
                {
                    contain.Remove(dictionaryMatchProgress[x]);
                }
            }
        }

        public static void CheckFamilies()
        {
            int count = 0;
            bigFamily = 0;

            for (int x = 0;x < familyArray.Length;x++)
            {
                familyArray[x] = 0;
            }

            for (int i = 0; i < contain.Count; i++)
            {
                count = 0;
                for (int j = 0; j < contain[i].Length; j++)
                {
                    if (contain[i][j].Equals(input[0]))
                    {
                        count++;
                    }
                }
                familyArray[count--]++;  //Update families
            }

            bigFamily = GetMaxCharacters();
            UpdateBigCharFamily(bigFamily);
            CheckIndexFamily();
        }

        public static void CheckIndexFamily()
        {
            maxIndex = 0;
            int[] indexArray = new int[guessDisplay.Count];

            for (int x = 0; x < indexArray.Length; x++)
            {
                indexArray[x] = 0;
            }

            for (int i = 0; i < biggestFamilyCharRep.Count; i++)
            {
                for (int j = 0; j < guessDisplay.Count; j++)
                {
                    if (biggestFamilyCharRep[i][j].Equals(input[0]))
                    {
                        indexArray[j]++;
                    }
                }
            }

            maxIndex = GetMaxIndex(indexArray);
            UpdateBigIndexFamily(maxIndex);
        }

        public static int GetMaxCharacters()
        {
            int max = familyArray[0];
            int index = 0;
            for (int i = 1; i < familyArray.Length; i++)
            {
                if (familyArray[i] > max)
                {
                    max = familyArray[i];
                    index = i;
                }
            }
            return index;
        }

        public static int GetMaxIndex(int[] array)
        {
            int max = array[0];
            int index = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max) 
                { 
                    max = array[i];
                    index = i;
                }
            }
            return index;
        }

        public static void UpdateBigCharFamily(int count)
        {
            biggestFamilyCharRep = contain.ToList();
            int index;

            for (int x = 0;x < biggestFamilyCharRep.Count; x++)
            {
                index = 0;
                for(int i = 0;i < biggestFamilyCharRep[x].Length; i++)
                {
                    if(biggestFamilyCharRep[x][i].Equals(input[0]))
                    { index++; }
                }

                if(!(index == count))
                { biggestFamilyCharRep.RemoveAt(x);}
            }
        }

        public static void UpdateBigIndexFamily(int index)
        {
            biggestIndexFamily = biggestFamilyCharRep.ToList();
            
            for (int i = 0; i < biggestIndexFamily.Count; i++)
            {
                if (!(biggestIndexFamily[i][index].Equals(input[0])))
                {
                    biggestIndexFamily.RemoveAt(i);
                }
            }
        }

        public static void UpdateGuessProgress()
        {
            guessDisplay[maxIndex] = input[0];
        }

        public static void UpdatePossibleAnswers(int status)
        {
            gameAnswers.Clear();

            if (status == 1)
            {
                gameAnswers = biggestIndexFamily.ToList();
            }

            if (status == 2)
            {
                gameAnswers = notContain.ToList();
            }
        }

        public static void GamePlay()
        {
            while (!win & nbrOfAttempts != 0)
            {
                contain.Clear();
                notContain.Clear();

                GameInput();
                DictionaryMatchProgress();
                DictionaryCheck();

                if (contain.Count > notContain.Count)
                {
                    CheckFamilies();
                    UpdateGuessProgress();
                    updateStatus = 1;
                    UpdatePossibleAnswers(updateStatus);
                }
                else
                {
                    Console.WriteLine("Wrong Guess! Try again");
                    updateStatus = 2;
                    UpdatePossibleAnswers(updateStatus);
                }

                if (!guessDisplay.Contains('_')) { win = true; }

                nbrOfAttempts--;
            }
            GameConclude();
        }

        public static void UpdateGameAnswers(int status)
        {
            gameAnswers.Clear();

            if (status == 1) 
            {
                gameAnswers = biggestIndexFamily.ToList(); 
            }
            else if (status == 2) 
            { 
                gameAnswers = notContain.ToList();
            }
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

                for(int i = 0; i < gameAnswers.Count; i++)
                {
                    Console.WriteLine(gameAnswers[i]);
                }

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
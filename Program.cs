using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Word_Family_Guess_the_word_game_
{
    class WordFamily
    {
        private static Random random = new Random();                              // declaring a random object to help randomly generate values
        
        private static string input;                                              // to hold character input by the user
        
        private static bool win;                                                  // bool to determine whether user wins or loses
        
        private static List<string> gameAnswers = new List<string>();             // list to hold dictionary words
        
        private static int wordSize = 0;                                          // the size of the word to be guessed
        
        private static List<char> guessDisplay;                                   // tracks the progress of guesses made by the player
        
        private static List<string> contain = new List<string>();                 // list of words containing the character input by the user
        
        private static List<string> notContain = new List<string>();              // list of words not containing the character input by the user
        
        private static List<string> biggestFamilyCharRep = new List<string>();    // list of words with the highest repetition of the input character
        
        private static List<string> biggestIndexFamily = new List<string>();      // list of words with where the input character is at the index
                                                                                  // with the most possible words
        
        private static List<string> dictionaryMatchProgress = new List<string>(); // list of words which match the characters and indices of
                                                                                  // already guessed words
        
        private static int nbrOfAttempts;                                         // number of attempts the player has left
        
        private static int[] familyArray = new int[7];                            // array contaning integer counters, the counters classify words
                                                                                  // according to the number of times a guessed character appears
                                                                                  // in a certain word
        
        private static int bigFamily = 0;                                         // variable containing an integer mapping to the word family a word
                                                                                  // belongs to depending on the number of times a guessed character
                                                                                  // appears in the word (Easy level)
        
        private static int maxIndex = 0;                                          // variable containing an integer mapping to the index family a word
                                                                                  // belongs to depending on whether a guessed character is found at that
                                                                                  // index in the word.
        
        private static int updateStatus = 0;                                      // variable to help determine what the new pool of words to choose from
                                                                                  // is in the next iteration of the game

        private static string difficulty;                                           // string variable to get user's difficulty of choice

        private static int[] characterRepetitionCoefficient = { 7, 6, 5, 4, 3, 2, 1 }; // array holding coefficients of preference for word families
                                                                                       // according to the number of repetitions of the guessed character the word has

        private static int[] vowelRepetitionCoefficient = { 8, 7, 6, 5, 4, 3, 2, 1 };             // array holding coefficients of preference for word families
                                                                                       // according to the number of vowels a word in the word list has

        private static int containCoefficient = 1;                                  // coefficient of preference for words containing guessed character
        private static int notContainCoefficient = 10;                               // coefficient of preference for words not containing guessed character

        private static int chosenFamily = 0;                                        // variable containing an integer mapping to the word family a word
                                                                                    // belongs to depending on the number of times a guessed character
                                                                                    // appears in the word (Hard level)

        private static int chosenVowelFamily = 0;                                   // variable containing an integer mapping to the vowel family
                                                                                    // a word belongs to depending on the number of vowels in the word

        private static List<string> vowelFamilies = new List<string>();             // list of words selected depending on their vowel content and previous coefficients

        private static int[] vowelArray = new int[8];                               // array contaning integer counters, the counters classify words
                                                                                    // according to the number of vowel in a certain word
        /// <summary>
        /// Fuunction to initialise object attributes at the start of the program
        /// </summary>
        public static void Initialise()
        {
            wordSize = random.Next(4, 12);                                        // randomly get the size of the word to guess
            guessDisplay = new List<char>(wordSize);                              // define guess progress list with the number of characters matching
                                                                                  // the word size
            
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
                            // each line contains a word and each word is added to gameAnswers list
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
            
            win = false;                                // initialise the win boll to false, the player hasn't won the game yet

            for (int i = 0; i < wordSize; i++)          // every index of the word to guess is initialise with dash marks
            {
                guessDisplay.Add('_');
            }

            nbrOfAttempts = wordSize * 2;               // nummber of attempts the user has

            GameIntro();
        }

        /// <summary>
        /// Function to introduce the player to the game and start the game
        /// </summary>
        private static void GameIntro()
        {
            Console.Clear();
            Console.WriteLine("Discover the word in " + nbrOfAttempts + " attempts by guessing using letters.");
            Console.WriteLine("This word has: " + wordSize + " letters. Good luck!");
            Console.WriteLine("Enter a letter between A-Z");
            Console.WriteLine("Choose your difficulty level: Easy(E) or Hard(H) (E/H)");
            difficulty = Console.ReadLine().ToLower();

            if(!(difficulty[0].Equals('h') || difficulty[0].Equals('e')))
            {
                Console.WriteLine("Your input was not letter E or H!");
                Console.WriteLine("Press Enter key to try again!");
                Console.ReadKey();
                GameIntro();
            }

            GamePlay();
        }

        /// <summary>
        /// Function to obtain input character guessed by the player
        /// </summary>
        private static void GameInput()
        {
            // show player number of attepmts left
            Console.WriteLine("Number of Attempts left: " + nbrOfAttempts);

            // first show the user their progress
            Console.Write("Player Guess: ");
            foreach (char letter in guessDisplay)
            {
                Console.Write(letter);
            }
            Console.WriteLine();

            // get player guess
            Console.Write("Enter your guess: ");
            input = Console.ReadLine().ToLower();
            Console.WriteLine();

            // check whether the input is a character and if not ask the player a new guess
            if (!((int)input[0] > 96 && (int)input[0] < 123))
            {
                Console.WriteLine("Your input was not a letter of the alphabet!");
                Console.WriteLine("Enter a letter between A-Z");
                GameInput();   // recursive call to the function
            }
        }

        /// <summary>
        /// Function to remove words which don't match guess progress
        /// words are checked index by index
        /// words are not removed if they contain the guess characters and
        /// at exactly the index they appear at iin the guess progress list.
        /// </summary>
        private static void DictionaryMatchProgress()
        {
            int count = 0;                                        // couter for not guessed characters
            List<bool> correct = new List<bool>();                // bool to check each character of a word matches the one in the guess progress
                                                                  // at a particuler index
            
            dictionaryMatchProgress = gameAnswers.ToList();       // copy word pool to a list which is going to be narrowed to according
                                                                  // to game rules defined further in the code

            for (int x = 0; x < guessDisplay.Count; x++)
            {
                if (guessDisplay[x] == '_')
                {
                    count++;                                      // count number of not guessed characters
                }
                correct.Add(true); //initialise bool array
            }

            // if the count is less than the number of characters of the word to guess, that means some character(s) have been guessed
            if (count < guessDisplay.Count)
            {
                for (int i = 0; i < gameAnswers.Count; i++)
                {
                    for (int j = 0; j < guessDisplay.Count; j++)
                    {
                        for (int x = 0; x < correct.Count; x++)
                        {
                            correct[x] = true;
                        }

                        if (guessDisplay[j] != '_')
                        {
                            // if a character index of a word in the word pool where a character was correctly guessed does not match
                            // the character in the guess progress, it gets flaged and then removed.
                            // this is because the word won't match the guesses made.
                            if (!(gameAnswers[i][j].Equals(guessDisplay[j])))
                            {
                                correct[j] = false;
                            }
                        }
                    }

                    if (correct.Contains(false))
                    { dictionaryMatchProgress.Remove(gameAnswers[i]); }
                }
            }
        }

        /// <summary>
        /// Once the word pool has been narrowed down to fit the guess progress, the word pool is filtered to two groups.
        /// Group 1: is a list of words containing the character guessed by the player.
        /// Group 2: is a list of words not containing the character guessed by the player.
        /// This function serves the purpose to separate these two groups.
        /// </summary>
        private static void DictionaryCheck()
        {
            //copy the word pool to both groups and remove word according to what group they fit in
            contain = dictionaryMatchProgress.ToList();
            notContain = dictionaryMatchProgress.ToList();

            for (int x = 0; x < dictionaryMatchProgress.Count; x++)
            {
                if (dictionaryMatchProgress[x].Contains(input))
                {
                    notContain.Remove(dictionaryMatchProgress[x]);
                }
                else
                {
                    contain.Remove(dictionaryMatchProgress[x]);
                }
            }
        }

        // START OF EASY IMPLEMENTATION

        /// <summary>
        /// check words in thw word pool and classify words in word families according 
        /// to the number of times the guessed character appears in each word in the word pool
        /// </summary>
        private static void CheckFamilies()
        {
            int count = 0;
            bigFamily = 0;                                     // initialise the family of words with the most number of words
                                                               // depending on the number of occurences of the guessed character
                                                               // in those words

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

            bigFamily = GetMaxCharacters();                   // set the new big family
            UpdateBigCharFamily(bigFamily);
            CheckIndexFamily();
        }

        /// <summary>
        /// Function to identify the largest group of word containing the guessed character at a particular index
        /// </summary>
        private static void CheckIndexFamily()
        {
            maxIndex = 0;
            int[] indexArray = new int[guessDisplay.Count];           // array containing counters at every index of every word character

            // initialise every element of the array
            for (int x = 0; x < indexArray.Length; x++)
            {
                indexArray[x] = 0;
            }

            for (int i = 0; i < biggestFamilyCharRep.Count; i++)
            {
                for (int j = 0; j < guessDisplay.Count; j++)
                {
                    if (biggestFamilyCharRep[i][j].Equals(input[0]) & guessDisplay[j].Equals('_'))
                    {
                        indexArray[j]++;
                    }
                }
            }

            maxIndex = GetMaxIndex(indexArray);                      // get the index with more matching words 
            UpdateBigIndexFamily(maxIndex);
        }

        /// <summary>
        /// Determines which family has more words depending on the number of guessed character occurences
        /// </summary>
        /// <integer></returns>
        /// Returns an integer index mapping to the index of the largest word family in the families array
        private static int GetMaxCharacters()
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

        /// <summary>
        /// Determines which family has more words depending on the index of the guessed character
        /// </summary>
        /// <integer></returns>
        /// Returns an integer index mapping an the index in the guess progress list
        private static int GetMaxIndex(int[] array)
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

        /// <summary>
        /// Function to update word pool to only contain words in the largest word family depending on 
        /// the guess character occurence
        /// </summary>
        /// <param name="count"></param>
        private static void UpdateBigCharFamily(int count)
        {
            biggestFamilyCharRep = contain.ToList();
            int occurence;

            for (int x = 0;x < contain.Count; x++)
            {
                occurence = 0;
                for(int i = 0;i < contain[x].Length; i++)
                {
                    if(contain[x][i].Equals(input[0]))
                    { occurence++; }
                }

                if(!(occurence == count))
                { biggestFamilyCharRep.Remove(contain[x]);}
            }
        }

        /// <summary>
        /// Function to update the word to only contain words with the guess character at the index with
        /// with more words
        /// </summary>
        /// <param name="index"></param>
        private static void UpdateBigIndexFamily(int index)
        {
            biggestIndexFamily = biggestFamilyCharRep.ToList();
            
            for (int i = 0; i < biggestFamilyCharRep.Count; i++)
            {
                if (!(biggestFamilyCharRep[i][index].Equals(input[0])))
                {
                    biggestIndexFamily.Remove(biggestFamilyCharRep[i]);
                }
            }
        }

        /// <summary>
        /// Function to update the guess progress with the correct guess at the max index
        /// </summary>
        private static void UpdateGuessProgress()
        {
            guessDisplay[maxIndex] = input[0];
        }

        /// <summary>
        /// Function to update the word pool to the new filtered list
        /// </summary>
        /// <param name="status"></param>
        private static void UpdatePossibleAnswers(int status)
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

        // END OF EASY IMPLEMENTATION

        // START OF HARD IMPLEMENTATION

        /// <summary>
        /// Divides words in word families according to number of times the guessed character appears in the word
        /// </summary>
        private static void DivideWordFamilies()
        {
            int count = 0;

            for (int x = 0; x < familyArray.Length; x++)
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
        }

        /// <summary>
        /// Takes every family and multiplies its number of elements and coefficient of preference
        /// The highest coefficient is kept and is the new family chosen
        /// </summary>
        /// <returns></returns>
        private static void FamilyCoefficients()
        {
            int[] largestCoefficient = new int[familyArray.Length];

            // initialise
            for (int i = 0; i < familyArray.Length; i++)
            {
                largestCoefficient[i] = 0;
            }

            for (int i = 0; i < familyArray.Length; i++)
            {
                largestCoefficient[i] = characterRepetitionCoefficient[i] * familyArray[i] * containCoefficient;
            }

            int max = largestCoefficient[0];
            for (int i = 1; i < largestCoefficient.Length; i++)
            {
                if (largestCoefficient[i] > max)
                {
                    max = largestCoefficient[i];
                    chosenFamily = i;
                }
            }
            containCoefficient = max;
        }

        /// <summary>
        /// Further divide word families according to the number of vowels in the word
        /// </summary>
        private static void VowelCheck()
        {
            int count = 0;
            vowelFamilies = biggestFamilyCharRep.ToList();
            char[] vowels = { 'a', 'e', 'i', 'o', 'u'};

            for (int x = 0; x < familyArray.Length; x++)
            {
                vowelArray[x] = 0;
            }

            for (int i = 0;i < biggestFamilyCharRep.Count;i++)
            {
                count = 0;
                for(int j = 0;j < biggestFamilyCharRep[i].Length; j++)
                {
                    if(vowels.Contains(biggestFamilyCharRep[i][j]))
                    {
                        count++;
                    }
                }
                vowelArray[count--]++;
            }
        }

        /// <summary>
        /// Takes every vowel family and multiplies its number of elements and coefficient of preference
        /// The highest coefficient is kept and is the new family chosen
        /// </summary>
        private static void VowelCoefficients()
        {
            int[] largestCoefficient = new int[vowelArray.Length];

            // initialise
            for (int i = 0; i < familyArray.Length; i++)
            {
                largestCoefficient[i] = 0;
            }

            for (int i = 0; i < familyArray.Length; i++)
            {
                largestCoefficient[i] = vowelRepetitionCoefficient[i] * vowelArray[i] * containCoefficient;
            }

            int max = largestCoefficient[0];
            for (int i = 1; i < largestCoefficient.Length; i++)
            {
                if (largestCoefficient[i] > max)
                {
                    max = largestCoefficient[i];
                    chosenVowelFamily = i;
                }
            }
            containCoefficient = max;
        }

        /// <summary>
        /// Take the vowel family with the highest coeffiecient as the new word list
        /// to be further analysed
        /// </summary>
        private static void GetVowelFamily()
        {
            int count = 0;
            char[] vowels = { 'a', 'e', 'i', 'o', 'u' };

            for (int i = 0; i < biggestFamilyCharRep.Count; i++)
            {
                count = 0;
                for (int j = 0; j < biggestFamilyCharRep[i].Length; j++)
                {
                    if (vowels.Contains(biggestFamilyCharRep[i][j]))
                    {
                        count++;
                    }
                }
                if(!(vowelArray[chosenVowelFamily]==count))
                {
                    vowelFamilies.Remove(biggestFamilyCharRep[i]);
                }
            }
        }

        /// <summary>
        /// Function to identify the largest group of word containing the guessed character at a particular index
        /// </summary>
        private static void CheckIndexFamily(bool hardLevelCall)
        {
            if (hardLevelCall)
            {
                maxIndex = 0;
                int[] indexArray = new int[guessDisplay.Count];           // array containing counters at every index of every word character

                // initialise every element of the array
                for (int x = 0; x < indexArray.Length; x++)
                {
                    indexArray[x] = 0;
                }

                for (int i = 0; i < vowelFamilies.Count; i++)
                {
                    for (int j = 0; j < guessDisplay.Count; j++)
                    {
                        if (vowelFamilies[i][j].Equals(input[0]) & guessDisplay[j].Equals('_'))
                        {
                            indexArray[j]++;
                        }
                    }
                }

                maxIndex = GetMaxIndex(indexArray);                      // get the index with more matching words 
                UpdateBigIndexFamily(maxIndex);
            }
        }

        /// <summary>
        /// Function to control the game flow
        /// </summary>
        private static void GamePlay()
        {
            while (!win & nbrOfAttempts != 0)
            {
                contain.Clear();
                notContain.Clear();

                if (difficulty[0].Equals('e'))
                {
                    // Difficulty level is easy
                    GameInput();
                    DictionaryMatchProgress();
                    DictionaryCheck();

                    if (contain.Count > notContain.Count)
                    {
                        Console.WriteLine("Correct guess! Keep going");
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
                        nbrOfAttempts--;
                    }

                    if (!guessDisplay.Contains('_')) { win = true; }
                }

                if(difficulty[0].Equals('h'))
                {
                    containCoefficient = 1;
                    notContainCoefficient = 10;

                    // Difficulty level is hard
                    GameInput();
                    DictionaryMatchProgress();
                    DictionaryCheck();

                    containCoefficient *= contain.Count;
                    notContainCoefficient *= notContain.Count;

                    DivideWordFamilies();
                    FamilyCoefficients();
                    UpdateBigCharFamily(chosenFamily);
                    VowelCheck();

                    if(containCoefficient > notContainCoefficient)
                    {
                        Console.WriteLine("Correct guess! Keep going");
                        CheckIndexFamily(true);
                        UpdateGuessProgress();
                        updateStatus = 1;
                        UpdatePossibleAnswers(updateStatus);
                    }
                    else
                    {
                        Console.WriteLine("Wrong Guess! Try again");
                        updateStatus = 2;
                        UpdatePossibleAnswers(updateStatus);
                        nbrOfAttempts--;
                    }

                    if (!guessDisplay.Contains('_')) { win = true; }
                }
            }
            GameConclude();
        }

        /// <summary>
        /// Function to conclude the game
        /// Informs user if they have won or not and why
        /// </summary>
        private static void GameConclude()
        {
            if (win)
            {
                Console.WriteLine();
                Console.WriteLine("YOU WON! Press any key to quit.");
                Console.Write("Word of the game was: ");

                for (int i = 0; i < guessDisplay.Count; i++)
                {
                    Console.Write(guessDisplay[i]);
                }
                Console.WriteLine();
            }
            if (nbrOfAttempts == 0)
            {
                Console.WriteLine();
                Console.WriteLine("You have " + nbrOfAttempts + " attempts left.");
                Console.WriteLine("YOU LOST! Press any key to quit.");
                ShowRandomWord();
            }

            Console.ReadKey();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// Randomly choose word of the game from word pool
        /// </summary>
        private static void ShowRandomWord()
        {
            if (gameAnswers.Count > 1)
            {
                Console.WriteLine("Word of the game was: " + gameAnswers[random.Next(gameAnswers.Count)]);
            }
            else
            {
                Console.WriteLine("Word of the game was: " + gameAnswers[0]);
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
using System.Diagnostics;

const string EasyDifficulty = "EASY";
const string MediumDifficulty = "MEDIUM";
const string HardDifficulty = "HARD";
const string RandomOperator = "RANDOM";

const int EasyMultiplier = 1;
const int MediumMultiplier = 2;
const int HardMultiplier = 3;

const string ReturnPreviousScreen = "RETURN";
const string AdditionOperator = "+";
const string SubtractOperator = "-";
const string MultiplicationOperator = "*";
const string DivisionOperator = "/";

int PlayerLives = 3;
int PlayerPoints = 0;
int[] LeaderBoard = new int[5];
Stopwatch gameTimer = new Stopwatch();

StartApplicationScreen();

void StartApplicationScreen()
{
    //Title Screen
    Console.WriteLine("WELCOME TO THE MATH GAME!");

    Console.WriteLine("1: Play");
    Console.WriteLine("2: Rules");
    Console.WriteLine("3: Leaderboard");
    Console.WriteLine("4: Exit\n");

    Console.Write("Enter a number to continue:");
    string playerMenuInput = Console.ReadLine() ?? string.Empty;
    Console.Clear();

    switch (playerMenuInput)
    {
        case "1":
            ChooseDifficultyScreen();
            break;
        case "2":
            HowToPlayScreen();
            break;
        case "3":
            LeaderBoardScreen();
            break;
        case "4":
            ExitGame();
            break;
        default:
            Console.Clear();
            Console.WriteLine("CHOOSE A VALID INPUT (1-4)");
            StartApplicationScreen();
            break;
    }
}

//PLAY
void ChooseDifficultyScreen()
{
    Console.WriteLine($"Enter \"{ReturnPreviousScreen}\" to return to menu");
    Console.WriteLine("Choose your difficulty!\n");
    Console.WriteLine("1: " + EasyDifficulty);
    Console.WriteLine("2: " + MediumDifficulty);
    Console.WriteLine("3: " + HardDifficulty);

    Console.Write("Enter here:");
    int difficulty = 1;
    string playerDifficultyInput = Console.ReadLine() ?? string.Empty;
    Console.Clear();

    switch (playerDifficultyInput.ToUpper())
    {
        case "1":
            difficulty = EasyMultiplier;
            ChooseOperatorsScreen(difficulty);
            break;
        case "2":
            difficulty = MediumMultiplier;
            ChooseOperatorsScreen(difficulty);
            break;
        case "3":
            difficulty = HardMultiplier;
            ChooseOperatorsScreen(difficulty);
            break;
        case ReturnPreviousScreen:
            StartApplicationScreen();
            break;
        default:
            Console.WriteLine("CHOOSE A VALID INPUT (1-3)!\n");
            ChooseDifficultyScreen();
            break;
    }
}

void ChooseOperatorsScreen(int difficulty)
{
    Console.WriteLine($"Enter \"{ReturnPreviousScreen}\" to return to menu");
    Console.WriteLine("Choose your operator!\n");

    Console.WriteLine("1: " + AdditionOperator);
    Console.WriteLine("2: " + SubtractOperator);
    Console.WriteLine("3: " + MultiplicationOperator);
    Console.WriteLine("4: " + DivisionOperator);
    Console.WriteLine("5: " + RandomOperator);

    string playerOperatorInput = Console.ReadLine() ?? string.Empty;
    gameTimer.Start();
    Console.Clear();

    switch (playerOperatorInput.ToUpper())
    {
        case "1":
            PlayGameScreen(difficulty, AdditionOperator);
            break;
        case "2":
            PlayGameScreen(difficulty, SubtractOperator);
            break;
        case "3":
            PlayGameScreen(difficulty, MultiplicationOperator);
            break;
        case "4":
            PlayGameScreen(difficulty, DivisionOperator);
            break;
        case "5":
            PlayGameScreen(difficulty, RandomOperator);
            break;
        case ReturnPreviousScreen:
            StartApplicationScreen();
            break;
        default:
            Console.WriteLine("Enter a valid input (1-5)");
            ChooseOperatorsScreen(difficulty);
            break;
    }


}

void PlayGameScreen(int difficulty, string choosenOperator)
{
    string question = string.Empty;
    int playerAnswer = 0;
    int correctAnswer = 0;

    while (PlayerLives > 0)
    {
        Console.WriteLine("Score: " + PlayerPoints);
        Console.WriteLine("Lives: " + new string('X', PlayerLives));

        question = GenerateQuestion(difficulty, choosenOperator);
        Console.WriteLine("What is: " + question + " = ?");

        Console.WriteLine($"\nEnter \"{ReturnPreviousScreen}\" to end your run!");
        Console.Write("Enter here:");
        string playerAnswerInput = Console.ReadLine() ?? string.Empty;

        if (playerAnswerInput.ToUpper() == ReturnPreviousScreen)
        {
            Console.Clear();
            DisplayScoreScreen();
            break;
        }

        if (!Int32.TryParse(playerAnswerInput, out playerAnswer))
        {
            PlayerLives--;
            Console.Clear();
            continue;
        }

        correctAnswer = CheckAnswer(question);

        if (correctAnswer == playerAnswer)
        {
            //scale points with difficulty
            //difficulty is the score multiplies for EASY,MEDIUM,HARD difficulties
            PlayerPoints += 100 * difficulty;
        }
        else
        {
            PlayerLives--;
        }

        Console.WriteLine("Correct answer was: " + correctAnswer);
        Console.WriteLine("Press Enter to Continue");
        Console.ReadLine();

        Console.Clear();
    }

    gameTimer.Stop();
    DisplayScoreScreen();
}

//CREATING EQUATION
string GenerateQuestion(int difficulty, string optionalOperator = RandomOperator)
{
    int numberOfOperands = difficulty;
    string question = string.Empty;

    Random random = new Random();
    question += random.Next(-100, 100).ToString();

    if (optionalOperator == RandomOperator)
    {
        for (int i = 0; i < numberOfOperands; i++)
        {
            //pick a random operand
            question += " " + GenerateOperator(random.Next(4));
            question += " " + random.Next(-100, 100).ToString();
        }
    }
    else
    {
        for (int i = 0; i < numberOfOperands; i++)
        {
            question += " " + optionalOperator;
            question += " " + random.Next(-100, 100).ToString();
        }
    }

    return question;
}

//SOLVING EQUATION
int CheckAnswer(string question)
{
    string[] solveQuestion = question.Split(' ');
    string currentOperator = string.Empty;
    int currentValue = 0;
    int questionSize = solveQuestion.Length;
    int operatorIndex = 0;
    int total = 0;

    while (questionSize > 1)
    {
        if (solveQuestion.Contains(MultiplicationOperator))
        {
            operatorIndex = Array.IndexOf(solveQuestion, MultiplicationOperator);
            currentOperator = MultiplicationOperator;
        }
        else if (solveQuestion.Contains(DivisionOperator))
        {
            operatorIndex = Array.IndexOf(solveQuestion, DivisionOperator);
            currentOperator = DivisionOperator;
        }
        else if (solveQuestion.Contains(AdditionOperator))
        {
            operatorIndex = Array.IndexOf(solveQuestion, AdditionOperator);
            currentOperator = AdditionOperator;
        }
        else if (solveQuestion.Contains(SubtractOperator))
        {
            operatorIndex = Array.IndexOf(solveQuestion, SubtractOperator);
            currentOperator = SubtractOperator;
        }

        currentValue = Calculate(solveQuestion, currentOperator, operatorIndex);

        solveQuestion[operatorIndex] = currentValue.ToString();
        solveQuestion[operatorIndex - 1] = string.Empty;
        solveQuestion[operatorIndex + 1] = string.Empty;

        Int32.TryParse(solveQuestion[operatorIndex], out total);
        questionSize -= 2;
    }

    return total;
}

int Calculate(string[] solveQuestion, string operand, int operatorIndex)
{
    //find the first value to the left and right of operand
    //calculate
    int leftValue = 0;
    int rightValue = 0;

    //search left side of operator for next value
    for (int i = operatorIndex; i >= 0; i--)
    {
        if (solveQuestion[i] != null && Int32.TryParse(solveQuestion[i], out leftValue))
        {
            break;
        }
    }
    //search right side of operator for next value
    for (int i = operatorIndex; i <= solveQuestion.Length; i++)
    {
        if (solveQuestion[i] != null)
        {
            if (solveQuestion[i] != null && Int32.TryParse(solveQuestion[i], out rightValue))
            {
                break;
            }
        }
    }

    switch (operand)
    {
        case AdditionOperator:
            return leftValue + rightValue;
        case SubtractOperator:
            return leftValue - rightValue;
        case MultiplicationOperator:
            return leftValue * rightValue;
        case DivisionOperator:
            return leftValue / rightValue;
    }

    return 0;
}

string GenerateOperator(int random)
{
    switch (random)
    {
        case 1:
            return AdditionOperator;
        case 2:
            return SubtractOperator;
        case 3:
            return DivisionOperator;
        case 4:
            return MultiplicationOperator;
        default:
            return AdditionOperator;
    }
}

void DisplayScoreScreen()
{
    Console.WriteLine("SCORE: " + PlayerPoints);
    Console.WriteLine("Elasped Time: " + gameTimer.Elapsed.ToString(@"hh\:mm\:ss"));

    Console.WriteLine("TOP SCORE: " + LeaderBoard[0]);
    Console.WriteLine("Press Enter to Return to Main Menu");
    Console.ReadLine();
    Console.Clear();

    //Enter score into leaderboard if it is in top 5
    //Reset player variables
    EnterScoreToLeaderboard();
    PlayerLives = 3;
    PlayerPoints = 0;
    gameTimer.Reset();

    StartApplicationScreen();
}

void EnterScoreToLeaderboard()
{
    //set to past array indices in case algorithm does not find a placement
    int scorePlacement = LeaderBoard.Length;
    int previousScore = PlayerPoints;
    int nextScore = 0;

    for (int i = 0; i < LeaderBoard.Length; i++)
    {
        if (LeaderBoard[i] <= PlayerPoints)
        {
            scorePlacement = i;
            break;
        }
    }

    //move all placements at the new score down one 
    for (int i = scorePlacement; i < LeaderBoard.Length; ++i)
    {
        nextScore = LeaderBoard[i];
        LeaderBoard[i] = previousScore;
        previousScore = nextScore;
    }
}

//RULES
void HowToPlayScreen()
{
    Console.WriteLine("HOW TO PLAY:");
    Console.WriteLine("STEP 1: In main menu type \'1\' (Play)");
    Console.WriteLine($"STEP 2: Choose your difficulty ({EasyDifficulty}, {MediumDifficulty}, {HardDifficulty})");
    Console.WriteLine($"\tEasy - {EasyMultiplier} Operator Equations");
    Console.WriteLine($"\tMedium - {MediumMultiplier} Operator Equations");
    Console.WriteLine($"\tHard - {HardMultiplier} Operator Equations");
    Console.WriteLine("STEP 3: Choose random or specific operators");
    Console.WriteLine("STEP 4: Solve math problems, get points! (You get three lives)");

    Console.WriteLine($"\nPress enter to go back to main menu");
    Console.Write("Enter here:");

    Console.ReadLine();
    Console.Clear();

    StartApplicationScreen();
}

//LEADERBOARD
void LeaderBoardScreen()
{
    int count = 1;

    Console.WriteLine("TOP SCORES".PadLeft(10));
    foreach (int score in LeaderBoard)
    {
        Console.WriteLine($"{count}: " + score);
        count++;
    }

    Console.WriteLine($"\nPress enter to return to menu");
    Console.ReadLine();

    Console.Clear();
    StartApplicationScreen();


}

//EXIT
void ExitGame()
{
    Environment.Exit(0);
}
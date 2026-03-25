using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizGame
{
    // ==================== PLAYER CLASS ====================
    public class Player
    {
        private string _playerName;
        private int _totalScore;
        private int _livesRemaining;
        private int _currentLevel;

        public string PlayerName
        {
            get { return _playerName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length >= 2)
                    _playerName = value;
                else
                    _playerName = "Player";
            }
        }

        public int TotalScore
        {
            get { return _totalScore; }
            private set { _totalScore = value; }
        }

        public int LivesRemaining
        {
            get { return _livesRemaining; }
            private set { _livesRemaining = value; }
        }

        public int CurrentLevel
        {
            get { return _currentLevel; }
            set { _currentLevel = value; }
        }

        public bool IsAlive
        {
            get { return _livesRemaining > 0; }
        }

        public Player(string playerName)
        {
            PlayerName = playerName;
            _totalScore = 0;
            _livesRemaining = 3;
            _currentLevel = 1;
        }

        public void UpdateScore(int points)
        {
            if (points > 0)
            {
                _totalScore += points;
                Console.WriteLine($"🎉 +{points} points! Total score: {_totalScore}");
            }
        }

        public void LoseLife()
        {
            if (_livesRemaining > 0)
            {
                _livesRemaining--;
                Console.WriteLine($"💔 Wrong answer! Lives remaining: {_livesRemaining}");
            }
        }

        public void ResetPlayer()
        {
            _totalScore = 0;
            _livesRemaining = 3;
            _currentLevel = 1;
            Console.WriteLine($"🔄 {PlayerName} has been reset for a new game!");
        }

        public bool HasLivesLeft()
        {
            return IsAlive;
        }

        public void DisplayPlayerStatus()
        {
            Console.WriteLine($"\n=== Player Status ===");
            Console.WriteLine($"Name: {PlayerName}");
            Console.WriteLine($"Score: {TotalScore}");
            Console.WriteLine($"Lives: {LivesRemaining}");
            Console.WriteLine($"Level: {(CurrentLevel == 1 ? "Beginner" : "Advanced")}");
        }
    }

    // ==================== QUESTION CLASS ====================
    public class Question
    {
        private string _questionText;
        private List<string> _optionsList;
        private string _correctAnswer;
        private int _pointsValue;

        public string QuestionText
        {
            get { return _questionText; }
            set { _questionText = value; }
        }

        public List<string> OptionsList
        {
            get { return _optionsList; }
            set { _optionsList = value; }
        }

        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set { _correctAnswer = value; }
        }

        public int PointsValue
        {
            get { return _pointsValue; }
            set { _pointsValue = value; }
        }

        public Question(string questionText, List<string> optionsList, string correctAnswer, int pointsValue)
        {
            _questionText = questionText;
            _optionsList = optionsList;
            _correctAnswer = correctAnswer;
            _pointsValue = pointsValue;
        }

        public void DisplayQuestion()
        {
            Console.WriteLine($"\n📝 {_questionText}");
            Console.WriteLine("Options:");
            for (int i = 0; i < _optionsList.Count; i++)
            {
                Console.WriteLine($"   {i + 1}. {_optionsList[i]}");
            }
        }

        public bool CheckAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
                return false;

            string trimmedAnswer = userAnswer.Trim();
            
            if (int.TryParse(trimmedAnswer, out int answerNumber))
            {
                if (answerNumber >= 1 && answerNumber <= _optionsList.Count)
                {
                    string selectedOption = _optionsList[answerNumber - 1];
                    return string.Equals(selectedOption, _correctAnswer, StringComparison.OrdinalIgnoreCase);
                }
            }
            
            return string.Equals(trimmedAnswer, _correctAnswer, StringComparison.OrdinalIgnoreCase);
        }

        public string GetCorrectAnswer()
        {
            return _correctAnswer;
        }
    }

    // ==================== LEVEL CLASS ====================
    public class Level
    {
        private int _levelNumber;
        private string _levelName;
        private List<Question> _questionsList;
        private int _passingScore;

        public int LevelNumber
        {
            get { return _levelNumber; }
            set { _levelNumber = value; }
        }

        public string LevelName
        {
            get { return _levelName; }
            set { _levelName = value; }
        }

        public List<Question> QuestionsList
        {
            get { return _questionsList; }
            set { _questionsList = value; }
        }

        public int PassingScore
        {
            get { return _passingScore; }
            set { _passingScore = value; }
        }

        public int TotalQuestions
        {
            get { return _questionsList.Count; }
        }

        public Level(int levelNumber, string levelName, int passingScore)
        {
            _levelNumber = levelNumber;
            _levelName = levelName;
            _passingScore = passingScore;
            _questionsList = new List<Question>();
        }

        public void LoadQuestions(List<Question> questions)
        {
            _questionsList.Clear();
            foreach (var question in questions)
            {
                _questionsList.Add(question);
            }
            Console.WriteLine($"✅ Loaded {_questionsList.Count} questions for {_levelName} level");
        }

        public bool IsLevelCompleted(int playerScore)
        {
            bool completed = playerScore >= _passingScore;
            if (completed)
            {
                Console.WriteLine($"🎯 Level {_levelNumber} completed! Score: {playerScore}/{_passingScore} needed");
            }
            else
            {
                Console.WriteLine($"❌ Level {_levelNumber} not completed. Need {_passingScore} points, got {playerScore}");
            }
            return completed;
        }

        public int GetNextLevel()
        {
            return _levelNumber + 1;
        }

        public void DisplayLevelInfo()
        {
            Console.WriteLine($"\n=== Level {_levelNumber}: {_levelName} ===");
            Console.WriteLine($"Total Questions: {TotalQuestions}");
            Console.WriteLine($"Passing Score: {_passingScore} points");
        }
    }

    // ==================== SCOREBOARD CLASS ====================
    public class ScoreBoard
    {
        private Dictionary<string, int> _playerScores;
        private int _highestScore;

        public Dictionary<string, int> PlayerScores
        {
            get { return _playerScores; }
            set { _playerScores = value; }
        }

        public int HighestScore
        {
            get { return _highestScore; }
            set { _highestScore = value; }
        }

        public ScoreBoard()
        {
            _playerScores = new Dictionary<string, int>();
            _highestScore = 0;
        }

        public void UpdateScore(string playerName, int score)
        {
            if (_playerScores.ContainsKey(playerName))
            {
                _playerScores[playerName] = score;
            }
            else
            {
                _playerScores.Add(playerName, score);
            }

            if (score > _highestScore)
            {
                _highestScore = score;
                Console.WriteLine($"🏆 NEW HIGH SCORE! {playerName}: {score} points!");
            }
        }

        public void DisplayScoreBoard()
        {
            Console.WriteLine("\n🏆 === SCOREBOARD === 🏆");
            
            if (_playerScores.Count == 0)
            {
                Console.WriteLine("No scores recorded yet.");
                return;
            }

            var sortedScores = _playerScores.OrderByDescending(x => x.Value);
            
            Console.WriteLine("Rank\tPlayer Name\t\tScore");
            Console.WriteLine("----------------------------------------");
            
            int rank = 1;
            foreach (var entry in sortedScores)
            {
                Console.WriteLine($"{rank}\t{entry.Key,-20}\t{entry.Value}");
                rank++;
            }
            
            Console.WriteLine($"\n🏆 Highest Score: {_highestScore} points");
        }

        public int GetPlayerScore(string playerName)
        {
            if (_playerScores.ContainsKey(playerName))
                return _playerScores[playerName];
            return 0;
        }
    }

    // ==================== GAME ENGINE CLASS ====================
    public class GameEngine
    {
        private Player _currentPlayer;
        private Level _currentLevel;
        private string _gameStatus;
        private int _scoreToWin;
        private ScoreBoard _scoreBoard;

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set { _currentPlayer = value; }
        }

        public Level CurrentLevel
        {
            get { return _currentLevel; }
            set { _currentLevel = value; }
        }

        public string GameStatus
        {
            get { return _gameStatus; }
            set { _gameStatus = value; }
        }

        public ScoreBoard ScoreBoard
        {
            get { return _scoreBoard; }
            set { _scoreBoard = value; }
        }

        public GameEngine(Player player, int scoreToWin)
        {
            _currentPlayer = player;
            _scoreToWin = scoreToWin;
            _gameStatus = "Active";
            _scoreBoard = new ScoreBoard();
        }

        public void StartGame()
        {
            Console.WriteLine($"🎮 === QUIZ GAME STARTING ===");
            Console.WriteLine($"Welcome, {_currentPlayer.PlayerName}!");
            Console.WriteLine($"Goal: Reach {_scoreToWin} points to win!");
            Console.WriteLine($"You have {_currentPlayer.LivesRemaining} lives. Answer carefully!\n");
            
            _currentPlayer.ResetPlayer();
            _gameStatus = "Active";
            _scoreBoard.UpdateScore(_currentPlayer.PlayerName, 0);
        }

        public bool ProcessAnswer(Question question, string userAnswer)
        {
            if (_gameStatus != "Active")
            {
                Console.WriteLine("Game is no longer active.");
                return false;
            }

            if (!_currentPlayer.HasLivesLeft())
            {
                Console.WriteLine("No lives remaining! Game over.");
                return false;
            }

            bool isCorrect = question.CheckAnswer(userAnswer);
            
            if (isCorrect)
            {
                _currentPlayer.UpdateScore(question.PointsValue);
                _scoreBoard.UpdateScore(_currentPlayer.PlayerName, _currentPlayer.TotalScore);
                
                if (_currentPlayer.TotalScore >= _scoreToWin)
                {
                    _gameStatus = "Won";
                    Console.WriteLine($"\n🎉 CONGRATULATIONS! You've won! 🎉");
                }
                return true;
            }
            else
            {
                _currentPlayer.LoseLife();
                Console.WriteLine($"Correct answer was: {question.GetCorrectAnswer()}");
                return false;
            }
        }

        public void EndGame()
        {
            if (_gameStatus == "Won")
            {
                Console.WriteLine($"\n🎉✨ {_currentPlayer.PlayerName} has won the game! ✨🎉");
                Console.WriteLine($"🏆 Final Score: {_currentPlayer.TotalScore} points! 🏆");
            }
            else
            {
                Console.WriteLine($"\n💀 GAME OVER! 💀");
                Console.WriteLine($"😔 {_currentPlayer.PlayerName} has lost the game.");
                Console.WriteLine($"📊 Final Score: {_currentPlayer.TotalScore} points");
            }
            
            _scoreBoard.UpdateScore(_currentPlayer.PlayerName, _currentPlayer.TotalScore);
            _scoreBoard.DisplayScoreBoard();
        }

        public bool SwitchToNextLevel(Level nextLevel)
        {
            if (nextLevel == null)
                return false;

            _currentLevel = nextLevel;
            _currentPlayer.CurrentLevel = nextLevel.LevelNumber;
            Console.WriteLine($"\n🚀 Moving to {nextLevel.LevelName} Level!");
            return true;
        }

        public void LoadLevelQuestions(Level level, List<Question> questions)
        {
            level.LoadQuestions(questions);
            _currentLevel = level;
        }

        public void DisplayGameStatus()
        {
            Console.WriteLine($"\n=== GAME STATUS ===");
            Console.WriteLine($"Status: {_gameStatus}");
            Console.WriteLine($"Current Score: {_currentPlayer.TotalScore}/{_scoreToWin}");
            Console.WriteLine($"Lives: {_currentPlayer.LivesRemaining}");
            if (_currentLevel != null)
            {
                Console.WriteLine($"Current Level: {_currentLevel.LevelName}");
            }
        }
    }

    // ==================== MAIN PROGRAM ====================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║     WELCOME TO THE QUIZ GAME!         ║");
            Console.WriteLine("║     Test Your Knowledge!              ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            // Get player name
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(playerName))
                playerName = "Player";
            
            // Create Player object
            Player player = new Player(playerName);
            player.DisplayPlayerStatus();
            
            // Create GameEngine
            GameEngine gameEngine = new GameEngine(player, 50);
            ScoreBoard mainScoreBoard = gameEngine.ScoreBoard;
            
            // Create Beginner questions
            List<Question> beginnerQuestions = new List<Question>();
            beginnerQuestions.Add(new Question("What is the capital of Malaysia?", 
                new List<string> {"Kuala Lumpur", "Singapore", "Bangkok", "Jakarta"}, 
                "Kuala Lumpur", 10));
            beginnerQuestions.Add(new Question("Which planet is known as the Red Planet?", 
                new List<string> {"Venus", "Mars", "Jupiter", "Saturn"}, 
                "Mars", 10));
            beginnerQuestions.Add(new Question("What is the largest ocean on Earth?", 
                new List<string> {"Atlantic Ocean", "Indian Ocean", "Arctic Ocean", "Pacific Ocean"}, 
                "Pacific Ocean", 10));
            beginnerQuestions.Add(new Question("Who painted the Mona Lisa?", 
                new List<string> {"Vincent van Gogh", "Pablo Picasso", "Leonardo da Vinci", "Michelangelo"}, 
                "Leonardo da Vinci", 10));
            beginnerQuestions.Add(new Question("What is the chemical symbol for gold?", 
                new List<string> {"Go", "Gd", "Au", "Ag"}, 
                "Au", 10));
            
            // Create Level 1
            Level beginnerLevel = new Level(1, "Beginner", 25);
            gameEngine.LoadLevelQuestions(beginnerLevel, beginnerQuestions);
            
            // Start the game
            gameEngine.StartGame();
            
            // Play Beginner Level
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║         BEGINNER LEVEL                ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            
            for (int i = 0; i < beginnerQuestions.Count && player.HasLivesLeft(); i++)
            {
                Question currentQuestion = beginnerQuestions[i];
                currentQuestion.DisplayQuestion();
                Console.Write("\nYour answer (enter number or text): ");
                string answer = Console.ReadLine();
                
                gameEngine.ProcessAnswer(currentQuestion, answer);
                gameEngine.DisplayGameStatus();
            }
            
            // Check if player can advance
            if (player.HasLivesLeft() && beginnerLevel.IsLevelCompleted(player.TotalScore))
            {
                Console.WriteLine("\n🎉 CONGRATULATIONS! You've unlocked the Advanced Level! 🎉");
                
                // Create Advanced questions
                List<Question> advancedQuestions = new List<Question>();
                advancedQuestions.Add(new Question("What is the powerhouse of the cell?", 
                    new List<string> {"Nucleus", "Mitochondria", "Ribosome", "Endoplasmic Reticulum"}, 
                    "Mitochondria", 15));
                advancedQuestions.Add(new Question("Who wrote 'Romeo and Juliet'?", 
                    new List<string> {"Charles Dickens", "Mark Twain", "William Shakespeare", "Jane Austen"}, 
                    "William Shakespeare", 15));
                advancedQuestions.Add(new Question("What is the square root of 144?", 
                    new List<string> {"10", "11", "12", "13"}, 
                    "12", 15));
                advancedQuestions.Add(new Question("Which country won the FIFA World Cup in 2018?", 
                    new List<string> {"Germany", "Brazil", "France", "Croatia"}, 
                    "France", 15));
                advancedQuestions.Add(new Question("What does CPU stand for?", 
                    new List<string> {"Central Processing Unit", "Computer Personal Unit", "Central Program Utility", "Core Processing Unit"}, 
                    "Central Processing Unit", 15));
                
                // Create Level 2
                Level advancedLevel = new Level(2, "Advanced", 40);
                gameEngine.SwitchToNextLevel(advancedLevel);
                gameEngine.LoadLevelQuestions(advancedLevel, advancedQuestions);
                
                // Play Advanced Level
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║         ADVANCED LEVEL                ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                
                for (int i = 0; i < advancedQuestions.Count && player.HasLivesLeft(); i++)
                {
                    Question currentQuestion = advancedQuestions[i];
                    currentQuestion.DisplayQuestion();
                    Console.Write("\nYour answer (enter number or text): ");
                    string answer = Console.ReadLine();
                    
                    gameEngine.ProcessAnswer(currentQuestion, answer);
                    gameEngine.DisplayGameStatus();
                }
            }
            else if (!player.HasLivesLeft())
            {
                Console.WriteLine("\n😢 You lost all your lives. Better luck next time! 😢");
            }
            else
            {
                Console.WriteLine($"\n📊 You scored {player.TotalScore} points. Need {beginnerLevel.PassingScore} points to unlock Advanced Level.");
            }
            
            // End the game
            gameEngine.EndGame();
            
            // Show final player status
            player.DisplayPlayerStatus();
            
            // Show encapsulation explanation
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║      ENCAPSULATION DEMONSTRATION      ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("\n✓ Private fields protect data from direct access");
            Console.WriteLine("✓ Properties control how data is accessed");
            Console.WriteLine("✓ Methods provide controlled functionality");
            Console.WriteLine("✓ Data validation ensures data integrity");
            
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║    Thanks for playing!                ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.ReadKey();
        }
    }
}


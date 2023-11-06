using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MinesweeperGame
{
    static class ScoreManager
    {
        private static List<ScoreEntry> _scores = new List<ScoreEntry>();
        private const string ScoreFileName = "scores.csv";

        static ScoreManager()
        {
            LoadScores();
        }

        public static void SaveScore(string playerName, int score, string difficulty, string result)
        {
            _scores.Add(new ScoreEntry(playerName, score, difficulty, result));
            SaveScoresToFile();
        }

        public static void ViewScores()
        {
            foreach (var entry in _scores)
            {
                Console.WriteLine($"{entry.PlayerName}: {entry.Result} | {entry.Score}s ({entry.Difficulty})");
            }
        }

        private static void LoadScores()
        {
            if (File.Exists(ScoreFileName))
            {
                var lines = File.ReadAllLines(ScoreFileName);
                _scores = lines.Skip(1).Select(line =>
                {
                    var parts = line.Split(',');
                    return new ScoreEntry(parts[0], int.Parse(parts[1]), parts[2], parts[3]); ;
                }).ToList();
            }
        }

        private static void SaveScoresToFile()
        {
            var lines = new List<string> { "PlayerName,Score,Difficulty,Result" };
            lines.AddRange(_scores.Select(score => $"{score.PlayerName},{score.Score},{score.Difficulty},{score.Result}"));
            File.WriteAllLines(ScoreFileName, lines);
        }
    }

    class ScoreEntry
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public string Difficulty { get; set; }
        public string Result { get; set; }


        public ScoreEntry(string playerName, int score, string difficulty, string result)
        {
            PlayerName = playerName;
            Score = score;
            Difficulty = difficulty;
            Result = result;
        }
    }
}

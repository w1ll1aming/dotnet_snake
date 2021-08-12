namespace dotnet_snake 
{
    public struct GameStats
    {
        public GameStats(bool gameover, int score)
        {
            GameOver = gameover;
            Score = score;
        }
        
        public int Score { get; set; }
        public bool GameOver { get; set; }
    }
}
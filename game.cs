namespace dotnet_snake {
    public struct GameStats {
        public GameStats(int score, bool gameover, bool paused) {
            this.Score = score;
            this.GameOver = gameover;
            this.GamePaused = paused;
        }
        
        public int Score { get; set; }
        public bool GameOver { get; set; }
        public bool GamePaused { get; set; }
    }
}
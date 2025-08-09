namespace SpaceX.Models
{
    public class GameOptions
    {
        public int Level { get; set; }
        public int Coins { get; set; }
        public Difficulty SelectedDifficulty { get; set; }
        public State GameState { get; set; }
        public enum State
        {
            MainMenu,
            CurrentGame,
            GameLost
        }
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }
        public GameOptions()
        {
            this.Level = 1;
            this.Coins = 0;
            this.GameState = State.CurrentGame;
        }
        public void GoToMainMenu()
        {
            this.Level = 1;
            this.Coins = 0;
            this.GameState = State.MainMenu;
        }
    }
}

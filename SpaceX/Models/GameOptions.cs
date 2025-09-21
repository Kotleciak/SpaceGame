namespace SpaceX.Models
{
    public class GameOptions
    {
        public int Level { get; set; }
        public int Coins { get; set; }
        public bool IsTutorial { get; set; }
        public int TutorialStep { get; set; }
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
            this.Level = 0;
            this.Coins = 0;
            this.GameState = State.CurrentGame;
            this.IsTutorial = true;
            this.TutorialStep = 0;
        }
        public void GoToMainMenu()
        {
            this.GameState = State.MainMenu;
        }
        public void StartNewGame()
        {
            this.Level = 0;
            this.Coins = 0;
            this.GameState = State.CurrentGame;
            this.IsTutorial = true;
            this.TutorialStep = 0;
        }
        public List<string> NextTuroialStep(string action)
        {
            string[] _TutorialSteps = new string[]
            {
                "MoveTutorial",
                "ShootTutorial",
                "OpenInventoryTutorial",
                "CloseInventoryTutorial"
            };
            List<string> result = new List<string>();
            if (action == "moved" && this.TutorialStep == 0)
            {
                this.TutorialStep++;
                result.Add(_TutorialSteps[this.TutorialStep - 1]);
                result.Add(_TutorialSteps[this.TutorialStep]);
            }
            else if(action == "clicked" && this.TutorialStep == 1)
            {
                this.TutorialStep++;
                result.Add(_TutorialSteps[this.TutorialStep - 1]);
                result.Add(_TutorialSteps[this.TutorialStep]);
            }
            else if (action == "opened" && this.TutorialStep == 2)
            {
                this.TutorialStep++;
                result.Add(_TutorialSteps[this.TutorialStep - 1]);
                result.Add(_TutorialSteps[this.TutorialStep]);
            }
            else if (action == "opened" && this.TutorialStep == 3)
            {
                this.TutorialStep++;
                result.Add(_TutorialSteps[this.TutorialStep - 1]);
                this.IsTutorial = false;
            }
            return result;
        }
    }
}

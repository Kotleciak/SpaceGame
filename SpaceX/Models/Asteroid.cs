namespace SpaceX.Models
{
    public class Asteroid
    {
        public int Health { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int YCenterPosition { get; set; }
        public int XCenterPosition { get; set; }
        public int Speed { get; set; }
        public AsteroidSize Size { get; set; }
        public int AsteroidHeigth { get; set; }
        public int AsteroidWidth { get; set; }
        public DateTime TimeToShowUp { get; set; }
        public bool IsReleased { get; set; }
        public int NumberOfTimesBeingHit { get; set; }
        public enum AsteroidSize
        {
            Small,
            Medium,
            Large
        }
        public Asteroid()
        {
            Random random = new Random();
            this.TimeToShowUp = DateTime.Now.AddSeconds(random.Next(1, 5));
            this.IsReleased = false;
            this.YPosition = -80;
        }
        public void MoveDown()
        {
            this.YPosition += 10;
            InitializeAsteroidCenterPosition();
        }
        public void InitializeAsteroidCenterPosition()
        {
            this.XCenterPosition = this.XPosition + this.AsteroidWidth / 2;
            this.YCenterPosition = this.YPosition + this.AsteroidHeigth /2;
        }
        public void GetRandomXPosition(int width)
        {
            Random random = new Random();
            this.XPosition = random.Next(width - this.AsteroidWidth);
            Console.WriteLine(this.XPosition);
        }
        public void InitializeAsteroidSize()
        {
            switch (this.Size)
            {
                case AsteroidSize.Small:
                    this.AsteroidHeigth = 20;
                    this.AsteroidWidth = 20;
                    this.Health = 30;
                    break;
                case AsteroidSize.Medium:
                    this.AsteroidHeigth = 40;
                    this.AsteroidWidth = 40;
                    this.Health = 150;
                    break;
                case AsteroidSize.Large:
                    this.AsteroidHeigth = 60;
                    this.AsteroidWidth = 60;
                    this.Health = 300;
                    break;
            }
        }
        public bool IsAlive(int height, int width)
        {
            return this.XPosition >= -30 && this.XPosition <= width + 30
                    && this.YPosition <= height + 30 && this.Health >= 0;
        }
        public int AsteroidHit(int damage)
        {
            this.Health = this.Health - damage;
            if(this.Health > 30 && this.Health <= 150)
            {
                this.Size = AsteroidSize.Medium;
                this.AsteroidHeigth = 40;
                this.AsteroidWidth = 40;
                Console.WriteLine("extra coins " + 10);
                return 10;
            }
            else if(this.Health <= 30 && this.Health > 0)
            {
                this.Size = AsteroidSize.Small;
                this.AsteroidHeigth = 20;
                this.AsteroidWidth = 20;
                Console.WriteLine("extra coins " + 5);
                return 5;
            }
            return 0;
        }
        public bool CheckIfReleased()
        {
            if(DateTime.Now >= this.TimeToShowUp)
            {
                this.IsReleased = true;
                Console.WriteLine("Released!");
            }
            return this.IsReleased;
        }
    }
}

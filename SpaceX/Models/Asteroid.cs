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
        public double SquaredRadius { get; set; }
        public enum AsteroidSize
        {
            Small,
            Medium,
            Large
        }
        public Asteroid(int Level)
        {
            Random random = new Random();
            this.TimeToShowUp = DateTime.Now.AddSeconds(random.Next(1, 5));
            this.IsReleased = false;
            this.YPosition = -80;
            this.Speed = Level / 2 + 1;

            //getting random asteroid sizes:
            int small = 100 - 2 * Level;
            if(small < 0)
            {
                small = 0;
            }
            int medium = 2 * Level;
            if(medium > 100)
            {
                medium = 0;
            }
            int large = 2 * Level;
            if(large > 100)
            {
                large = 100;
            }
            int rnd = random.Next(1, 101);
            if(rnd < small)
            {
                this.Size = Asteroid.AsteroidSize.Small;
            }
            else if(rnd < small + medium)
            {
                this.Size = Asteroid.AsteroidSize.Medium;
            }
            else
            {
                this.Size = Asteroid.AsteroidSize.Large;
            }
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
        }
        public void InitializeAsteroidSize()
        {
            switch (this.Size)
            {
                case AsteroidSize.Small:
                    this.AsteroidHeigth = 20;
                    this.AsteroidWidth = 20;
                    this.Health = 30;
                    this.SquaredRadius = 625;
                    break;
                case AsteroidSize.Medium:
                    this.AsteroidHeigth = 40;
                    this.AsteroidWidth = 40;
                    this.Health = 150;
                    this.SquaredRadius = 2500;
                    break;
                case AsteroidSize.Large:
                    this.AsteroidHeigth = 60;
                    this.AsteroidWidth = 60;
                    this.Health = 300;
                    this.SquaredRadius = 5625;
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
                this.SquaredRadius = 2500;
                return 10;
            }
            else if(this.Health <= 30 && this.Health > 0)
            {
                this.Size = AsteroidSize.Small;
                this.AsteroidHeigth = 20;
                this.AsteroidWidth = 20;
                this.SquaredRadius = 625;
                return 5;
            }
            return 0;
        }
        public bool CheckIfReleased()
        {
            if(DateTime.Now >= this.TimeToShowUp)
            {
                this.IsReleased = true;
            }
            return this.IsReleased;
        }
    }
}

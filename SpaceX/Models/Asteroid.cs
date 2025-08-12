namespace SpaceX.Models
{
    public class Asteroid
    {
        public int Health { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Speed { get; set; }
        public AsteroidSize Size { get; set; }
        public enum AsteroidSize
        {
            Small,
            Medium,
            Large
        } 
        public void MoveDown()
        {
            this.YPosition += 10;
        }
        public bool IsAlive(int height, int width)
        {
            return XPosition >= -30 && XPosition <= width + 30
                && YPosition >= -30 && YPosition <= height + 30;
        }
    }
}

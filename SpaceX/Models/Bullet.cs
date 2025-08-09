namespace SpaceX.Models
{
    public class Bullet
    {
        public int ID { get; set; }
        public int YPosition { get; set; }
        public int XPosition { get; set; }
        public int StartYPosition { get; set; }
        public int StartXPosition { get; set; }
        public void MoveUp()
        {
            this.YPosition -= 10;
            this.StartYPosition -= 10;
        }
    }
}

namespace SpaceX.Models
{
    public class Ship
    {
        public int ID { get; set; }
        public int YPosition { get; set; }
        public int XPosition { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int ShipWidth { get; set; }
        public int ShipHeight { get; set; }
        public void MoveUp(int distance)
        {
            YPosition -= distance;
        }
        public void MoveDown(int distance)
        {
            YPosition += distance;
        }
        public void MoveLeft(int distance)
        {
            XPosition -= distance;
        }
        public void MoveRight(int distance)
        {
            XPosition += distance;
        }
    }
}

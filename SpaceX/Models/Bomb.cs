namespace SpaceX.Models
{
    public class Bomb : Bullet
    {
        public Bomb(int UserYPosition)
        {
            this.MaxYPosition = UserYPosition + 10;
        }
        new public void Move()
        {
            this.YPosition += this.Speed;
            this.StartYPosition += this.Speed;
        }
    }
}

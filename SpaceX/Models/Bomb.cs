namespace SpaceX.Models
{
    public class Bomb : Bullet
    {
        public Bomb(int UserYPosition, EnemyShip enemy)
        {
            this.MaxYPosition = UserYPosition + 10;
            this.XPosition = enemy.XPosition + (enemy.ShipWidth / 2);
            this.YPosition = enemy.YPosition + 20;
            this.StartYPosition = enemy.YPosition + enemy.ShipHeight;
        }
        new public void Move()
        {
            this.YPosition += this.Speed;
            this.StartYPosition += this.Speed;
        }
    }
}

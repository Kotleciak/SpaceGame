namespace SpaceX.Models
{
    public class EnemyShip : Ship
    {
        public int UserXDestionationPosition { get; set; }
        public DateTime DateSinceLastAttack { get; set; }
        public EnemyShip()
        {
            this.XPosition = 1000;
            this.YPosition = -40;
            this.DateSinceLastAttack = DateTime.Now;
        }
        public new void SetMaxPositions(int maxX, int maxY)
        {
            this.MaxXPosition = maxX - 10;
            this.MaxYPosition = maxY;
        }
        public void AvoidUser(int UserXPosition)
        {
            if (this.XCenterPosition == UserXPosition)
            {
                this.MoveRight();
            }
            else if (this.XCenterPosition == UserXPosition && this.MaxXPosition - this.XCenterPosition < 30)
            {
                this.MoveLeft();
            }
            else if (this.XCenterPosition > UserXPosition)
            {
                this.MoveRight();
            }
            else
            {
                this.MoveLeft();
            }
        }
        public void Rain()
        {
            Console.WriteLine("It's here!");
        }
        public void TimeToAttackEnemyShip()
        {
            if((DateTime.Now - this.DateSinceLastAttack).TotalSeconds < 5)
            {
                Console.WriteLine("Attack");
            }
            else
            {
                this.DateSinceLastAttack = DateTime.Now;
                Console.WriteLine("Date reset");
            }
        }
    }
}

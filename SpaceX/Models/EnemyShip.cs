namespace SpaceX.Models
{
    public class EnemyShip : Ship
    {
        public int XDestionationPosition { get; set; }
        public DateTime DateSinceLastAttack { get; set; }
        public bool IsAttacking { get; set; }
        public int ThirdCountAttack { get; set; }
        public EnemyShip()
        {
            this.XPosition = 1000;
            this.YPosition = -40;
            this.DateSinceLastAttack = DateTime.Now;
            this.IsAttacking = false;
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
        public void TimeToAttackEnemyShip()
        {
            this.InitializeShipCenterPosition();
            if (this.XDestionationPosition > this.XCenterPosition)
            {
                this.MoveRight();
                if(this.XDestionationPosition <= this.XCenterPosition)
                {
                    this.IsAttacking = false;
                    this.DateSinceLastAttack = DateTime.Now;
                }
            }
            else if(this.XDestionationPosition < this.XCenterPosition)
            {
                this.MoveLeft();
                if(this.XDestionationPosition >= this.XCenterPosition)
                {
                    this.IsAttacking = false;
                    this.DateSinceLastAttack = DateTime.Now;
                }
            }
        }
        public void AttackOrAvoid(int UserXPosition)
        {
            if(this.IsAttacking)
            {
                this.TimeToAttackEnemyShip();
            }
            else if((DateTime.Now - this.DateSinceLastAttack).TotalSeconds < 5)
            {
                this.AvoidUser(UserXPosition);
            }
            else
            {
                this.IsAttacking = true;
                if (UserXPosition > this.XCenterPosition)
                {
                    if(UserXPosition + 50 >= this.MaxXPosition)
                    {
                        this.XDestionationPosition = UserXPosition - 15;
                    }
                    else
                    {
                        this.XDestionationPosition = UserXPosition + 20;
                    }
                }
                else
                {
                    if (UserXPosition - 60 <= 0)
                    {
                        this.XDestionationPosition = UserXPosition + 15;
                    }
                    else
                    {
                        this.XDestionationPosition = UserXPosition - 20;
                    }
                }
                this.TimeToAttackEnemyShip();
            }
        }
    }
}

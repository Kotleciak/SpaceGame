namespace SpaceX.Models
{
    public class EnemyShip : Ship
    {
        public int XDestionationPosition { get; set; }
        public int YDestinationPosition { get; set; }
        public DateTime DateSinceLastAttack { get; set; }
        public bool IsAttacking { get; set; }
        public int CountAttack { get; set; }
        public EnemyShipClass Class { get; set; }
        public enum EnemyShipClass
        {
            Basic,
            Tank,
            Boss //I don't know for sure if I will implement it. I have good concept for it, but I don't know if I will have time to do it.
        }
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
            if(this.YPosition < 35)
            {
                this.MoveUp();
            }
        }
        private void BasicAttack()
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
            this.CountAttack++;
        }
        private void TankAttack()
        {
            this.InitializeShipCenterPosition();
            if (this.XDestionationPosition > this.XCenterPosition)
            {
                this.MoveRight();
                if (this.XDestionationPosition <= this.XCenterPosition)
                {
                    this.IsAttacking = false;
                    this.DateSinceLastAttack = DateTime.Now;
                }
            }
            else if (this.XDestionationPosition < this.XCenterPosition)
            {
                this.MoveLeft();
                if (this.XDestionationPosition >= this.XCenterPosition)
                {
                    this.IsAttacking = false;
                    this.DateSinceLastAttack = DateTime.Now;
                }
            }
            if(this.YDestinationPosition > this.YCenterPosition)
            {
                this.MoveDown();
                if(this.YDestinationPosition <= this.YCenterPosition)
                {
                    this.IsAttacking = false;
                }
            }
        }
        private void Attack()
        {
            switch(this.Class)
            {
                case EnemyShipClass.Basic:
                    this.BasicAttack();
                    break;
                case EnemyShipClass.Tank:
                    this.TankAttack();
                    break;
                case EnemyShipClass.Boss:

                    break;
            }
        }
        public void AttackOrAvoid(int UserXPosition)
        {
            if(this.IsAttacking)
            {
                this.BasicAttack();
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
                if(this.Class == EnemyShipClass.Tank)
                {
                    this.YDestinationPosition = this.MaxYPosition - 100;
                }
                this.Attack();
            }
        }
        public bool ShouldAttack()
        {
            Console.WriteLine("This ship ypositon is: " + this.YPosition);
            Console.WriteLine("Center y is: " + this.YCenterPosition);
            if(this.CountAttack > 3)
            {
                this.CountAttack = 0;
            }
            if (this.IsAttacking)
            {
                switch (this.Class)
                {
                    case EnemyShipClass.Basic:
                        return this.CountAttack == 3;
                    case EnemyShipClass.Tank:
                        return this.CountAttack < 1;
                    case EnemyShipClass.Boss:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}

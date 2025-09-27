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
        public int AttackedCount { get; set; }
        public DateTime AttackTime { get; set; }
        public enum EnemyShipClass
        {
            Basic,
            Tank,
            Boss //I don't know for sure if I will implement it. I have good concept for it, but I don't know if I will have time to do it.
        }
        public EnemyShip(bool isItTimeForTank, int Level)
        {
            this.XPosition = 1000;
            this.YPosition = -40;
            this.DateSinceLastAttack = DateTime.Now;
            this.IsAttacking = false;
            if (isItTimeForTank)
            {
                this.Class = EnemyShip.EnemyShipClass.Tank;
                this.Health = 200 + Level * 11;
            }
            else
            {
                this.Class = EnemyShip.EnemyShipClass.Basic;
                this.Health = 100 + Level * 9;
            }
        }
        public new void SetMaxPositions(int maxX, int maxY)
        {
            this.MaxXPosition = maxX - 10;
            this.MaxYPosition = maxY;
        }
        public void AvoidUser(int UserXPosition, int UserYPosition)
        {
            if (this.XCenterPosition == UserXPosition)
            {
                this.MoveRight();
            }                                                       
            else if (this.MaxXPosition - this.XCenterPosition < 30)
            {
                this.MoveLeft();
            }                                                               //this.XCenterPosition == UserXPosition && 
            else if (this.XCenterPosition > UserXPosition)
            {
                this.MoveRight();
            }
            else
            {
                this.MoveLeft();
            }
            if(this.YPosition > 35)
            {
                this.MoveUp();
            }
        }
        public void BasicAttack()
        {
            this.InitializeShipCenterPosition();
            if(this.XDestionationPosition >= this.MaxXPosition)
            {
                this.XDestionationPosition = this.MaxXPosition - 20;
            }
            if(this.XDestionationPosition <= 0)
            {
                this.XDestionationPosition = 20;
            }
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
            this.CountAttack++;
        }
        public void TankAttack()
        {
            this.InitializeShipCenterPosition();
            if(this.AttackedCount >= 3)
            {
                this.AttackedCount = 0;
                this.IsAttacking = false;
                this.DateSinceLastAttack = DateTime.Now;
                return;
            }
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
        public void AttackOrAvoid(int UserXPosition, int UserYPosition)
        {
            if (this.IsAttacking && (DateTime.Now - this.AttackTime).TotalSeconds > 11)
            {
                this.IsAttacking = false;
                this.DateSinceLastAttack = DateTime.Now;
                return;
            }
            if (this.IsAttacking)
            {
                this.Attack();
            }
            else if((DateTime.Now - this.DateSinceLastAttack).TotalSeconds < 5)
            {
                this.AvoidUser(UserXPosition, UserYPosition);
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
                    this.YDestinationPosition = UserYPosition - 300;
                }
                this.AttackTime = DateTime.Now;
                this.Attack();
            }
        }
        public bool ShouldAttack()
        {
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
                        if(Math.Abs(this.XCenterPosition - this.XDestionationPosition) < 30)
                        {
                            return true;
                        }
                        break;
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

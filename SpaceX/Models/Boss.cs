using System.Globalization;
using System.Security.AccessControl;

namespace SpaceX.Models
{
    public class Boss : EnemyShip
    {
        public static int AttackPhase = 0;
        public static int ForWhichBossIsItTimeToDropBombs = 0;
        public static int ForWhichBossIsItTimeToAvoidUser = 1;
        int timeForWhichShip = 1;
        public static int numberOfBossesAtRound;
        int ShiftXPosition = 40;
        DateTime LastAttack = DateTime.Now;
        public Boss(bool isItTimeForTank, int Level) : base(isItTimeForTank, Level)
        {
            numberOfBossesAtRound = 2;
            AttackPhase = 0;
            this.DateSinceLastAttack = DateTime.Now;
            this.CountAttack = 0;
            this.Health = 1000 + Level * 10;
        }
        ~Boss()
        {
            numberOfBossesAtRound--;
        }
        public void IsItTimeForRainAttack(int numberOfBosses, int XDesPos)
        {
            if((DateTime.Now - LastAttack).TotalSeconds > 5 && AttackPhase == 0)
            {
                LastAttack = DateTime.Now;
                if(numberOfBosses == 1)
                {
                    AttackPhase = 2;
                    this.XDestionationPosition = XDesPos;
                }
                else
                {
                    AttackPhase = 1;
                }
            }
        }
        public string FindCorrectPosition(int Ship1XPosition, int Ship2XPosition)
        {
            string hasToMove = "none";
            int distance = Math.Abs(Ship1XPosition - Ship2XPosition);
            if (Ship1XPosition > Ship2XPosition)
            {
                hasToMove = "Ship2";
                if(distance < 50)
                {
                    hasToMove = "None";
                }
            }
            else
            {
                hasToMove = "Ship1";
                if (distance < 50)
                {
                    hasToMove = "None";
                }
            }
            return hasToMove;
        }
        public bool MoveToDestinationPosition(Boss secondBoss)
        {
            if(this.XDestionationPosition > this.XCenterPosition)
            {
                this.MoveRight(secondBoss);
            }
            else
            {
                this.MoveLeft(secondBoss);
            }
            if (Math.Abs(this.XDestionationPosition - this.XCenterPosition) < 40)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GoBackToStartYPosition(Boss secondBoss)
        {
            bool yDone = true;
            bool xDone = true;
            if (Math.Abs(this.YDestinationPosition - this.YPosition) > 10)
            {
                this.MoveUp(secondBoss);
                yDone = false;
            }
            else
            {
                this.MoveDown(secondBoss);
            }
            if (Math.Abs(this.XDestionationPosition - this.XCenterPosition) > 10)
            {
                if (this.XDestionationPosition > this.XCenterPosition)
                    this.MoveRight(secondBoss);
                else
                    this.MoveLeft(secondBoss);
                xDone = false;
            }
            return yDone && xDone;
        }
        public bool CanMove(Boss secondBoss, string direction)
        {
            if (secondBoss == null)
                return true;

            // Przewidujemy nową pozycję po ruchu
            int nextX = this.XPosition;
            int nextY = this.YPosition;
            
            switch (direction)
            {
                case "right":
                    nextX += this.Speed;
                    break;
                case "left":
                    nextX -= this.Speed;
                    break;
                case "up":
                    nextY -= this.Speed;
                    break;
                case "down":
                    nextY += this.Speed;
                    break;
            }

            // Sprawdzamy kolizję prostokątów (AABB)
            bool overlapX = nextX < secondBoss.XPosition + secondBoss.ShipWidth &&
                            nextX + this.ShipWidth > secondBoss.XPosition;
            bool overlapY = nextY < secondBoss.YPosition + secondBoss.ShipHeight &&
                            nextY + this.ShipHeight > secondBoss.YPosition;

            // Jeśli już się stykają, pozwól na ruch, jeśli się oddalają
            bool currentlyOverlappingX = this.XPosition < secondBoss.XPosition + secondBoss.ShipWidth &&
                                        this.XPosition + this.ShipWidth > secondBoss.XPosition;
            bool currentlyOverlappingY = this.YPosition < secondBoss.YPosition + secondBoss.ShipHeight &&
                                        this.YPosition + this.ShipHeight > secondBoss.YPosition;

            if (overlapX && overlapY)
            {
                // Pozwól na ruch, jeśli dystans się zwiększa
                int currentDist = Math.Abs(this.XCenterPosition - secondBoss.XCenterPosition) + Math.Abs(this.YCenterPosition - secondBoss.YCenterPosition);
                int nextDist = Math.Abs((nextX + this.ShipWidth / 2) - (secondBoss.XCenterPosition)) + Math.Abs((nextY + this.ShipHeight / 2) - (secondBoss.YCenterPosition));
                if (nextDist > currentDist)
                    return true; // pozwól się rozdzielić
                return false;
            }

            return true;
        }
        public new void MoveUp(Boss secondBoss)
        {
            if(this.CanMove(secondBoss, "up"))
            {
                if (this.YPosition - this.Speed > 0)
                {
                    this.YPosition -= this.Speed;
                    this.InitializeShipCenterPosition();
                }
                else
                {
                    this.YPosition = 0;
                    this.InitializeShipCenterPosition();
                }
            }
        }
        public new void MoveDown(Boss secondBoss)
        {
            if(this.CanMove(secondBoss, "down"))
            {
                if (this.YPosition + this.Speed + this.ShipHeight < this.MaxYPosition)
                {
                    this.YPosition += this.Speed;
                    this.InitializeShipCenterPosition();
                }
                else
                {
                    this.YPosition = this.MaxYPosition - this.ShipHeight;
                    this.InitializeShipCenterPosition();
                }
            }
        }
        public new void MoveLeft(Boss secondBoss)
        {
            if(this.CanMove(secondBoss, "left"))
            {
                if (this.XPosition - this.Speed > 0)
                {
                    this.XPosition -= this.Speed;
                    this.InitializeShipCenterPosition();
                }
                else
                {
                    this.XPosition = 0;
                    this.InitializeShipCenterPosition();
                }
            }
        }
        public new void MoveRight(Boss secondBoss)
        {
            if(this.CanMove(secondBoss, "right"))
            {
                if (this.XPosition + this.Speed + this.ShipWidth < this.MaxXPosition)
                {
                    this.XPosition += this.Speed;
                    this.InitializeShipCenterPosition();
                }
                else
                {
                    this.XPosition = this.MaxXPosition - this.ShipWidth;
                    this.InitializeShipCenterPosition();
                }
            }
        }
        public new void AvoidUser(int UserXPosition, int UserYPosition, Boss secondBoss)
        {
            if (this.XCenterPosition == UserXPosition)
            {
                this.MoveRight(secondBoss);
            }
            else if (this.XCenterPosition == UserXPosition && this.MaxXPosition - this.XCenterPosition < 30)
            {
                this.MoveLeft(secondBoss);
            }
            else if (this.XCenterPosition > UserXPosition)
            {
                this.MoveRight(secondBoss);
            }
            else
            {
                this.MoveLeft(secondBoss);
            }
            if (this.YPosition > 35)
            {
                this.MoveUp(secondBoss);
            }
        }
    } 
}

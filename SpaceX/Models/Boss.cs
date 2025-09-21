using System.Security.AccessControl;

namespace SpaceX.Models
{
    public class Boss : EnemyShip
    {
        public static int AttackPhase = 0;
        public static int ForWhichBossIsItTimeToDropBombs = 0;
        public static int ForWhichBossIsItTimeToAvoidUser = 1;
        int timeForWhichShip = 1;
        int numberOfBossesAtRound;
        int ShiftXPosition = 40;
        DateTime LastAttack = DateTime.Now;
        public Boss(bool isItTimeForTank) : base(isItTimeForTank)
        {
            numberOfBossesAtRound = 2;
            AttackPhase = 0;
            this.DateSinceLastAttack = DateTime.Now;
            this.CountAttack = 0;
        }
        ~Boss()
        {
            numberOfBossesAtRound--;
        }
        public void IsItTimeForRainAttack(int numberOfBosses)
        {
            if((DateTime.Now - LastAttack).TotalSeconds > 10 && AttackPhase == 0)
            {
                LastAttack = DateTime.Now;
                if(numberOfBosses == 1)
                {
                    AttackPhase = 2;
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
        public bool MoveToDestinationPosition()
        {
            if(this.XDestionationPosition > this.XCenterPosition)
            {
                this.MoveRight();
            }
            else
            {
                this.MoveLeft();
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
        public bool GoBackToStartYPosition()
        {
            bool yDone = true;
            bool xDone = true;
            Console.WriteLine("YDestination is: " + this.YDestinationPosition + " and the yposition is: " + this.YPosition + " and maxY is: " + this.MaxYPosition);
            if (Math.Abs(this.YDestinationPosition - this.YPosition) > 0)
            {
                Console.WriteLine("Is it even invoking?");
                this.MoveUp();
                yDone = false;
            }
            if (Math.Abs(this.XDestionationPosition - this.XCenterPosition) > 10)
            {
                if (this.XDestionationPosition > this.XCenterPosition)
                    this.MoveRight();
                else
                    this.MoveLeft();
                xDone = false;
            }
            return yDone && xDone;
        }
    } 
}

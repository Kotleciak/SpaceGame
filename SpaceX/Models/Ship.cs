using System.Net.Http.Headers;

namespace SpaceX.Models
{
    public class Ship
    {
        public int ID { get; set; }
        public int YPosition { get; set; }
        public int XPosition { get; set; }
        public int MaxHealth { get; set; } //max 6 levels - 100, 300, 400, 600, 800, 1000
        public int Health { get; set; }
        public int Speed { get; set; } // max 5 levels - 15, 20, 25, 30, 35
        public int LevelOfBulletsDmg { get; set; } // max 5
        public int LevelOfBulletsSpeed { get; set; } // max 5
        public int ShipWidth { get; set; }
        public int ShipHeight { get; set; }
        public int YCenterPosition { get; set; }
        public int XCenterPosition { get; set; }
        public int MaxXPosition { get; set; }
        public int MaxYPosition { get; set; }
        public Ship()
        {
            this.XPosition = 10;
            this.YPosition = 10;
            this.Health = 100;
            this.ShipWidth = 100;
            this.ShipHeight = 100;
            this.Speed = 15;
        }
        public void ResetShip()
        {
            this.XPosition = 10;
            this.YPosition = 10;
            this.Health = 100;
            this.ShipWidth = 100;
            this.ShipHeight = 100;
            this.Speed = 15;
        }
        public void InitializeShipCenterPosition()
        {
            this.XCenterPosition = this.XPosition + (this.ShipWidth / 2);
            this.YCenterPosition = this.YPosition + (this.ShipHeight / 2);
        }
        public void MoveUp()
        {
            if (this.YPosition - this.Speed > 0)
            {
                this.YPosition -= this.Speed;
                this.InitializeShipCenterPosition();
            }
        }
        public void MoveDown()
        {
            if(this.YPosition + this.Speed + this.ShipHeight < this.MaxYPosition)
            {
                this.YPosition += this.Speed;
                this.InitializeShipCenterPosition();
            }
        }
        public void MoveLeft()
        {
            if(this.XPosition - this.Speed > 0)
            {
                this.XPosition -= this.Speed;
                this.InitializeShipCenterPosition();
            }
        }
        public void MoveRight()
        {
            if (this.XPosition + this.Speed + this.ShipWidth < this.MaxXPosition)
            {
                this.XPosition += this.Speed;
                this.InitializeShipCenterPosition();
            }
        }
        public void IncreaseMaxHealth()
        {
            switch (this.MaxHealth)
            {
                case 100:
                    this.MaxHealth = 300;
                    break;
                case 300:
                    this.MaxHealth = 400;
                    break;
                case 400:
                    this.MaxHealth = 600;
                    break;
                case 600:
                    this.MaxHealth = 800;
                    break;
                case 800:
                    this.MaxHealth = 1000;
                    break;
                default:
                    return; // max reached
            }
        }
        public void IncreaseSpeed()
        {
            switch (this.Speed)
            {
                case 15:
                    this.Speed = 20;
                    break;
                case 20:
                    this.Speed = 25;
                    break;
                case 25:
                    this.Speed = 30;
                    break;
                case 30:
                    this.Speed = 35;
                    break;
                default:
                    return; // max reached
            }
        }
    }
}

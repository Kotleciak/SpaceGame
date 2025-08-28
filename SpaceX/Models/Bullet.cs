namespace SpaceX.Models
{
    public class Bullet
    {
        public int ID { get; set; }
        public int YPosition { get; set; }
        public int XPosition { get; set; }
        public int StartYPosition { get; set; }
        public int StartXPosition { get; set; }
        public int MaxYPosition { get; set; }
        public int ShooterID { get; set; }
        public int Speed { get; set; }
        private void MoveUp()
        {
            this.YPosition -= this.Speed;
            this.StartYPosition -= this.Speed;
        }
        private void MoveDown()
        {
            this.YPosition += this.Speed;
            this.StartYPosition += this.Speed;
        }
        public void Move()
        {
            if(this.ShooterID == 0)
            {
                this.MoveUp();
            }
            else
            {
                this.MoveDown();
            }
        }
        public bool IsAlive()
        {
            return this.YPosition > -20 && this.YPosition < this.MaxYPosition + 30;
        }
    }
}

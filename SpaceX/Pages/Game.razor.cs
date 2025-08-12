using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using SpaceX.Models;
using System.Security.AccessControl;
using System.Threading;

namespace SpaceX.Pages
{
    public partial class Game
    {
        private Canvas2DContext _context;
        protected BECanvasComponent _canvasReference;
        private Canvas2DContext _bulletContext;
        protected BECanvasComponent _bulletCanvasReference;
        private ElementReference CanvaContainer;
        private ElementReference BulletsContainer;

        private int _canvasWidth = 300;
        private int _canvasHeight = 400;
        private List<Bullet> _bullets = new List<Bullet>();
        private List<Asteroid> _asteroids = new List<Asteroid>();
        Ship myShip = new Ship
        {
            ID = 1
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _canvasHeight = await JS.InvokeAsync<int>("getScreenHeight");
                _canvasWidth = await JS.InvokeAsync<int>("getScreenWidth");
                StateHasChanged();
                await JS.InvokeVoidAsync("focusElement", CanvaContainer);
                this._context = await this._canvasReference.CreateCanvas2DAsync();
                this._bulletContext = await this._bulletCanvasReference.CreateCanvas2DAsync();
                await this._context.SetFillStyleAsync("green");
                await this._context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipWidth, myShip.ShipHeight);
                StateHasChanged();
                Asteroid asteroid = new Asteroid
                {
                    Health = 10,
                    XPosition = 50,
                    YPosition = 50,
                    Speed = 20,
                    Size = Asteroid.AsteroidSize.Small
                };
                _asteroids.Add(asteroid);
                await UpdateBullets();
            }
        }
        protected async Task Move(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "w":
                    myShip.MoveUp(30);
                    break;
                case "s":
                    myShip.MoveDown(30);
                    break;
                case "a":
                    myShip.MoveLeft(30);
                    break;
                case "d":
                    myShip.MoveRight(30);
                    break;
                default:
                    return;
            }


            await _context.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);
            await _context.SetFillStyleAsync("green");
            await _context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipWidth, myShip.ShipHeight);
            foreach (var asteroid in _asteroids)
            {
                asteroid.MoveDown();
                await this._context.SetFillStyleAsync("gray");
                await this._context.FillRectAsync(asteroid.XPosition, asteroid.YPosition, 20, 20);
            }

            await JS.InvokeVoidAsync("focusElement", CanvaContainer);
        }
        private async Task SendNewBullet(MouseEventArgs e)
        {
            Console.WriteLine("wysylam bulleta");
            Bullet newBullet = new Bullet
            {
                ID = _bullets.Count,
                XPosition = myShip.XPosition + (myShip.ShipWidth / 2),
                YPosition = myShip.YPosition - 20,
                StartXPosition = myShip.XPosition + (myShip.ShipWidth / 2),
                StartYPosition = myShip.YPosition
            };
            _bullets.Add(newBullet);
            Console.WriteLine("bullet zrobiony");

            
        }
        private async Task UpdateBullets()
        {
            await _bulletContext.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);
            foreach (var bullet in _bullets)
            {
                bullet.MoveUp();
                await this._bulletContext.SetStrokeStyleAsync("red");
                await this._bulletContext.SetLineWidthAsync(5);
                await this._bulletContext.BeginPathAsync();
                await this._bulletContext.MoveToAsync(bullet.StartXPosition, bullet.StartYPosition);
                await this._bulletContext.LineToAsync(bullet.XPosition, bullet.YPosition);
                await this._bulletContext.StrokeAsync();
            }
            await UpdateShipsAndAsteroids();
            await Task.Delay(50);
            await UpdateBullets();
        }
        private async Task UpdateShipsAndAsteroids()
        {
            Console.WriteLine("Ships and asteroids updated");
            await _context.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);
            List<Asteroid> asteroidsToRemove = new List<Asteroid>();
            foreach (var asteroid in _asteroids)
            {
                Console.WriteLine("x position " + asteroid.XPosition);
                Console.WriteLine("y position " + asteroid.YPosition);
                asteroid.MoveDown();
                await this._context.SetFillStyleAsync("gray");
                await this._context.FillRectAsync(asteroid.XPosition, asteroid.YPosition, 20, 20);
                if (!asteroid.IsAlive(_canvasHeight, _canvasWidth))
                {
                    asteroidsToRemove.Add(asteroid);
                }
            }
            foreach (var asteroid in asteroidsToRemove)
            {
                _asteroids.Remove(asteroid);
                Console.WriteLine("wylecial");
            }
            await this._context.SetFillStyleAsync("green");
            await this._context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipHeight, myShip.ShipWidth);
        }
        private async Task NextLevel()
        {

        }
    }
}

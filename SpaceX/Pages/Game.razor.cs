using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SpaceX.Models;
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
                await UpdateBullets();
            }
        }
        protected async Task Move(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "ArrowUp":
                    myShip.MoveUp(30);
                    break;
                case "ArrowDown":
                    myShip.MoveDown(30);
                    break;
                case "ArrowLeft":
                    myShip.MoveLeft(30);
                    break;
                case "ArrowRight":
                    myShip.MoveRight(30);
                    break;
                default:
                    return;
            }


            await _context.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);
            await _context.SetFillStyleAsync("green");
            await _context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipWidth, myShip.ShipHeight);

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
                Console.WriteLine("start x bulleta to: " + bullet.StartXPosition);
                Console.WriteLine("start y bulleta to: " + bullet.StartYPosition);
                await this._bulletContext.MoveToAsync(bullet.StartXPosition, bullet.StartYPosition);
                await this._bulletContext.LineToAsync(bullet.XPosition, bullet.YPosition);
                await this._bulletContext.StrokeAsync();
            }
            await Task.Delay(1000);
            await UpdateBullets();
        }
    }
}

using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SpaceX.Models;

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
            ID = 1,
            XPosition = 10,
            YPosition = 10,
            Health = 100,
            ShipWidth = 100,
            ShipHeight = 100
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Console.WriteLine("doszlo tutaj");
            if (firstRender)
            {
                Console.WriteLine("to jest prawda");
                _canvasHeight = await JS.InvokeAsync<int>("getScreenHeight");
                _canvasWidth = await JS.InvokeAsync<int>("getScreenWidth");
                StateHasChanged();
                await JS.InvokeVoidAsync("focusElement", CanvaContainer); 
                this._context = await this._canvasReference.CreateCanvas2DAsync();
                await this._context.SetFillStyleAsync("green");
                await this._context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipWidth, myShip.ShipHeight);
                StateHasChanged();
            }
        }
        protected async Task Move(KeyboardEventArgs e)
        {
            Console.WriteLine("time to move!");
            switch (e.Key)
            {
                case "ArrowUp":
                    myShip.MoveUp(30);
                    Console.WriteLine("up");
                    break;
                case "ArrowDown":
                    myShip.MoveDown(30);
                    Console.WriteLine("down");
                    break;
                case "ArrowLeft":
                    myShip.MoveLeft(30);
                    Console.WriteLine("left");
                    break;
                case "ArrowRight":
                    myShip.MoveRight(30);
                    Console.WriteLine("right");
                    break;
                default:
                    Console.WriteLine("kupa");
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
                YPosition = 0
            };
            Console.WriteLine("bullet zrobiony");
            /*
            await this._bulletContext.SetStrokeStyleAsync("red");
            await this._bulletContext.SetLineWidthAsync(5);
            await this._bulletContext.BeginPathAsync();
            await this._bulletContext.MoveToAsync(myShip.XPosition + (myShip.ShipWidth / 2), myShip.YPosition);
            await this._bulletContext.LineToAsync(newBullet.XPosition, newBullet.YPosition);
            await this._bulletContext.StrokeAsync();
            */

        }
    }
}

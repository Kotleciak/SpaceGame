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

        private ElementReference CanvaContainer;
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
            if (firstRender)
            {
                _canvasHeight = await JS.InvokeAsync<int>("getScreenHeight");
                _canvasWidth = await JS.InvokeAsync<int>("getScreenWidth");
                await JS.InvokeVoidAsync("focusElement", CanvaContainer); 
                this._context = await this._canvasReference.CreateCanvas2DAsync();
                await this._context.SetFillStyleAsync("green");
                await this._context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipWidth, myShip.ShipHeight);
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
            int xposition = Convert.ToInt32(e.OffsetX);
            int yposition = Convert.ToInt32(e.OffsetY);
            double a = (myShip.YPosition - yposition) / (myShip.XPosition - xposition);
            double b = yposition - a * xposition;
            


            await this._context.SetStrokeStyleAsync("red");
            await this._context.SetLineWidthAsync(5);
            await this._context.BeginPathAsync();
            await this._context.MoveToAsync(myShip.XPosition, myShip.YPosition);
            await this._context.LineToAsync(xposition, yposition);
            await this._context.StrokeAsync();

        }
    }
}

using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Threading;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
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

        private string _eqDisplayed = "none";
        private string[] ShopElements = new string[] { "MaxHealth", "Speed", "Damage", "BulletS" };
        private int _shopElementIndex;

        private List<Bullet> _bullets = new List<Bullet>();
        private List<Asteroid> _asteroids = new List<Asteroid>();
        private List<EnemyShip> _enemyShips = new List<EnemyShip>();
        Ship myShip = new Ship()
        {
            ID = 0
        };
        EnemyShip enemyShip = new EnemyShip()
        {
            ID = 1
        };
        private GameOptions _gameOptions = new GameOptions();
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
                    XPosition = 50,
                    Speed = 1,
                    Size = Asteroid.AsteroidSize.Large
                };
                asteroid.InitializeAsteroidSize();
                asteroid.GetRandomXPosition(_canvasWidth);
                _asteroids.Add(asteroid);
                _enemyShips.Add(enemyShip);
                myShip.SetMaxPositions(_canvasWidth, _canvasHeight);
                enemyShip.SetMaxPositions(_canvasWidth, _canvasHeight);
                myShip.XPosition = (_canvasWidth / 2) - (myShip.ShipWidth / 2);
                myShip.YPosition = _canvasHeight - myShip.ShipHeight - 10;
                myShip.InitializeShipCenterPosition();
                _gameOptions.Level = 1;
                _gameOptions.Coins = 0;
                await UpdateBullets();
            }
        }
        protected async Task Move(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "w":
                    myShip.MoveUp();
                    break;
                case "s":
                    myShip.MoveDown();
                    break;
                case "a":
                    myShip.MoveLeft();
                    break;
                case "d":
                    myShip.MoveRight();
                    break;
                case "e":
                    await OpenEquipment();
                    break;
                case "ArrowUp":
                    await MoveShopElementUp();
                    break;
                case "ArrowDown":
                    await MoveShopElementDown();
                    break;
                case " ":
                    await CommitShopPurchase();
                    break;
                default:
                    return;
            }


            await _context.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);
            await _context.SetFillStyleAsync("green");
            await _context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipWidth, myShip.ShipHeight);
            foreach (var asteroid in _asteroids)
            {
                await this._context.SetFillStyleAsync("gray");
                await this._context.FillRectAsync(asteroid.XPosition, asteroid.YPosition, asteroid.AsteroidWidth, asteroid.AsteroidHeigth);
            }
            await this._context.SetFillStyleAsync("red");
            await this._context.FillRectAsync(enemyShip.XPosition, enemyShip.YPosition, enemyShip.ShipHeight, enemyShip.ShipWidth);

            await JS.InvokeVoidAsync("focusElement", CanvaContainer);
        }
        private async Task SendNewBullet(int ID)
        {
            Bullet newBullet = new Bullet
            {
                ID = _bullets.Count,
                ShooterID = ID,
                MaxYPosition = _canvasHeight
            };
            if(ID == 0)
            {
                newBullet.XPosition = myShip.XPosition + (myShip.ShipWidth / 2);
                newBullet.YPosition = myShip.YPosition - 20;
                newBullet.StartXPosition = myShip.XPosition + (myShip.ShipWidth / 2);
                newBullet.StartYPosition = myShip.YPosition;
            }
            else
            {
                var enemy = _enemyShips.Where(x => x.ID == ID).First();
                newBullet.XPosition = enemy.XPosition + (enemy.ShipWidth / 2);
                newBullet.YPosition = enemy.YPosition + 20;
                newBullet.StartXPosition = enemy.XPosition + (enemy.ShipWidth / 2);
                newBullet.StartYPosition = enemy.YPosition + enemy.ShipHeight;
            }
            _bullets.Add(newBullet);
        }
        private async Task UpdateBullets()
        {
            await _bulletContext.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);
            List<Bullet> bulletsToRemove = new List<Bullet>();
            foreach (var bullet in _bullets)
            {
                if (!bullet.IsAlive())
                {
                    bulletsToRemove.Add(bullet);
                }
                bullet.Move();
                if(bullet.ShooterID == 0)
                {
                    await this._bulletContext.SetStrokeStyleAsync("green");
                }
                else
                {
                    await this._bulletContext.SetStrokeStyleAsync("red");
                }
                await this._bulletContext.SetLineWidthAsync(5);
                await this._bulletContext.BeginPathAsync();
                await this._bulletContext.MoveToAsync(bullet.StartXPosition, bullet.StartYPosition);
                await this._bulletContext.LineToAsync(bullet.XPosition, bullet.YPosition);
                await this._bulletContext.StrokeAsync();
            }
            foreach (var bullet in bulletsToRemove)
            {
                _bullets.Remove(bullet);
            }
            bulletsToRemove.Clear();
            await UpdateShipsAndAsteroids();
            await Task.Delay(50);
            await CheckIfBulletsTookDamage();
            await UpdateBullets();
        }
        private async Task UpdateShipsAndAsteroids()
        {
            await _context.ClearRectAsync(0, 0, _canvasWidth, _canvasHeight);
            List<Asteroid> asteroidsToRemove = new List<Asteroid>();
            foreach (var asteroid in _asteroids)
            {
                asteroid.MoveDown();
                await this._context.SetFillStyleAsync("gray");
                await this._context.FillRectAsync(asteroid.XPosition, asteroid.YPosition, asteroid.AsteroidWidth, asteroid.AsteroidHeigth);
                if (!asteroid.IsAlive(_canvasHeight, _canvasWidth))
                {
                    asteroidsToRemove.Add(asteroid);
                }
            }
            foreach (var asteroid in asteroidsToRemove)
            {
                _asteroids.Remove(asteroid);
            }
            asteroidsToRemove.Clear();
            await this._context.SetFillStyleAsync("green");
            await this._context.FillRectAsync(myShip.XPosition, myShip.YPosition, myShip.ShipHeight, myShip.ShipWidth);
            foreach(var enemy in _enemyShips)
            {
                if (enemy.YPosition < 30)
                {
                    enemy.MoveDown();
                }
                enemy.AttackOrAvoid(myShip.XCenterPosition);
                if (enemy.IsAttacking && enemy.ThirdCountAttack == 3)
                {
                    await SendNewBullet(enemy.ID);
                    enemy.ThirdCountAttack = 0;
                }
                else if (enemy.IsAttacking)
                {
                    enemy.ThirdCountAttack++;
                    Console.WriteLine(_bullets.Count);
                }
                await this._context.SetFillStyleAsync("red");
                await this._context.FillRectAsync(enemy.XPosition, enemy.YPosition, enemy.ShipHeight, enemy.ShipWidth);
            }
        }
        private async Task NextLevel()
        {
            Console.WriteLine("Next Level");
        }
        private async Task CheckIfBulletsTookDamage()
        {
            var bulletsToRemove = new HashSet<Bullet>();
            var asteroidsDestroyed = new HashSet<Asteroid>();
            var enemyShipsDestroyed = new HashSet<EnemyShip>();

            
            
            foreach (var bullet in _bullets)
            {
                foreach (var asteroid in _asteroids)
                {
                    asteroid.InitializeAsteroidCenterPosition();
                    double distanceSquared = Math.Pow(asteroid.XCenterPosition - bullet.XPosition, 2)
                                              + Math.Pow(asteroid.YCenterPosition - bullet.YPosition, 2);
                    if (distanceSquared < 2500 && bullet.ShooterID == 0)
                    {
                        _gameOptions.Coins = _gameOptions.Coins + _asteroids.Where(x => x == asteroid).FirstOrDefault().AsteroidHit();
                        if (asteroid.Health <= 0)
                        {
                            asteroidsDestroyed.Add(asteroid);
                            _gameOptions.Coins += 1;
                        }
                        bulletsToRemove.Add(bullet);
                    }
                }
                foreach (var enemy in _enemyShips)
                {
                    double distanceSquared = Math.Pow(enemy.XCenterPosition - bullet.XPosition, 2)
                                          + Math.Pow(enemy.YCenterPosition - bullet.YPosition, 2);
                    if (distanceSquared < 2500 && bullet.ShooterID == 0)
                    {
                        enemy.ShipTookDamage(myShip.LevelOfBulletsDmg * 10);
                        Console.WriteLine("Enemy ship took damage! His health is " + enemy.Health);
                        if (!enemy.IsAlive())
                        {
                            _gameOptions.Coins = _gameOptions.Coins + 20; //I set 20 as placeholder
                            enemyShipsDestroyed.Add(enemy);
                            _gameOptions.Coins += 1;
                        }
                        bulletsToRemove.Add(bullet);
                    }
                }
                double myShipDistanceSquared = Math.Pow(myShip.XCenterPosition - bullet.XPosition, 2)
                                          + Math.Pow(myShip.YCenterPosition - bullet.YPosition, 2);
                if (myShipDistanceSquared < 2500 && bullet.ShooterID != 0)
                {
                    myShip.ShipTookDamage(10);
                    Console.WriteLine("My ship took damage! My health is " + myShip.Health);
                    bulletsToRemove.Add(bullet);
                }
            }


            foreach (var bullet in bulletsToRemove)
            {
                _bullets.Remove(bullet);
            }
            foreach (var asteroid in asteroidsDestroyed)
            {
                _asteroids.Remove(asteroid);
            }
            foreach (var enemy in enemyShipsDestroyed)
            {
                _enemyShips.Remove(enemy);
            }
        }
        private async Task OpenEquipment()
        {
            if(_eqDisplayed == "none")
            {
                _shopElementIndex = 0;
                _eqDisplayed = "block";
            }
            else
            {
                _eqDisplayed = "none";
            }
            await JS.InvokeVoidAsync("ShopElementChanged", ShopElements[_shopElementIndex], myShip.GetCurrentShopPrices());
        }
        private async Task MoveShopElementUp()
        {
            if (_shopElementIndex >= 1 && _eqDisplayed == "block")
            {
                _shopElementIndex--;
                await JS.InvokeVoidAsync("ShopElementChanged", ShopElements[_shopElementIndex], myShip.GetCurrentShopPrices());
            }
        }
        private async Task MoveShopElementDown()
        {
            if (_shopElementIndex < 3 && _eqDisplayed == "block") 
            {
                _shopElementIndex++;
                await JS.InvokeVoidAsync("ShopElementChanged", ShopElements[_shopElementIndex], myShip.GetCurrentShopPrices());
            }
        }
        private async Task CommitShopPurchase()
        {
            if (_eqDisplayed == "block")
            {
                switch (ShopElements[_shopElementIndex])
                {
                    case "MaxHealth":
                        myShip.IncreaseMaxHealth();
                        break;
                    case "Speed":
                        myShip.IncreaseSpeed();
                        break;
                    case "Damage":
                        if(myShip.LevelOfBulletsDmg + 1 < 5)
                        {
                            myShip.LevelOfBulletsDmg++;
                        }
                        break;
                    case "BulletS":
                        if(myShip.LevelOfBulletsSpeed + 1 < 5)
                        {
                            myShip.LevelOfBulletsSpeed++;
                        }
                        break;
                }
                _gameOptions.Coins -= 10; 
                await JS.InvokeVoidAsync("ShopElementChanged", ShopElements[_shopElementIndex], myShip.GetCurrentShopPrices());
            }
        }
    }
}

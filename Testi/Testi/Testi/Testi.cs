using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class Testi : PhysicsGame
{
    PhysicsObject pallo;

    public override void Begin()
    {
        LuoKentta();
        LuoOhjaimet();
        Mouse.IsCursorVisible = true;

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
    }
    void LuoKentta()
    {
        pallo = new PhysicsObject(130, 130);
        pallo.Shape = Shape.Circle;
        pallo.Color = Color.White;
        pallo.X = 0;
        pallo.Y = 0;
        Add(pallo);

        Level.CreateBorders();
        Level.Background.Color = Color.LightBlue;
    }
    void pallonsuunta()
    {
        Vector suunta = new Vector(Mouse.PositionOnWorld.X + pallo.X / 1000, Mouse.PositionOnWorld.Y + pallo.Y / 1000);
        pallo.Hit(suunta);
    }
    void LuoOhjaimet()
    {
        Mouse.Listen(MouseButton.Left, ButtonState.Pressed, pallonsuunta, null);
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
}

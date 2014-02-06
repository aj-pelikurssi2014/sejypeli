using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class Tankkipeli : PhysicsGame
{
    Tank tankki1;
    Tank tankki2;

    Vector vauhti1 = new Vector(-30, 0);
    Vector vauhti2 = new Vector(30, 0);

    //Pelin aloittaminen
    public override void Begin()
    {
        LuoKentta();
        AsetaOhjaimet();

        tankki1 = LuoTankki(Level.Left + 50, -350, 1000, 0);
        tankki2 = LuoTankki(Level.Right - 50, -350, 1000, 180);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
    }

    //Tankkien määrittäminen
    Tank LuoTankki(double x, double y, double massa, double kulma)
    {
        Tank tankki = new Tank(200, 70);
        tankki.X = x;
        tankki.Y = y;
        tankki.Mass = massa;
        tankki.Cannon.Angle = Angle.FromDegrees(kulma);

        Add(tankki);
        return tankki;
    }

    //Kentän ja painovoiman luominen
    void LuoKentta()
    {
        Level.CreateBottomBorder();
        Gravity = new Vector(0, -1500);
    }

    //Vasemman pelaajan vektorit ja toiminnot
    void LiikutaTankki1Vasemmalle()
    {
        tankki1.Velocity = tankki1.Velocity + vauhti1;
    }
    void LiikutaTankki1Oikealle()
    {
        tankki1.Velocity = tankki1.Velocity + vauhti2;
    }
    void KeuliTankki1()
    {
        tankki1.ApplyTorque(200000000);
    }
    void Tankki1Ampuminen()
    {
        tankki1.Shoot(50000);
    }
    void KaannaTykki1Ylos()
    {
        double tykinkulma = tankki1.Cannon.Angle.Degrees;
        double uusikulma = tykinkulma + 2;

        if (uusikulma <= 90)
        {
            tankki1.Cannon.Angle = Angle.FromDegrees(uusikulma);
        }
    }
    void KaannaTykki1Alas()
    {
        double tykinkulma = tankki1.Cannon.Angle.Degrees;
        double uusikulma = tykinkulma - 2;

        if (uusikulma > 0)
        {
            tankki1.Cannon.Angle = Angle.FromDegrees(uusikulma);
        }
    }

    //Oikean pelaajan vektorit ja toiminnot
    void LiikutaTankki2Vasemmalle()
    {
        tankki2.Velocity = tankki2.Velocity + vauhti1;
    }
    void LiikutaTankki2Oikealle()
    {
        tankki2.Velocity = tankki2.Velocity + vauhti2;
    }
    void KeuliTankki2()
    {
        tankki2.ApplyTorque(-200000000);
    }
    void Tankki2Ampuminen()
    {
        tankki2.Shoot(50000);
    }
    void KaannaTykki2Ylos()
    {
        double tykinkulma = tankki2.Cannon.Angle.Degrees;
        double uusikulma = tykinkulma - 2;

        if (uusikulma >= 90)
        {
            tankki2.Cannon.Angle = Angle.FromDegrees(uusikulma);
        }
    }
    void KaannaTykki2Alas()
    {
        double tykinkulma = tankki2.Cannon.Angle.Degrees;
        double uusikulma = tykinkulma + 2;

        if (uusikulma < 270)
        {
            tankki2.Cannon.Angle = Angle.FromDegrees(uusikulma);
        }
    }

    //Ohjainten luominen
    void AsetaOhjaimet()
    {
        //Pelistä poistuminen
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        //Vasemman pelaajan ohjaimet
        Keyboard.Listen(Key.A, ButtonState.Down, LiikutaTankki1Vasemmalle, null);
        Keyboard.Listen(Key.D, ButtonState.Down, LiikutaTankki1Oikealle, null);
        Keyboard.Listen(Key.LeftShift, ButtonState.Down, KeuliTankki1, null);
        Keyboard.Listen(Key.Space, ButtonState.Down, Tankki1Ampuminen, null);
        Keyboard.Listen(Key.W, ButtonState.Down, KaannaTykki1Ylos, null);
        Keyboard.Listen(Key.S, ButtonState.Down, KaannaTykki1Alas, null);
        //Oikean pelaajan ohjaimet
        Keyboard.Listen(Key.Left, ButtonState.Down, LiikutaTankki2Vasemmalle, null);
        Keyboard.Listen(Key.Right, ButtonState.Down, LiikutaTankki2Oikealle, null);
        Keyboard.Listen(Key.RightShift, ButtonState.Down, KeuliTankki2, null);
        Keyboard.Listen(Key.RightControl, ButtonState.Down, Tankki2Ampuminen, null);
        Keyboard.Listen(Key.Up, ButtonState.Down, KaannaTykki2Ylos, null);
        Keyboard.Listen(Key.Down, ButtonState.Down, KaannaTykki2Alas, null);
    }
}

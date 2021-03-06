﻿using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class Tasohyppelypeli1 : PhysicsGame
{
    const double nopeus = 200;
    const double hyppyNopeus = 750;
    const int RUUDUN_KOKO = 40;
    bool pelaajaAlhaalla = false;
    PlatformCharacter pelaaja1;

    Image tasoKuva = LoadImage("taso");
    Image taso2Kuva = LoadImage("taso2");
    Image kallioKuva = LoadImage("kallio");
    Image pelaajanKuva = LoadImage("norsu");
    Image pelaajanKuvaKyykyssa = LoadImage("litistynytnorsu");
    Image tahtiKuva = LoadImage("tahti");
    Image tntKuva = LoadImage("tnt");
    Image rajahtanytKuva = LoadImage("rajahtanytSeina");

    SoundEffect maaliAani = LoadSoundEffect("maali");

    public override void Begin()
    {
        Gravity = new Vector(0, -1000);

        LuoKentta();
        LisaaNappaimet();

        Camera.Follow(pelaaja1);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
    }

    void LuoKentta()
    {
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("kentta");
        ruudut.SetTileMethod(Color.FromHexCode("56FF56"), LisaaTaso);
        ruudut.SetTileMethod(Color.FromHexCode("7F3F23"), LisaaTaso2);
        ruudut.SetTileMethod(Color.FromHexCode("999999"), LisaaKallio);
        ruudut.SetTileMethod(Color.FromHexCode("FFFF00"), LisaaTahti);
        ruudut.SetTileMethod(Color.FromHexCode("FF0000"), LisaaRajahde);
        ruudut.SetTileMethod(Color.FromHexCode("00FFE5"), LisaaPelaaja);
        ruudut.Execute(RUUDUN_KOKO, RUUDUN_KOKO);
        Level.CreateBorders();
        Level.Background.CreateGradient(Color.White, Color.SkyBlue);
    }

    void LisaaTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Tag = "seina";
        taso.Image = tasoKuva;
        taso.CollisionIgnoreGroup = 1;
        Add(taso);
    }

    void LisaaTaso2(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso2 = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso2.Position = paikka;
        taso2.Tag = "seina";
        taso2.Image = taso2Kuva;
        taso2.CollisionIgnoreGroup = 1;
        Add(taso2);
    }

    void LisaaTahti(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject tahti = PhysicsObject.CreateStaticObject(leveys, korkeus);
        tahti.IgnoresCollisionResponse = true;
        tahti.Position = paikka;
        tahti.Image = tahtiKuva;
        tahti.Tag = "tahti";
        Add(tahti);
    }

    void LisaaKallio(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject kallio = PhysicsObject.CreateStaticObject(leveys, korkeus);
        kallio.Position = paikka;
        kallio.Image = kallioKuva;
        kallio.Tag = "kallio";
        kallio.CollisionIgnoreGroup = 1;
        Add(kallio);
    }

    void LisaaRajahde(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject rajahde = PhysicsObject.CreateStaticObject(leveys, korkeus);
        rajahde.Position = paikka;
        rajahde.Image = tntKuva;
        rajahde.Tag = "rajahde";
        Add(rajahde);
    }

    void LisaaPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new PlatformCharacter(leveys, korkeus);
        pelaaja1.Position = paikka;
        pelaaja1.Mass = 4.0;
        pelaaja1.Image = pelaajanKuva;
        AddCollisionHandler(pelaaja1, "tahti", TormaaTahteen);
        Add(pelaaja1);

        pelaaja1.Weapon = new AssaultRifle(10, 2);

        pelaaja1.Weapon.InfiniteAmmo = true;
        pelaaja1.Weapon.ProjectileCollision = ammusosuu;
        pelaaja1.Weapon.Power.Value = 300;
        pelaaja1.Weapon.Power.DefaultValue = 300;
        pelaaja1.Weapon.FireRate = 2;
        pelaaja1.Weapon.AmmoIgnoresGravity = true;
        pelaaja1.Weapon.AmmoIgnoresExplosions = true;
        pelaaja1.Weapon.CanHitOwner = false;
        pelaaja1.Weapon.Image = null;
        pelaaja1.Weapon.AttackSound = null;

        pelaaja1.Weapon.X = 0.0;
        pelaaja1.Weapon.Y = 0.0;
        pelaaja1.Weapon.Angle = Angle.FromDegrees(0);
    }

    void ammusosuu(PhysicsObject ammus, PhysicsObject kohde)
    {
        if (kohde.Tag.ToString() == "seina")
        {
            Explosion pum = new Explosion(20.0);
            pum.Position = ammus.Position;
            pum.UseShockWave = false;
            Add(pum);
            ammus.Destroy();
        }
        if (kohde.Tag.ToString() == "tahti")
        {
            Explosion pum = new Explosion(100.0);
            pum.Position = ammus.Position;
            Add(pum);
            pum.ShockwaveReachesObject += RajahdysTapahtuu;
            MessageDisplay.Add("Tuhosit tähden!");
            ammus.Destroy();
            kohde.Destroy();
        }
        if (kohde.Tag.ToString() == "rajahde")
        {
            Explosion pum = new Explosion(300.0);
            pum.Position = ammus.Position;
            Add(pum);
            pum.ShockwaveReachesObject += RajahdysTapahtuu;
            ammus.Destroy();
            kohde.Destroy();
        }
    }

    void RajahdysTapahtuu(IPhysicsObject kohde, Vector shokki)
    {
        if (kohde.Tag.ToString() == "seina")
        {
            PhysicsObject rajahtanytSeina = PhysicsObject.CreateStaticObject(RUUDUN_KOKO, RUUDUN_KOKO);
            rajahtanytSeina.Position = kohde.Position;
            rajahtanytSeina.Image = rajahtanytKuva;
            rajahtanytSeina.CollisionIgnoreGroup = 1;
            rajahtanytSeina.IgnoresCollisionResponse = true; 
            Add(rajahtanytSeina, -1);

            kohde.Destroy();
        }
    }

    void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, -nopeus);
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, nopeus);
        Keyboard.Listen(Key.Down, ButtonState.Down, MeneAlas, "Menee kyykkyyn", pelaaja1);
        Keyboard.Listen(Key.Down, ButtonState.Released, NouseYlos, "Nousee ylös", pelaaja1);
        Keyboard.Listen(Key.A, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);
        Keyboard.Listen(Key.S, ButtonState.Down, ammu, "Pelaaja ampuu", pelaaja1);

        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");

        ControllerOne.Listen(Button.DPadLeft, ButtonState.Down, Liikuta, "Pelaaja liikkuu vasemmalle", pelaaja1, -nopeus);
        ControllerOne.Listen(Button.DPadRight, ButtonState.Down, Liikuta, "Pelaaja liikkuu oikealle", pelaaja1, nopeus);
        ControllerOne.Listen(Button.A, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
    }

    void Liikuta(PlatformCharacter hahmo, double nopeus)
    {
        if (pelaajaAlhaalla == true)
        {
            hahmo.Walk(nopeus / 2);
        }
        else
        {
            hahmo.Walk(nopeus);
        }
    }

    void MeneAlas(PlatformCharacter hahmo)
    {
        pelaajaAlhaalla = true;
        hahmo.Image = pelaajanKuvaKyykyssa;
    }

    void NouseYlos(PlatformCharacter hahmo)
    {
        pelaajaAlhaalla = false;
        hahmo.Image = pelaajanKuva;
    }

    void Hyppaa(PlatformCharacter hahmo, double nopeus)
    {
        if (pelaajaAlhaalla == true)
        {
            //Älä tee mitään
        }
        else
        {
            hahmo.Jump(nopeus);
        }
    }

    void ammu(PlatformCharacter pelaaja1)
    {
        PhysicsObject ammus = pelaaja1.Weapon.Shoot();

        if (ammus != null)
        {
            ammus.Size *= 1;
            ammus.MaximumLifetime = TimeSpan.FromSeconds(3.0);
        }
    }

    void TormaaTahteen(PhysicsObject hahmo, PhysicsObject tahti)
    {
        maaliAani.Play();
        MessageDisplay.Add("Keräsit tähden!");
        tahti.Destroy();
    }
}
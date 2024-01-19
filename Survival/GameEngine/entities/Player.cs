﻿using Survival.GameEngine.Inventory.ItemComponent;
using Survival.GameEngine.Inventory;
using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Survival.GameEngine.Inventory.ItemComponent;
using System.Windows;

namespace Survival
{
    internal class Player : LivingEntity
    {
        private Artifact itemEquiped;

        public Artifact ItemEquiped
        {
            get
            {
                return this.itemEquiped;
            }
            set
            {
                this.itemEquiped = value;
            }
        }

        private int money;

        public int Money
        {
            get
            {
                return this.money;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Money must be positive");
                }
                this.money = value;
            }
        }

        private Inventory inventory = new Inventory();
        public Inventory Inventory { get => inventory; set => inventory = value; }

        //animation de face
        ImageBrush face = new ImageBrush();
        ImageBrush pied_droit = new ImageBrush();
        ImageBrush pied_gauche = new ImageBrush();

        ImageBrush[] animeFace = new ImageBrush[3];

        //animation de dos

        ImageBrush dos = new ImageBrush();
        ImageBrush dos_droit = new ImageBrush();
        ImageBrush dos_gauche = new ImageBrush();

        ImageBrush[] animeDos = new ImageBrush[3];

        //animation de coté gauche
        ImageBrush cote_gauche = new ImageBrush();
        ImageBrush cote_gauche_pied = new ImageBrush();

        ImageBrush[] animeCoteGauche = new ImageBrush[2];


        //animation de coté droit
        ImageBrush cote_droit = new ImageBrush();
        ImageBrush cote_droit_pied = new ImageBrush();

        ImageBrush[] animeCoteDroit = new ImageBrush[2];

        


        public Player(string name, int life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(name, life, texture, position, velocity)
        {
            //chargement des images de face
            face.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\face.png"));
            pied_droit.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\pied_droit.png"));
            pied_gauche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\pied_gauche.png"));

            animeFace[0] = face;
            animeFace[1] = pied_droit;
            animeFace[2] = pied_gauche;

            // chargement des images de dos
            dos.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\dos.png"));
            dos_droit.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\dos_droit.png"));
            dos_gauche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\dos_gauche.png"));

            animeDos[0] = dos;
            animeDos[1] = dos_gauche;
            animeDos[2] = dos_droit;

            // chargement des images sur le coté gauche

            cote_gauche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\coté_gauche.png"));
            cote_gauche_pied.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\cote_gauche_pied.png"));

            animeCoteGauche[0] = cote_gauche;
            animeCoteGauche[1] = cote_gauche_pied;

            //chargement des images sur le coté droit

            cote_droit.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\coté_droit.png"));
            cote_droit_pied.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\cote_droit_pied.png"));

            animeCoteDroit[0] = cote_droit;
            animeCoteDroit[1] = cote_droit_pied;

            this.ItemEquiped = (Artifact)Inventory.ITEMS_POSSIBLE["Artifact"][0];
        }

        int accFace = 0;
        int accDos = 0;
        int accCoteGauche = 0;
        int accCoteDroit = 0;
        int accAnimation = 0;
        Vector2 AncienVecteur = Vector2.Zero;
        public static readonly int LATENCE = 7;

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (this.Velocity != Vector2.Zero)
            {
                accAnimation++;
                if (accAnimation == LATENCE || this.Velocity.X != AncienVecteur.X || this.Velocity.Y != AncienVecteur.Y)
                {
                    accAnimation = 0;
                    if (this.Velocity.Y == 1)
                    {

                        if (accFace == animeFace.Length)
                        {
                            accFace = 0;
                        }
                        this.Rectangle.Fill = animeFace[accFace];
                        accFace++;
                    }
                    else if (this.Velocity.Y == -1)
                    {

                        if (accDos == animeDos.Length)
                        {
                            accDos = 0;
                        }
                        this.Rectangle.Fill = animeDos[accDos];
                        accDos++;
                    }
                    else if (this.Velocity.X == 1)
                    {

                        if (accCoteGauche == animeCoteGauche.Length)
                        {
                            accCoteGauche = 0;
                        }
                        this.Rectangle.Fill = animeCoteGauche[accCoteGauche];
                        accCoteGauche++;
                    }
                    else if (this.Velocity.X == -1)
                    {

                        if (accCoteDroit == animeCoteDroit.Length)
                        {
                            accCoteDroit = 0;
                        }
                        this.Rectangle.Fill = animeCoteDroit[accCoteDroit];
                        accCoteDroit++;
                    }

                }
                AncienVecteur = this.Velocity;
            }
        }

        public override void Collide(Entity otherEntity)
        {
            base.Collide(otherEntity);

            if (otherEntity is Mob && (DateTime.Now - this.lastDamageTaken).TotalMilliseconds > 1000)
            {
                float deltaX = this.Position.X - otherEntity.Position.X;
                float deltaY = this.Position.Y - otherEntity.Position.Y;

                Vector2 delta = Vector2.Normalize(new Vector2(deltaX, deltaY));
                this.Velocity = this.Velocity + delta;
                
                this.TakeDamage(((Mob) otherEntity).BaseDamage);
            }
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Engine.Instance.Pause();
            Engine.Instance.EntityToRemove.AddRange(Engine.Instance.Entities);
            // TODO: Death screen
        }

        public void Fire(Vector2 direction)
        {

            if (this.ItemEquiped != null)
            {
                Vector2 velo = direction - this.Position;
                Console.WriteLine(direction + " " + this.Position);
                velo = Vector2.Normalize(velo);
                Engine.Instance.Entities.Add(new Projectile("", this, TimeSpan.FromSeconds(3), this.ItemEquiped.Texture, this.Position, velo));
            }


        }

    }

	

}

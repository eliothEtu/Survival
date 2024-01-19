using Survival.GameEngine.Inventory.ItemComponent;
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

        private double damage;
        public double Damage { get => damage; set => damage = value; }

        private float projectileVelocity;
        public float ProjectileVelocity { get => projectileVelocity; set => projectileVelocity = value; }

        private TimeSpan projectileLifeSpan;
        public TimeSpan ProjectileLifeSpan { get => projectileLifeSpan; set => projectileLifeSpan = value; }

        //animation de face
        ImageBrush face = new ImageBrush();
        ImageBrush pied_droit = new ImageBrush();
        ImageBrush pied_gauche = new ImageBrush();

        ImageBrush[] animeFace = new ImageBrush[2];

        //animation de dos

        ImageBrush dos = new ImageBrush();
        ImageBrush dos_droit = new ImageBrush();
        ImageBrush dos_gauche = new ImageBrush();

        ImageBrush[] animeDos = new ImageBrush[2];

        //animation de coté gauche
        ImageBrush cote_gauche = new ImageBrush();
        ImageBrush cote_gauche_pied = new ImageBrush();

        ImageBrush[] animeCoteGauche = new ImageBrush[2];


        //animation de coté droit
        ImageBrush cote_droit = new ImageBrush();
        ImageBrush cote_droit_pied = new ImageBrush();

        ImageBrush[] animeCoteDroit = new ImageBrush[2];


        public Player(string name, int life, double damage, BitmapImage texture, Vector2 position, Vector2 velocity) : base(name, life, texture, position, velocity)
        {
            this.Damage = damage;
            this.ProjectileVelocity = 1;
            this.ProjectileLifeSpan = TimeSpan.FromSeconds(2);

            //chargement des images de face
            face.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\face.png"));
            pied_droit.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\pied_droit.png"));
            pied_gauche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\pied_gauche.png"));

            //animeFace[0] = face;
            animeFace[0] = pied_droit;
            animeFace[1] = pied_gauche;

            // chargement des images de dos
            dos.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\dos.png"));
            dos_droit.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\dos_droit.png"));
            dos_gauche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\dos_gauche.png"));
            animeDos[0] = dos_gauche;
            animeDos[1] = dos_droit;

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

        }

        int accFace = 0;
        int accDos = 0;
        int accCoteGauche = 0;
        int accCoteDroit = 0;

        double timeAnim = 0;

        public override void Update(float deltaTime)
        {

            if (this.Velocity.Y == 1)
            {
                if (accFace == animeFace.Length)
                {
                    accFace = 0;
                }
                this.Rectangle.Fill = animeFace[accFace];
                if (timeAnim > 0.25)
                {
                    timeAnim = 0;
                    accFace++;
                }
            }
            if (this.Velocity.Y == -1)
            {
                if (accDos == animeDos.Length)
                {
                    accDos = 0;
                }
                this.Rectangle.Fill = animeDos[accDos];
                if (timeAnim > 0.25)
                {
                    timeAnim = 0;
                    accDos++;
                }
            }
            if (this.Velocity.X == 1)
            {
                if (accCoteGauche == animeCoteGauche.Length)
                {
                    accCoteGauche = 0;
                }
                this.Rectangle.Fill = animeCoteGauche[accCoteGauche];
                if (timeAnim > 0.25)
                {
                    timeAnim = 0;
                    accCoteGauche++;
                }
            }
            if (this.Velocity.X == -1)
            {
                if (accCoteDroit == animeCoteDroit.Length)
                {
                    accCoteDroit = 0;
                }
                this.Rectangle.Fill = animeCoteDroit[accCoteDroit];
                if (timeAnim > 0.25)
                {
                    timeAnim = 0;
                    accCoteDroit++;
                }
            }
            timeAnim += deltaTime;
            base.Update(deltaTime);
        }

        public void Fire(Vector2 direction)
        {
            if (this.ItemEquiped != null)
            {
                Vector2 dir = direction - this.Position;
                dir = Vector2.Normalize(dir);
                Engine.Instance.Entities.Add(new Projectile("", this, this.ProjectileLifeSpan, this.ItemEquiped.Texture, this.Position, dir * this.ProjectileVelocity));
            }
        }
    }
}

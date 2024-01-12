using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;

namespace Survival
{
    internal class Player : LivingEntity
    {

        private string itemEquiped;

        public string ItemEquiped
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
				if(value < 0)
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


        public Player(int life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(life, texture, position, velocity)
        {
            Inventory.InventoryList.Add(new Armor("Helmet", "Helmet", "Helmet tier 1", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Helmet R.png")), "Armor", Tuple.Create("Health", 1)));
            Inventory.InventoryList.Add(new Armor("Helmet", "Helmet", "Helmet tier 2", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Helmet 2R.png")), "Armor", Tuple.Create("Health", 1)));
            Inventory.InventoryList.Add(new Armor("Gloves", "Gloves", "Gloves tier1", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\GloveR.png")), "Armor", Tuple.Create("D", 1)));
            Inventory.InventoryList.Add(new Armor("Chestplate", "Chestplate", "Chestplate tier 1", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\ChestplateR.png")), "Armor", Tuple.Create("S", 1)));
            Inventory.InventoryList.Add(new Armor("Boots", "Boots", "Boots tier 1", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\BootsR.png")), "Armor", Tuple.Create("V", 1)));

            Inventory.InventoryList.Add(new Ring("", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\anneaufeu.png")), "Ring", Tuple.Create("Health", 1.0)));
            Inventory.InventoryList.Add(new Ring("", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\anneaufeu.png")), "Ring", Tuple.Create("D", 1.0)));
            Inventory.InventoryList.Add(new Ring("", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\anneaufeu.png")), "Ring", Tuple.Create("D", 1.0)));
            Inventory.InventoryList.Add(new Ring("", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\anneaufeu.png")), "Ring", Tuple.Create("V", 1.0)));

            Inventory.InventoryList.Add(new Artifact("Artifact", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\flammePresentation.png")), "Artifact"));
            Inventory.InventoryList.Add(new Artifact("Artifact", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\glacePrésentation.png")), "Artifact"));

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
        }

        int accFace = 0;
        int accDos = 0;
        int accCoteGauche = 0;
        int accCoteDroit = 0;
        public override void Update()
        {
            this.Position += this.Velocity * 5;
            if (this.Velocity.Y == 1)
            {
                if (accFace == animeFace.Length)
                {
                    accFace = 0;
                }
                this.Rectangle.Fill = animeFace[accFace];
                accFace++;
            }
            if (this.Velocity.Y == -1)
            {
                if (accDos == animeDos.Length)
                {
                    accDos = 0;
                }
                this.Rectangle.Fill = animeDos[accDos];
                accDos++;
            }
            if (this.Velocity.X == 1)
            {
                if (accCoteGauche == animeCoteGauche.Length)
                {
                    accCoteGauche = 0;
                }
                this.Rectangle.Fill = animeCoteGauche[accCoteGauche];
                accCoteGauche++;
            }

        }

        public void Fire()
        {

        }

    }

	

}

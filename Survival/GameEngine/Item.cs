﻿using Survival.MalwareStuff;
using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Survival.GameEngine
{
    public class Item
    {
        private readonly string[] TYPE_NAME = new string[] { "Armor", "Ring", "Artifact" };

        private string name;
        public string Name { get => name; set => name = value; }

        private string description;
        public string Description { get => description; set => description = value; }

        private ImageBrush texture;
        public ImageBrush Texture { get => texture; set => texture = value; }

        private Rectangle rectangle;
        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }

        private string type;
        public string Type 
        { 
            get => type;
            set 
            {
                if (TYPE_NAME.Contains(value))
                {
                    type = value;
                } else
                {
                    throw new ArgumentException("Type not valid");
                }
            } 
        }


        private bool bcanDrag;
        public bool bCanDrag { get => bcanDrag; set => bcanDrag = value; }

        public Item(string name, string description, BitmapImage image, string type)
        {
            this.Name = name;
            this.Description = description;
            if (this.Texture == null)
            {
                this.Texture = new ImageBrush();
            }

            this.Texture.ImageSource = image;
            this.Rectangle = new Rectangle()
            {
                Height = 90,
                Width = 90,
                Fill = this.Texture,
                Stroke = Brushes.Yellow,
            };
            this.Rectangle.PreviewMouseDown += MD;
            this.Type = type;
            bcanDrag = true;
        }

        public void MD(object sender, MouseButtonEventArgs e)
        {
            if (bCanDrag)
            {
                ((MainWindow)Application.Current.MainWindow).preparationWindow.dragObject = this;
                ((MainWindow)Application.Current.MainWindow).preparationWindow.canvPW.CaptureMouse();
            }
        }
    }
}

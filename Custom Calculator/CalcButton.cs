﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CalcButtons
{
    public class CalcButton:Button
    {
        //FIELDS
        private int borderSize = 0;
        private int borderRadius = 50;
        private Color borderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));

        //PROPERTIES
        public int BorderSize
        {
            get =>borderSize;
            set { borderSize = value; Invalidate(); }
        }
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = (value <= Height) ? value : Height; Invalidate(); }
        }
        public Color BackgroundColor
        {
            get => BackColor;
            set { BackColor = value; }
        }
        public Color TextColor
        {
            get => ForeColor;
            set { ForeColor = value; }
        }
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        //CONSTRUCTOR
        public CalcButton()
        {
            Size size = new Size(200, 100);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.DodgerBlue;
            ForeColor = Color.White;
            Resize += new EventHandler(Button_Resize);
        }

        //METHOD
        private GraphicsPath GetFigurePath(RectangleF rectangle, float radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.StartFigure();
            graphicsPath.AddArc(rectangle.X, rectangle.Y, radius, radius, 180, 90);
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Y, radius, radius, 270, 90);
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Height - radius, radius, radius, 0, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Bottom - radius, radius, radius, 90, 90);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rectangleSurface = new RectangleF(0, 0, Width, Height);
            RectangleF rectangleBorder = new RectangleF(1, 1, Width - 0.5F, Height - 1);
            if (borderRadius > 1)
            {
                using (GraphicsPath graphicsPathSurface = GetFigurePath(rectangleSurface, borderRadius))
                using (GraphicsPath graphicsPathBorder = GetFigurePath(rectangleBorder, borderRadius-1F))
                using (Pen penSurface = new Pen(Parent.BackColor, 2))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    Region = new Region(graphicsPathSurface);
                    pevent.Graphics.DrawPath(penBorder, graphicsPathSurface);
                    if (borderSize >= 1)
                    {
                        pevent.Graphics.DrawPath(penBorder, graphicsPathBorder);
                    }
                }
            }
            else
            {
                Region = new Region(rectangleSurface);
                if (borderSize >= 1)
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                    }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }
        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                Invalidate();
        }
        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > Height)
                borderRadius = Height;
        }
    }
}
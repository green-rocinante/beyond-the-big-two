using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace BeyondtheBigTwo
{
    // This code is from the week 8 unit learning resources
    public delegate void ClickAction(Button b);
    public class Button
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string Caption { get; set; }

        public Button()
        {
            Width = 100;
            Height = 25;
        }

        public void Draw()
        {
            SplashKit.FillRectangle(Color.LightGray, X, Y, Width, Height);
            SplashKit.DrawText(Caption, Color.Black, X + 5, Y + 5);
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle() { X = X, Y = Y, Width = Width, Height = Height };
            }
        }

        public bool IsMouseHover
        {
            get
            {
                return SplashKit.PointInRectangle(SplashKit.MousePosition(), Rectangle);
            }
        }

        public event ClickAction OnClick;

        public void HandleInput()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton) && IsMouseHover)
            {
                if (OnClick != null)
                    OnClick?.Invoke(this);
            }
        }
    }
}
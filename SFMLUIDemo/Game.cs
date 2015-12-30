using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLUI;

namespace SFMLUIDemo
{
    public static class Game
    {
        private static readonly Vector2u DefaultWindowSize = new Vector2u(1600, 900);
        private const uint DISPLAY_RATE = 60;
        private const String GAME_NAME = "SFMLUI Demo";
        private static Stopwatch m_Timer { get; } = new Stopwatch();
        private static long m_LastTime { get; set; }

        private static UIBase m_UI { get; set; }
        private static Texture m_TestTexture { get; set; }
        private static IntRect m_NormalRect { get; set; }
        private static IntRect m_HoverRect { get; set; }
        private static IntRect m_ClickRect { get; set; }

        public static RenderWindow Window { get; private set; }

        public static void Main()
        {
            InitializeWindow();
            Start();
        }

        private static void Start()
        {
            m_Timer.Start();

            CreateTestTexture();

            var font = new Font("arial.ttf");

            m_UI = new UIBase();

            var icon = new UIIcon(m_TestTexture, m_NormalRect);
            var iconCaption = new UICaption("Icon", font, 16, Color.Green);
            iconCaption.CenterOn(icon);
            var button = new UIButton(m_TestTexture, m_NormalRect)
            {
                HoverRect = m_HoverRect,
                ClickRect = m_ClickRect
            };
            button.StackOnBottom(icon);
            var buttonCaption = new UICaption("Button", font, 16, Color.Green);
            buttonCaption.CenterOn(button);
            var textField = new UITextField(Window, new Vector2f(100, 20), Color.White, font, 16, Color.Green)
            {
                TextOffset = new Vector2f(0, -2)
            };
            textField.StackOnBottom(button);

            m_UI.Add(icon);
            m_UI.Add(iconCaption);
            m_UI.Add(button);
            m_UI.Add(buttonCaption);
            m_UI.Add(textField);

            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                Update(GetDeltaTime());

                Window.Clear(Color.Black);

                Render();

                Window.Display();
            }
        }

        private static void Update(float dt)
        {
            m_UI.Update();
        }

        private static void Render()
        {
            Window.Draw(m_UI);
        }

        private static void CreateTestTexture()
        {
            var size = new Vector2u(300, 100);
            var image = new Image(size.X, size.Y);
            const int borderSize = 10;
            
            for (uint j = 0; j < size.Y; j++)
            {
                for (uint i = 0; i < size.X / 3; i++)
                {
                    var color = Color.White;

                    if (i < borderSize || i > (size.X / 3) - borderSize
                        || j < borderSize || j > size.Y - borderSize)
                    {
                        color = new Color(200, 200, 200);
                    }
                    image.SetPixel(i, j, color);
                }
            }

            for (uint j = 0; j < size.Y; j++)
            {
                for (uint i = size.X / 3; i < size.X * 2 / 3; i++)
                {
                    var color = new Color(200, 200, 200);

                    if (i < (size.X / 3) + borderSize || i > (size.X * 2 / 3) - borderSize
                        || j < borderSize || j > size.Y - borderSize)
                    {
                        color = new Color(150, 150, 150);
                    }
                    image.SetPixel(i, j, color);
                }
            }

            for (uint j = 0; j < size.Y; j++)
            {
                for (uint i = size.X * 2 / 3; i < size.X; i++)
                {
                    var color = new Color(150, 150, 150);

                    if (i < (size.X * 2 / 3) + borderSize || i > size.X - borderSize
                        || j < borderSize || j > size.Y - borderSize)
                    {
                        color = new Color(100, 100, 100);
                    }
                    image.SetPixel(i, j, color);
                }
            }

            m_TestTexture = new Texture(image);
            m_NormalRect = new IntRect(0, 0, 100, 100);
            m_HoverRect = new IntRect(100, 0, 100, 100);
            m_ClickRect = new IntRect(200, 0, 100, 100);

        }

        private static float GetDeltaTime()
        {
            var elapsedMs = m_Timer.ElapsedMilliseconds - m_LastTime;
            m_LastTime = m_Timer.ElapsedMilliseconds;
            return (elapsedMs / 1000f);
        }

        private static void InitializeWindow()
        {
            Window = new RenderWindow(new VideoMode(DefaultWindowSize.X, DefaultWindowSize.Y, 32), GAME_NAME, Styles.Default);
            Window.SetFramerateLimit(DISPLAY_RATE);

            Window.Closed += OnWindowClose;
            Window.MouseMoved += OnMouseMove;
            Window.MouseButtonPressed += OnMouseClick;
            Window.MouseButtonReleased += OnMouseClick;
        }

        private static void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            m_UI.PostMouseClickEventToUI(new Vector2f(e.X, e.Y), e.Button);
        }

        private static void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            m_UI.PostMouseMoveToUI(new Vector2f(e.X, e.Y));
        }

        private static void OnWindowClose(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}

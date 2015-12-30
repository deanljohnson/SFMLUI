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
        private const int BORDER_SIZE = 10;

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

            var expandingFrame = new UIExpandingFrame(new Vector2f(100, 100), new Vector2f(10, 10), m_TestTexture, 
                new IntRect(0, 0, BORDER_SIZE, BORDER_SIZE), //corner 
                new IntRect(BORDER_SIZE, 0, m_NormalRect.Width - (BORDER_SIZE * 2), BORDER_SIZE), //side
                new IntRect(BORDER_SIZE, BORDER_SIZE, m_NormalRect.Width - (BORDER_SIZE * 2), m_NormalRect.Height - (BORDER_SIZE * 2)))//fill
            {
                Position = new Vector2f(10, 10)
            };

            var icon = new UIIcon(m_TestTexture, m_NormalRect) { Position = new Vector2f(0, 0)};
            var iconCaption = new UICaption("Icon", font, 16, Color.Green);
            iconCaption.CenterOn(icon);
            var button = new UIButton(m_TestTexture, m_NormalRect)
            {
                HoverRect = m_HoverRect,
                ClickRect = m_ClickRect
            };
            button.StackOnBottom(icon, new Vector2f(0, 5));
            button.CenterHorizontally(icon);
            var buttonCaption = new UICaption("Button", font, 16, Color.Green);
            buttonCaption.CenterOn(button);

            var captionedButton = new UICaptionedButton(m_TestTexture, m_NormalRect, "Captioned\nButton", font, 16, Color.Green)
            {
                HoverRect = m_HoverRect,
                ClickRect = m_ClickRect
            };
            captionedButton.StackOnBottom(button, new Vector2f(0, 5));
            captionedButton.CenterHorizontally(button);
            var textField = new UITextField(Window, new Vector2f(100, 20), new Color(240, 240, 240), font, 14, Color.Green)
            {
                TextOffset = new Vector2f(0, -1)
            };
            textField.StackOnBottom(captionedButton, new Vector2f(0, 5));
            textField.CenterHorizontally(captionedButton);

            expandingFrame.Add(icon);
            expandingFrame.Add(iconCaption);
            expandingFrame.Add(button);
            expandingFrame.Add(buttonCaption);
            expandingFrame.Add(captionedButton);
            expandingFrame.Add(textField);

            m_UI.Add(expandingFrame);

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
            
            for (uint j = 0; j < size.Y; j++)
            {
                for (uint i = 0; i < size.X / 3; i++)
                {
                    var color = Color.White;

                    if (i < BORDER_SIZE || i > (size.X / 3) - BORDER_SIZE
                        || j < BORDER_SIZE || j > size.Y - BORDER_SIZE)
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

                    if (i < (size.X / 3) + BORDER_SIZE || i > (size.X * 2 / 3) - BORDER_SIZE
                        || j < BORDER_SIZE || j > size.Y - BORDER_SIZE)
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

                    if (i < (size.X * 2 / 3) + BORDER_SIZE || i > size.X - BORDER_SIZE
                        || j < BORDER_SIZE || j > size.Y - BORDER_SIZE)
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

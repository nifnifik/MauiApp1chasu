namespace MauiApp1chasu
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ClockView.Drawable = new ClockDrawable();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                ClockView.Invalidate();
                return true;
            });
        }
    }

    public class ClockDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var now = DateTime.Now;
            float centerX = dirtyRect.Width / 2;
            float centerY = dirtyRect.Height / 2;
            float radius = Math.Min(centerX, centerY) - 10;

            // Циферблат
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            canvas.DrawCircle(centerX, centerY, radius);

            // Секундные деления
            DrawMinuteMarks(canvas, centerX, centerY, radius);

            // Цифры на циферблате
            DrawNumbers(canvas, centerX, centerY, radius);

            // Стрелки
            DrawHand(canvas, centerX, centerY, now.Hour % 12 * 30 + now.Minute * 0.5f, radius * 0.5f, 6, Colors.White); // часовая
            DrawHand(canvas, centerX, centerY, now.Minute * 6, radius * 0.7f, 4, Colors.White); // минутная
            DrawHand(canvas, centerX, centerY, now.Second * 6, radius * 0.9f, 2, Colors.Red); // секундная
        }

        private void DrawNumbers(ICanvas canvas, float centerX, float centerY, float radius)
        {
            canvas.FontColor = Colors.White;
            canvas.FontSize = 18; // Увеличил размер цифр

            for (int i = 1; i <= 12; i++)
            {
                float angle = (i * 30 - 90) * (float)Math.PI / 180; // -90 чтобы начать с 12 часов
                float numberRadius = radius - 25; // Увеличил отступ для больших цифр

                float x = centerX + numberRadius * (float)Math.Cos(angle);
                float y = centerY + numberRadius * (float)Math.Sin(angle);

                // Рисуем цифру
                string number = i.ToString();
                canvas.DrawString(number, x - 15, y - 12, 30, 24, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }

        private void DrawMinuteMarks(ICanvas canvas, float centerX, float centerY, float radius)
        {
            for (int i = 0; i < 60; i++)
            {
                float angle = i * 6 * (float)Math.PI / 180;

                // Длина деления - больше для каждых 5 минут, меньше для остальных
                float length = (i % 5 == 0) ? 8 : 4;
                float startRadius = radius - length;
                float endRadius = radius;

                float startX = centerX + startRadius * (float)Math.Sin(angle);
                float startY = centerY - startRadius * (float)Math.Cos(angle);
                float endX = centerX + endRadius * (float)Math.Sin(angle);
                float endY = centerY - endRadius * (float)Math.Cos(angle);

                canvas.StrokeColor = Colors.White;
                canvas.StrokeSize = (i % 5 == 0) ? 2 : 1; // Толще для каждых 5 минут
                canvas.DrawLine(startX, startY, endX, endY);
            }
        }

        private void DrawHand(ICanvas canvas, float cx, float cy, float angle, float length, float width, Color color)
        {
            float rad = angle * (float)Math.PI / 180;
            float endX = cx + length * (float)Math.Sin(rad);
            float endY = cy - length * (float)Math.Cos(rad);

            canvas.StrokeColor = color;
            canvas.StrokeSize = width;
            canvas.DrawLine(cx, cy, endX, endY);
        }
    }
}
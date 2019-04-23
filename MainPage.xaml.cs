using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=391641 dokumentiert.

namespace TouchTest
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const double size = 10;

        private IDictionary<uint, Rectangle> rects;
        private Random ran;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            rects = new Dictionary<uint, Rectangle>();
            ran = new Random();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);

            AddRect(e.GetCurrentPoint(grid));
        }

        private void AddRect(PointerPoint pp)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = GetRandomBrush();
            rect.Margin = GetMargin(pp.Position);
            rect.Width = rect.Height = GetSize();

            rects.Add(pp.PointerId, rect);
            grid.Children.Add(rect);
        }

        private Brush GetRandomBrush()
        {
            byte[] rgb = new byte[3];

            do
            {
                ran.NextBytes(rgb);
            } while (rgb.Select(b => (int)b).Sum() < 200);

            return new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2]));
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);

            Rectangle rect;
            if (!rects.TryGetValue(e.Pointer.PointerId, out rect)) return;

            rects.Remove(e.Pointer.PointerId);
            grid.Children.Remove(rect);
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);

            PointerPoint pp = e.GetCurrentPoint(grid);

            if (!rects.ContainsKey(e.Pointer.PointerId)) AddRect(pp);
            else rects[e.Pointer.PointerId].Margin = GetMargin(pp.Position);
        }

        private Thickness GetMargin(Point point)
        {
            double size = GetSize();
            return new Thickness(point.X - size / 2, point.Y - size / 2, -size, -size);
        }

        private double GetSize()
        {
            return ActualWidth / size;
        }
    }
}

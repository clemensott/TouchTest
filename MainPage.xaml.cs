using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
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

        private List<Tuple<Pointer, Rectangle>> pointers;
        private Random ran;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            pointers = new List<Tuple<Pointer, Rectangle>>();
            ran = new Random();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);

            AddRect(e);
        }

        private void AddRect(PointerRoutedEventArgs e)
        {
            var pp = e.GetCurrentPoint(this);

            Rectangle rect = new Rectangle();
            rect.Fill = GetRandomBrush();
            rect.Margin = GetMargin(pp.Position);
            rect.Width = rect.Height = GetSize();

            pointers.Add(new Tuple<Pointer, Rectangle>(e.Pointer, rect));
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

            int index = pointers.FindIndex(p => p.Item1.PointerId == e.Pointer.PointerId);

            if (index == -1) return;

            pointers.RemoveAt(index);
            grid.Children.RemoveAt(index);
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);

            int index = pointers.FindIndex(p => p.Item1.PointerId == e.Pointer.PointerId);

            if (index == -1) AddRect(e);
            else
            {
                var pp = e.GetCurrentPoint(this);
                pointers[index].Item2.Margin = GetMargin(pp.Position);
            }
        }

        private Thickness GetMargin(Point point)
        {
            return new Thickness(point.X - GetSize(), point.Y - GetSize(), 0, 0);
        }

        private double GetSize()
        {
            return ActualWidth / size;
        }
    }
}

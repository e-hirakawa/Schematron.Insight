using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Schematron.Validator.Utilities.Controls
{
    public class ToolItemButton:Button
    {
        public ToolItemButton() : base() { }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToolItemButton), new UIPropertyMetadata(null));

        public static readonly DependencyProperty IsLinkEnabledProperty =
            DependencyProperty.Register("IsLinkEnabled", typeof(bool), typeof(ToolItemButton), new UIPropertyMetadata(null));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public bool IsLinkEnabled
        {
            get { return (bool)GetValue(IsLinkEnabledProperty); }
            set { SetValue(IsLinkEnabledProperty, value); }
        }



    }
}

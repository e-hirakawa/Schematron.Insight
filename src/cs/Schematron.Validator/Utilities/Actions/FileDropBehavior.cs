using System;
using System.Windows;
using System.Windows.Input;

namespace Schematron.Validator.Utilities.Actions
{
    public class FileDropBehavior
    {
        static Type type_cmnd = typeof(ICommand);
        static Type type_self = typeof(FileDropBehavior);

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", type_cmnd, type_self, new PropertyMetadata(null, PropertyChangedCallback));

        public static ICommand GetCommand(DependencyObject obj) => (ICommand)obj?.GetValue(CommandProperty);
        public static void SetCommand(DependencyObject obj, ICommand value) => obj?.SetValue(CommandProperty, value);


        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = d as UIElement;
            if(element != null)
            {
                ICommand cmd = GetCommand(d);
                if(cmd != null)
                {
                    element.AllowDrop = true;
                    element.PreviewDragOver += Element_PreviewDragOver;
                    element.Drop += Element_Drop;
                }
                else
                {
                    element.AllowDrop = false;
                    element.PreviewDragOver -= Element_PreviewDragOver;
                    element.Drop -= Element_Drop;
                }
            }
        }

        private static void Element_Drop(object sender, DragEventArgs e)
        {
            var element = sender as UIElement;
            if (element != null)
            {
                var fileInfos = e.Data.GetData(DataFormats.FileDrop) as string[];
                var cmd = GetCommand(element);
                if (cmd?.CanExecute(null) ?? false)
                {
                    cmd.Execute(fileInfos);
                }
            }
        }

        private static void Element_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = (e.Data.GetData(DataFormats.FileDrop) != null) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }
    }
}

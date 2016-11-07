using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BobSquad.CustomClasses
{
    public class EqTypeControl : Button
    {
        private Image icon;
        private TextBlock typeName;

        public EqTypeControl(DataRow category)
        {
            Tag = category["Id"];
            StackPanel buttonStack = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };

            icon = new Image()
            {
                Source = new BitmapImage(new Uri(category["Icon"].ToString(), UriKind.Relative)),
                MaxHeight = 200,
                Margin = new Thickness(0, 15, 0, 0)
            };

            typeName = new TextBlock()
            {
                Text = category["Name"].ToString().ToUpper(),
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                FontSize = 15,
                FontWeight = FontWeights.DemiBold,
                Margin = new Thickness(0, 10, 0, 0)
            };

            buttonStack.Children.Add(icon);
            buttonStack.Children.Add(typeName);
            Content = buttonStack;
        }
    }
}

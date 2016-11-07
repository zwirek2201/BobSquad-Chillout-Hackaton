using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BobSquad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Uri _mainPage = new Uri("/MainPage.xaml", UriKind.Relative);
            MainFrame.Source = _mainPage;
        }


        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }


        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (FullMenu.Visibility == Visibility.Collapsed)
            {
                FullMenu.Visibility = Visibility.Visible;
                ShortMenu.Visibility = Visibility.Collapsed;
            }
            else if (ShortMenu.Visibility == Visibility.Collapsed)
            {
                ShortMenu.Visibility = Visibility.Visible;
                FullMenu.Visibility = Visibility.Collapsed;
            }
        }


        private void btnMenuCar_Click(object sender, RoutedEventArgs e)
        {
                    Uri _carPage = new Uri("/CarPage.xaml", UriKind.Relative);
                    MainFrame.Source = _carPage;
        }


        private void btnMenuRooms_Click(object sender, RoutedEventArgs e)
        {

                    Uri _roomPage = new Uri("/Room.xaml", UriKind.Relative);
                    MainFrame.Source = _roomPage;

        }

        private void btnMenuDevices_Click(object sender, RoutedEventArgs e)
        {

                    Uri _equipementPage = new Uri("/EquipmentCategories.xaml", UriKind.Relative);
                    MainFrame.Source = _equipementPage;

        }


        private void btnIconCars_MouseEnter(object sender, MouseEventArgs e)
        {
            btnIconCars.Margin = new Thickness(0);
        }

        private void btnIconCars_MouseLeave(object sender, MouseEventArgs e)
        {
            btnIconCars.Margin = new Thickness(6);
        }

        private void btnIconRooms_MouseEnter(object sender, MouseEventArgs e)
        {
            btnIconRooms.Margin = new Thickness(0);
        }

        private void btnIconRooms_MouseLeave(object sender, MouseEventArgs e)
        {
            btnIconRooms.Margin = new Thickness(6);
        }
       

        private void btnIconDevices_MouseEnter(object sender, MouseEventArgs e)
        {
            btnIconDevices.Margin = new Thickness(0);
        }

        private void btnIconDevices_MouseLeave(object sender, MouseEventArgs e)
        {
            btnIconDevices.Margin = new Thickness(6);
        }
    }
}
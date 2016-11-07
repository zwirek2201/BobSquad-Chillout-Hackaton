using System;
using System.Collections.Generic;
using System.Data;
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

namespace BobSquad.CustomClasses
{
    /// <summary>
    /// Interaction logic for FloorPage.xaml
    /// </summary>
    public partial class FloorPage : Page
    {
        private string floorId;
        public FloorPage(string floorId)
        {
            this.floorId = floorId;
            InitializeComponent();

            try
            {
                navigationStack.Orientation = Orientation.Horizontal;
                DataTable _floors = Services.DB.RunSelectCommand("select top 1 * from Floors where Id = @FloorId", new List<System.Data.SqlClient.SqlParameter>() { new System.Data.SqlClient.SqlParameter("FloorId", this.floorId) });
                foreach (DataRow floor in _floors.Rows)
                {
                    BitmapSource bSource = new BitmapImage(new Uri(@"Resources\" + floor["Layout"], UriKind.Relative));
                    LayoutCanvas.Background = new ImageBrush(bSource);

                    Label header = new Label()
                    {
                        FontSize = 20,
                        Foreground = new SolidColorBrush(Colors.Black),
                        Height = double.NaN,
                        Width = double.NaN,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    header.Content = "Piętro " + floor["Number"];

                    Button previonsButton = new Button()
                    {
                        Height = 40,
                        Width = 40,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = new SolidColorBrush(Colors.White),
                        Content = "<",
                        Margin = new Thickness(10, 5, 5, 5)
                    };

                    if (floorId == "1")
                        previonsButton.IsEnabled = false;

                    Button nextButton = new Button()
                    {
                        Height = 40,
                        Width = 40,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = new SolidColorBrush(Colors.White),
                        Content = ">",
                        Margin = new Thickness(5)
                    };

                    if (floorId == "2")
                        nextButton.IsEnabled = false;

                    previonsButton.Click += PrevionsButton_Click;
                    nextButton.Click += NextButton_Click;

                    navigationStack.Children.Add(header);
                    navigationStack.Children.Add(previonsButton);
                    navigationStack.Children.Add(nextButton);
                }

                DataTable _rooms = Services.DB.RunSelectCommand("select * from Rooms where Floor = @FloorId", new List<System.Data.SqlClient.SqlParameter>() { new System.Data.SqlClient.SqlParameter("FloorId", this.floorId) });

                foreach (DataRow _room in _rooms.Rows)
                {
                    CustomClasses.RoomControl room = new CustomClasses.RoomControl(new System.Drawing.Point(Convert.ToInt32(_room["LocalizationX"]), Convert.ToInt32(_room["LocalizationY"])), new System.Drawing.Size(Convert.ToInt32(_room["SizeX"]), Convert.ToInt32(_room["SizeY"])), _room["Id"].ToString());
                    room.Width = Convert.ToInt32(_room["SizeX"]);
                    room.Height = Convert.ToInt32(_room["SizeY"]);

                    Canvas.SetLeft(room, Convert.ToInt32(_room["LocalizationX"]));
                    Canvas.SetTop(room, Convert.ToInt32(_room["LocalizationY"]));

                    LayoutCanvas.Children.Add(room);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new FloorPage((Convert.ToInt32(floorId) + 1).ToString()));
        }

        private void PrevionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new FloorPage((Convert.ToInt32(floorId) - 1).ToString()));
        }
    }
}

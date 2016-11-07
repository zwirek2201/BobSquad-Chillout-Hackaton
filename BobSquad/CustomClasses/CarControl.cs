using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BobSquad.CustomClasses
{
    class CarControl : ToggleButton
    {
        private Image image;
        private SmallScheduleControl schedule;
        private Car _car;
        private bool rotated;
        private StackPanel mainPanel;
        private Label lab;
        private Label name;

        public CarControl(Car car, bool rotated = false)
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
            _car = car;
            image = new Image();
            image.Source = new BitmapImage(new Uri(@"Images\" + "car.png", UriKind.Relative));
            Margin = new Thickness(20, 20, 20, 20);
            BorderThickness = new Thickness(0);
            Content = image;
            Width = double.NaN;
            Height = double.NaN;


            this.rotated = rotated;
            if (rotated)
            {
                TransformGroup group = new TransformGroup();
                RotateTransform trans1 = new RotateTransform(180);
                group.Children.Add(trans1);
                TranslateTransform trans2 = new TranslateTransform(100,230);
                group.Children.Add(trans2);
                image.LayoutTransform= group;
            }
            

            SqlParameter carParam = new SqlParameter("CarId", car.Id.ToString());
            DataTable rentals = Services.DB.RunSelectCommand("select * from CarRentals where Car = @CarId",

            new List<SqlParameter>() {carParam});

            DataTable rentalsToday = rentals.Copy();
            rentalsToday.Rows.Clear();

            foreach (DataRow row in rentals.Rows)
            {
                if (Convert.ToDateTime(row["StartTime"]).Date == DateTime.Today.Date &&
                    Convert.ToDateTime(row["StartTime"]).Hour >= DateTime.Today.Hour)
                {
                    rentalsToday.Rows.Add(row.ItemArray);
                }
            }

            List<int> hours = new List<int>();

            foreach (DataRow row in rentalsToday.Rows)
            {
                DateTime startDate = Convert.ToDateTime(row["StartTime"]);
                DateTime endDate = Convert.ToDateTime(row["EndTime"]);

                for (int i = startDate.Hour; i < endDate.Hour; i++)
                {
                    if (!hours.Contains(i))
                        hours.Add(i);
                }
            }

            schedule = new SmallScheduleControl(car.Id.ToString(), hours, "CarRentals");
            schedule.Margin = new Thickness(0);
            schedule.PlacementTarget = this;
            schedule.Placement = PlacementMode.Bottom;
            schedule.StaysOpen = true;
            schedule.MouseLeave += CarControl_MouseLeave;

            mainPanel = new StackPanel()
            {
                Width = double.NaN,
                Height = double.NaN,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent),
                Opacity = 0.5
            };
            lab = new Label();
            
            name = new Label();
            name.Content = $"{_car.NickName}";
            name.FontSize = 18;
            name.FontWeight = FontWeights.Bold;
            lab.Content = $"{_car.Brand}\n{_car.Type.ToString()}\n{_car.Fuel.ToString()}\n" +
                         $"{_car.PassengerCount} miejsc\nSkrzynia {_car.GearBox}\n" +
                         $"Numer rej : {_car.RegistrationNumber}";
            mainPanel.Children.Add(name);
            mainPanel.Children.Add(lab);


            this.MouseEnter += CarControl_MouseEnter;
            this.MouseLeave += CarControl_MouseLeave;
        }

        private void CarControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!schedule.IsMouseOver)
            {
                schedule.IsOpen = false;
                this.IsChecked = false;
                this.Content = image;
            }
        }

        private void CarControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            schedule.IsOpen = true;
            this.IsChecked = true;
            this.Content = mainPanel;
        }
    }
    
}

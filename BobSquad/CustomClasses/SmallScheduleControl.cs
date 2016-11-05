using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BobSquad.CustomClasses
{
    class SmallScheduleControl : Popup
    {
        private int startHour = 6;
        private int endHour = 16;
        private string id;

        private StackPanel innerStack;
        private StackPanel mainStack;

        private List<int> blockedHours;
        private List<int> chosenHours;

        private Button addButton;
        private List<Button> scheduleButtons;
        private string updatedTable;
        
        public SmallScheduleControl(string Id, List<int> blockedHours, string updatedtable)
        {
            try
            {
                scheduleButtons = new List<Button>();
                this.updatedTable = updatedtable;
                this.id = Id;
                chosenHours = new List<int>();

                this.blockedHours = blockedHours;
                this.AllowsTransparency = true;


                mainStack = new StackPanel();
                mainStack.Orientation = Orientation.Vertical;
                mainStack.Margin = new System.Windows.Thickness(5, 0, 5, 5);

                Label header = new Label()
                {
                    Content = "Dostępność dzisiaj",
                    Height = double.NaN,
                    Width = double.NaN,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    Background = new SolidColorBrush(Colors.White)
                };

                innerStack = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Width = double.NaN,
                    Height = 60,
                    Background = new SolidColorBrush(Colors.White),
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                };

                for (int i = startHour; i < endHour; i++)
                {
                    Button scheduleHour = new Button()
                    {
                        Height = double.NaN,
                        Width = 30,
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Background = new SolidColorBrush(Colors.LightGreen),
                        BorderThickness = new System.Windows.Thickness(0.5),
                        BorderBrush = new SolidColorBrush(Colors.Gray),
                        Opacity = 0.5,
                        Tag = i.ToString()
                    };

                    if (blockedHours.Contains(i) || i < DateTime.Now.Hour)
                    {
                        scheduleHour.IsEnabled = false;
                        scheduleHour.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        scheduleHour.Click += ScheduleHour_Click;
                    }

                    scheduleButtons.Add(scheduleHour);

                    StackPanel hourStack = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                    };

                    Label hourLabel = new Label()
                    {
                        Width = double.NaN,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        Content = i,
                    };

                    hourStack.Children.Add(hourLabel);
                    scheduleHour.Content = hourStack;
                    innerStack.Children.Add(scheduleHour);
                }

                Button labelbutton = new Button()
                {
                    Height = double.NaN,
                    Width = double.NaN,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    Background = new SolidColorBrush(Colors.White),
                    BorderThickness = new System.Windows.Thickness(0)
                };

                labelbutton.Click += Labelbutton_Click;

                Label label = new Label()
                {
                    Content = "Więcej",
                    Height = double.NaN,
                    Width = double.NaN,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    Background = new SolidColorBrush(Colors.Transparent)
                };

                labelbutton.Content = label;

                mainStack.Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 320,
                    ShadowDepth = 0,
                    Opacity = 0.4,
                    BlurRadius = 8
                };

                mainStack.Children.Add(header);
                mainStack.Children.Add(innerStack);
                mainStack.Children.Add(labelbutton);
                this.Child = mainStack;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void Labelbutton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LargeScheduleControl large = new LargeScheduleControl(this.id, this.updatedTable);
        }

        private void ScheduleHour_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button send = (Button)sender;
            if (chosenHours.Contains(Convert.ToInt32(send.Tag)))
            {
                chosenHours.Remove(Convert.ToInt32(send.Tag));
                send.Background = new SolidColorBrush(Colors.White);
                if (chosenHours.Count == 0)
                {
                    mainStack.Children.Remove(addButton);
                }
            }
            else
            {
                if (chosenHours.Count == 0)
                {
                    if (addButton == null)
                    {
                        addButton = new Button()
                        {
                            Height = 25,
                            Width = double.NaN,
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                            VerticalAlignment = System.Windows.VerticalAlignment.Top,
                            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                            Background = new SolidColorBrush(Colors.Green),
                            BorderThickness = new System.Windows.Thickness(0),
                            Content = "Rezerwuj!",
                            VerticalContentAlignment = System.Windows.VerticalAlignment.Center
                        };
                        addButton.Click += AddButton_Click;
                    }
                    mainStack.Children.Insert(2, addButton);

                }
                chosenHours.Add(Convert.ToInt32(send.Tag));
                send.Background = new SolidColorBrush(Colors.SkyBlue);
            }
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button send = (Button)sender;
            chosenHours.Sort();
            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, chosenHours.First(), 0, 0);
            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, chosenHours.Last() + 1, 0, 0);

            string field = "";
            switch (updatedTable)
            {
                case "RoomRentals":
                    field = "Room";
                    break;

                case "CarRentals":
                    field = "Car";
                    break;

                case "EquipmentRentals":
                    field = "Equipment";
                    break;
            }

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@User", 1),
                new SqlParameter("@" + field, this.id),
                new SqlParameter("@StartTime", startTime.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("@endTime", endTime.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            string command = "insert into " + updatedTable +
                             " ([User]," + field + ", StartTime, EndTime) values (@User, @" + field + ", @StartTime, @EndTime)";
            Services.DB.RunCommand(command, parameters);
            send.Content = "Zarezerwowano!";

            foreach (Button hourButton in scheduleButtons)
            {
                if (chosenHours.Contains(Convert.ToInt32(hourButton.Tag)))
                {
                    hourButton.IsEnabled = false;
                }
            }
        }
    }
}

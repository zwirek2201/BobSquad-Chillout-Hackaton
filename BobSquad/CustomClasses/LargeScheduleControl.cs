using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BobSquad.CustomClasses
{
    class LargeScheduleControl : Popup
    {
        private StackPanel innerStack;
        private List<SmallSeparatedScheduleControl> schedules;
        private string id;
        private string updatedTable;
        public LargeScheduleControl(string id, string updatedTable)
        {
            schedules = new List<SmallSeparatedScheduleControl>();
            this.updatedTable = updatedTable;
            this.id = id;
            Placement = PlacementMode.Absolute;
            Width = 1000;
            HorizontalOffset = (1366 / 2) - (1000 / 2);
            VerticalOffset = (768 / 2) - (600 / 2);
            Height = 600;
            IsOpen = true;
            AllowsTransparency = true;
            StaysOpen = false;

            innerStack = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Width = double.NaN,
                Height = double.NaN,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.White),
                Margin = new System.Windows.Thickness(10)
            };

            innerStack.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                Direction = 320,
                ShadowDepth = 0,
                Opacity = 0.4,
                BlurRadius = 8
            };

            string command = "";
            string sentence = "";

            switch (updatedTable)
            {
                case "RoomRentals":
                    command = "select Number as Field from Rooms where Id = @ObjectId";
                    sentence = "Dostępność pokoju ";
                    break;

                case "CarRentals":
                    command = "select Model as Field from Cars where Id = @ObjectId";
                    sentence = "Dostępność samochodu ";
                    break;

                case "EquipmentRentals":
                    command = "select Name as Field from Equipments where Id = @ObjectId";
                    sentence = "Dostępność urządzenia ";
                    break;
            }
            SqlParameter objectId = new SqlParameter("objectId", this.id);
            DataTable table = Services.DB.RunSelectCommand(command,
                new List<SqlParameter>() { objectId });

            string field = table.Rows[0]["Field"].ToString();
            Label header = new Label()
            {
                Width = double.NaN,
                Height = 50,
                Background = new SolidColorBrush(Colors.White),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Content = sentence + field,
                FontSize = 25,
                VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center
            };

            innerStack.Children.Add(header);

            DateTime today = DateTime.Today;

            for (int i = 1; i <= 7; i++)
            {
                DateTime day = today.AddDays(i);
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    StackPanel schedulePanel = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Width = 500,
                        Height = 60,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        Background = new SolidColorBrush(Colors.White),
                        Margin = new System.Windows.Thickness(10,1,10,1)
                    };

                    string dayOfWeek = "";

                    switch (day.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            dayOfWeek = "Poniedziałek";
                            break;
                        case DayOfWeek.Tuesday:
                            dayOfWeek = "Wtorek";
                            break;
                        case DayOfWeek.Wednesday:
                            dayOfWeek = "Środa";
                            break;
                        case DayOfWeek.Thursday:
                            dayOfWeek = "Czwartek";
                            break;
                        case DayOfWeek.Friday:
                            dayOfWeek = "Piątek";
                            break;
                    }

                    TextBlock dayLabel = new TextBlock()
                    {
                        Text = day.ToString("yyyy-MM-dd") + "\n" + dayOfWeek,
                        Width = 100,
                        Height = 60,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        TextAlignment = System.Windows.TextAlignment.Left,
                        Background = new SolidColorBrush(Colors.White),
                        Margin = new System.Windows.Thickness(10, 0, 0, 0)
                    };
                    schedulePanel.Children.Add(dayLabel);

                    string field2 = "";
                    switch (updatedTable)
                    {
                        case "RoomRentals":
                            field2 = "Room";
                            break;

                        case "CarRentals":
                            field2 = "Car";
                            break;

                        case "EquipmentRentals":
                            field2 = "Equipment";
                            break;
                    }

                    SqlParameter Param = new SqlParameter("ObjectId", this.id);
                    DataTable rentals = Services.DB.RunSelectCommand("select * from " + updatedTable + " where " + field2 + " = @ObjectId",
                        new List<SqlParameter>() { Param });

                    DataTable rentalsToday = rentals.Copy();
                    rentalsToday.Rows.Clear();

                    foreach (DataRow row in rentals.Rows)
                    {
                        if (Convert.ToDateTime(row["StartTime"]).Date == day.Date)
                        {
                            rentalsToday.Rows.Add(row.ItemArray);
                        }
                    }

                    List<int> hours = new List<int>();


                    foreach (DataRow row in rentalsToday.Rows)
                    {
                        DateTime startDate = Convert.ToDateTime(row["StartTime"]);
                        DateTime endDate = Convert.ToDateTime(row["EndTime"]);

                        for (int j = startDate.Hour; j <= endDate.Hour; j++)
                        {
                            if (!hours.Contains(j))
                                hours.Add(j);
                        }
                    }

                    SmallSeparatedScheduleControl schedule = new SmallSeparatedScheduleControl(this.id, day, hours);
                    schedule.Width = 300;
                    schedule.Height = 60;
                    schedulePanel.Children.Add(schedule);
                    schedules.Add(schedule);
                    innerStack.Children.Add(schedulePanel);
                }
            }

            Button reserveButton = new Button()
            {
                Width = double.NaN,
                Height = 30,
                Background = new SolidColorBrush(Colors.Green),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Content = "Zarezerwuj!",
                Margin = new System.Windows.Thickness(0,10,0,0)
            };

            reserveButton.Click += ReserveButton_Click;
            innerStack.Children.Add(reserveButton);

           Child = innerStack;
        }

        private void ReserveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (SmallSeparatedScheduleControl schedule in schedules)
            {
                List<int> chosenHours = schedule.Hours;
                if (chosenHours.Count > 0)
                {
                    chosenHours.Sort();
                    DateTime startTime = new DateTime(schedule.Day.Year, schedule.Day.Month, schedule.Day.Day,
                        chosenHours.First(), 0, 0);
                    DateTime endTime = new DateTime(schedule.Day.Year, schedule.Day.Month, schedule.Day.Day,
                        chosenHours.Last(), 0, 0);

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
                                     " ([User]," + field + ", StartTime, EndTime) values (@User, @" + field +
                                     ", @StartTime, @EndTime)";
                    Services.DB.RunCommand(command, parameters);

                    foreach (Button hourButton in schedule.Buttons)
                    {
                        if (chosenHours.Contains(Convert.ToInt32(hourButton.Tag)))
                        {
                            hourButton.IsEnabled = false;
                        }
                    }
                    Button send = (Button) sender;
                    send.Content = "Zarezerwowano!";
                }
            }
        }
    }
}

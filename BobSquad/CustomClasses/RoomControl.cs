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
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BobSquad.CustomClasses
{
    class RoomControl : ToggleButton
    {
    private Point localization;
    private Size size;
    private string roomId;
    private CustomClasses.SmallScheduleControl schedule;
        private StackPanel mainPanel;

    public string Id
    {
        get { return roomId; }
    }

        public RoomControl(Point localization, Size size, string id)
        {
            this.roomId = id;

            SqlParameter roomId = new SqlParameter("RoomId", this.roomId);
            DataTable table =
                Services.DB.RunSelectCommand(
                    "select Rooms.Number, REquipments.Name from Rooms left join RoomEquipments on Rooms.Id = RoomEquipments.Room left join REquipments on REquipments.Id = RoomEquipments.Equipment where Rooms.Id = @RoomId", new List<SqlParameter>() {roomId});


            string name = table.Rows[0]["Number"].ToString();
            try
            {
                mainPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Background = new SolidColorBrush(Colors.Transparent)
                };


                Label header = new Label()
                {
                    Margin = new System.Windows.Thickness(0, 10, 0, 0),
                    Background = new SolidColorBrush(Colors.Transparent),
                    Content = name,
                    FontSize = 24,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontWeight = FontWeights.Bold
                };

                Label lab2 = new Label()
                {
                    Margin = new System.Windows.Thickness(0, 5, 0, 0),
                    Background = new SolidColorBrush(Colors.Transparent),
                    Content = "Wyposażenie:",
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontSize = 15,
                    Foreground = new SolidColorBrush(Colors.Black)
                };

                TextBlock equipment = new TextBlock()
                {
                    Margin = new System.Windows.Thickness(0, 3, 0, 0),                 
                    Background = new SolidColorBrush(Colors.Transparent),
                    FontSize = 12,
                    Text = "",
                    Foreground = new SolidColorBrush(Colors.Black),
                    TextAlignment = TextAlignment.Center
                };

                foreach (DataRow row in table.Rows)
                {
                    equipment.Text += row["Name"] + "\n";
                }

                if (equipment.Text == "\n")
                    equipment.Text = "brak";


                mainPanel.Children.Add(header);
                mainPanel.Children.Add(lab2);
                mainPanel.Children.Add(equipment);

                this.Content = mainPanel;
                this.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;

                mainPanel.Visibility = System.Windows.Visibility.Visible;

                this.localization = localization;
                this.size = size;
                Background = new SolidColorBrush(Colors.LightGray);
                Opacity = 0.8;

                SqlParameter roomParam = new SqlParameter("RoomId", this.roomId);
                DataTable rentals = Services.DB.RunSelectCommand("select * from RoomRentals where Room = @RoomId",
                    new List<SqlParameter>() {roomParam});

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

                schedule = new SmallScheduleControl(this.roomId, hours, "RoomRentals");
                schedule.Margin = new System.Windows.Thickness(0);
                schedule.PlacementTarget = this;
                schedule.Placement = PlacementMode.Bottom;
                schedule.StaysOpen = true;
                schedule.MouseLeave += RoomControl_MouseLeave;

                this.MouseEnter += RoomControl_MouseEnter;
                this.MouseLeave += RoomControl_MouseLeave;

                this.Content = mainPanel;

            }
            catch (Exception ex)
            {
                
            }
        }


        private void RoomControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!schedule.IsMouseOver)
            {
                schedule.IsOpen = false;
                this.IsChecked = false;
            }
        }

        private void RoomControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            schedule.IsOpen = true;
            this.IsChecked = true;
        }
    }
}

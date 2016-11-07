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

namespace BobSquad.CustomClasses
{
    class EquipmentRowControl : StackPanel
    {
        private SmallScheduleControl schedule;


        public EquipmentRowControl(Equipment equipment)
        {
            SetCustomProperties();

            SqlParameter equipmentParam = new SqlParameter("EquipmentId", equipment.EqId);
            DataTable rentals = Services.DB.RunSelectCommand("select * from EquipmentRentals where Equipment = @EquipmentId",
                new List<SqlParameter>() { equipmentParam });

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

            CreateSchedule(equipment, hours);
            this.MouseEnter += RoomControl_MouseEnter;
            this.MouseLeave += RoomControl_MouseLeave;

            CreateLabels(equipment);

        }

        private void RoomControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!schedule.IsMouseOver)
            {
                schedule.IsOpen = false;
                //this.IsChecked = false;
            }
        }

        private void RoomControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            schedule.IsOpen = true;
            //this.IsChecked = true;
        }

        private void SetCustomProperties()
        {
            Orientation = Orientation.Horizontal;
            Height = 60;
            Width = double.NaN;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Top;
            Background = new SolidColorBrush(Colors.LightGray);
            Margin = new Thickness(5, 1, 5, 1);
        }

        private void CreateLabels(Equipment equipment)
        {
            Label labName = new Label()
            {
                Content = equipment.EqName,
                Width = double.NaN,
                Height = double.NaN,
                FontSize = 18,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            Label labDesc = new Label()
            {
                Margin = new Thickness(10, 0, 0, 0),
                Content = equipment.EqDescription,
                Width = double.NaN,
                Height = double.NaN,
                FontSize = 18,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            this.Children.Add(labName);
            this.Children.Add(labDesc);
        }

        private void CreateSchedule(Equipment equipment, List<int> hours)
        {
            schedule = new SmallScheduleControl(equipment.EqId.ToString(), hours, "EquipmentRentals");
            schedule.Margin = new Thickness(0);
            schedule.PlacementTarget = this;
            schedule.Placement = PlacementMode.Bottom;
            schedule.StaysOpen = true;
            schedule.MouseLeave += RoomControl_MouseLeave;
            this.Children.Add(schedule);

        }
    }
}

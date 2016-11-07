using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace BobSquad.CustomClasses
{
    class SmallSeparatedScheduleControl : StackPanel
    {
        private int startHour = 6;
        private int endHour = 16;
        private string id;

        private StackPanel innerStack;
        private StackPanel mainStack;
        private DateTime day;

        private List<int> blockedHours;
        private List<int> chosenHours;

        private Button addButton;
        private List<Button> scheduleButtons;
        private string updatedTable;

        public List<int> Hours
        {
            get { return chosenHours; }
        }

        public List<Button> Buttons
        {
            get { return scheduleButtons; }
        }

        public DateTime Day
        {
            get { return day; }
        }

        public SmallSeparatedScheduleControl(string id, DateTime day, List<int> blockedHours)
        {
            chosenHours = new List<int>();
            this.blockedHours = blockedHours;
            this.id = id;
            this.day = day;
            Width = 300;
            Height = 60;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            scheduleButtons = new List<Button>();

            innerStack = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                    Width = 300,
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

                if (blockedHours.Contains(i) || (day.Date == DateTime.Now.Date && i < DateTime.Now.Hour))
                {
                    scheduleHour.IsEnabled = false;
                    scheduleHour.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    scheduleHour.Click += ScheduleHour_Click;
                }

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
                scheduleButtons.Add(scheduleHour);
                innerStack.Children.Add(scheduleHour);
            }
            this.Children.Add(innerStack);
        }

        private void ScheduleHour_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button send = (Button)sender;
            if (chosenHours.Contains(Convert.ToInt32(send.Tag)))
            {
                chosenHours.Remove(Convert.ToInt32(send.Tag));
                send.Background = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                chosenHours.Add(Convert.ToInt32(send.Tag));
                send.Background = new SolidColorBrush(Colors.SkyBlue);
            }
        }
    }
}

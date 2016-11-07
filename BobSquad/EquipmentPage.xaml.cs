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
using BobSquad.CustomClasses;
using BobSquad.Services;

namespace BobSquad
{
    /// <summary>
    /// Interaction logic for EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        public EquipmentPage(int eqTypeId)
        {
            InitializeComponent();
            DataTable equipment = DB.RunSelectCommand($"select * from Equipments where Type = {eqTypeId}");

            Label labHeader = new Label()
            {
                Content = DB.RunSelectCommand($"select Name from EquipmentTypes where Id = {eqTypeId}").Rows[0][0].ToString(),               
                FontSize = 20,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = new SolidColorBrush(Colors.Green),
            };

            mainStack.Children.Add(labHeader);
            foreach (DataRow equipmentRow in equipment.Rows)
            {
                mainStack.Children.Add(new EquipmentRowControl(new Equipment(equipmentRow)));
            }
        }
    }
}

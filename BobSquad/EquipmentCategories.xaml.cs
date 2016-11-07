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
    /// Interaction logic for Equipement.xaml
    /// </summary>
    public partial class EquipmentCategories : Page
    {
        private readonly DataTable _equipmentCategoriesTable;
        public EquipmentCategories()
        {
            InitializeComponent();
            _equipmentCategoriesTable = DB.RunSelectCommand("select * from EquipmentTypes");

        }

        private void btnType_Click(object sender, RoutedEventArgs e)
        {
            Button selectedType = (Button)sender;
            int typeId = int.Parse(selectedType.Tag.ToString());
            this.NavigationService?.Navigate(new EquipmentPage(typeId));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int rowIndex = 0;
            int colIndex = 0;
            int count = 0;
            foreach (DataRow category in _equipmentCategoriesTable.Rows)
            {
                count++;
                if (count%3 == 0 && count != _equipmentCategoriesTable.Rows.Count && rowIndex >= 1)
                {
                    RowDefinition additionalRow = new RowDefinition();
                    additionalRow.Height = new GridLength(1, GridUnitType.Star);
                    mainGrid.RowDefinitions.Add(additionalRow);
                }
                CreateTypeButtons(category, rowIndex, colIndex);

                if (colIndex == mainGrid.ColumnDefinitions.Count - 1)
                {
                    colIndex = 0;
                    rowIndex++;
                }
                else if (colIndex < 2)
                {
                    colIndex++;
                }
            }
        }

        private void CreateTypeButtons(DataRow category, int rowIndex, int colIndex)
        {
            EqTypeControl btnType = new EqTypeControl(category);
            btnType.Click += btnType_Click;

            Grid.SetRow(btnType, rowIndex);
            Grid.SetColumn(btnType, colIndex);

            mainGrid.Children.Add(btnType);
        }
    }
}

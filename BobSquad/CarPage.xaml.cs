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
    /// Interaction logic for CarPage.xaml
    /// </summary>
    public partial class CarPage : Page
    {
        public CarPage()
        {
            InitializeComponent();

            int _columnIndex = 0;
            int _rowIndex = 0;
            int _countItemsDataTableCars = 0;
            Boolean _rotateButtonCarControl = false;

            DataTable _dataTableCars = new DataTable();
            _dataTableCars = DB.RunSelectCommand("select * from Cars join CarTypes on Cars.Type=CarTypes.Id join" +
                                      " FuelTypes on Cars.FuelType=FuelTypes.Id");

            foreach (DataRow _row in _dataTableCars.Rows)
            {
                Car car = new Car (
                   Convert.ToInt32(_row[0]),
                   _row[1].ToString(),
                   Convert.ToInt32(_row[3]),
                   _row[7].ToString(),
                   _row[12].ToString(),
                   _row[9].ToString(),
                   _row[6].ToString(),
                   _row[2].ToString());
                CarControl carButtonControll = new CarControl(car, _rotateButtonCarControl);
                
                Grid.SetRow(carButtonControll, _rowIndex);
                Grid.SetColumn(carButtonControll, _columnIndex);
                CarGrid.Children.Add(carButtonControll);
                _columnIndex += 2;
                _countItemsDataTableCars += 1;
                if (_countItemsDataTableCars % 5 == 0)
                {
                    _rowIndex += 4;
                    _columnIndex = 0;
                    _rotateButtonCarControl = !_rotateButtonCarControl;
                }
            }
        }
    }
}

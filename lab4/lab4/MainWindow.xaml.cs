using System;
using System.Collections.Generic;
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
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static buildingCompanyEntities BCContext;

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            BCContext = new buildingCompanyEntities();
            var contractQuery = from c in BCContext.Contracts
                                  orderby c.Number
                                  select c;
            try
            {
                // Bind the ComboBox control to the query, 
                // which is executed during data binding.
                // To prevent the query from being executed multiple times during binding, 
                // it is recommended to bind controls to the result of the Execute method. 
                this.departmentList.DisplayMemberPath = "Number";
                this.departmentList.ItemsSource = ((ObjectQuery)contractQuery).Execute(MergeOption.AppendOnly);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

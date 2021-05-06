using System.Data;
using System.Windows;

namespace StoUslugClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Hide();
            //Auth form
            //AuthForm auth = new AuthForm();
            //auth.ShowDialog();
            //if (auth.DialogResult == true)
            //{
            //    Show();
            //}
            //else
            //{
            //    Close();
            //}
            Show();
        }

        private void DataGrid_SelectionChanged()
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable("test");
            dataTable.Columns.Add(new DataColumn()
            {
                ColumnName = "col1",
                DataType = typeof(string)
            });
            dataTable.Columns.Add(new DataColumn()
            {
                ColumnName = "col2",
                DataType = typeof(string)
            });
            var row1 = dataTable.NewRow();
            row1.ItemArray = new object[] { "test1", "test2" };
            var row2 = dataTable.NewRow();
            row2.ItemArray = new object[] { "test3", "test4" };
            dataTable.Rows.Add(row1);
            dataTable.Rows.Add(row2);
            MainTable.DataContext = dataTable;
        }
    }
}

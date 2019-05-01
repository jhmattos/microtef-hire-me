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

namespace Stone.WPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        private void ButtonTransaction_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new Uri("TransactionPage.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ButtonTransactionList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new Uri("TransactionList.xaml", UriKind.RelativeOrAbsolute);

        }
        private void CadastroCliente_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new Uri("ClientList.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}

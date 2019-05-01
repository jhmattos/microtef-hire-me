using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    /// Interação lógica para TransactionList.xam
    /// </summary>
    public partial class TransactionList : Page
    {
        HttpClient client = new HttpClient();
        public TransactionList()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://localhost:54602");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Loaded += TransactionList_Loaded;
        }

        async void TransactionList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Transaction/Get");
                response.EnsureSuccessStatusCode(); // Lança um código de erro
                string json  = await response.Content.ReadAsStringAsync();
                var transacoes = JsonConvert.DeserializeObject<List<TransactionDTO>>(json);
                gridTransacao.ItemsSource = transacoes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void GridTransacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

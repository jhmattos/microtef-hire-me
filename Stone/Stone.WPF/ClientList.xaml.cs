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
    /// Interação lógica para ClientList.xam
    /// </summary>
    public partial class ClientList : Page
    {
        HttpClient client = new HttpClient();
        public ClientList()
        {
            InitializeComponent();

            client.BaseAddress = new Uri("http://localhost:54602");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AtualizarGrid();
        }

        private void NovoCliente_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(null);
            client.Title = "Novo Cliente";
            bool? result = client.ShowDialog();
            if (result.HasValue && result.Value)
            {
                AtualizarGrid();
            }
        }

        async void AtualizarGrid()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/client/");
                response.EnsureSuccessStatusCode(); // Lança um código de erro
                string json = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<ClienteDTO>>(json);
                gridCliente.ItemsSource = clientes.Select(c => new { c.NumeroCartao, c.Nome });
                if (clientes.Count > 0) gridCliente.Columns.RemoveAt(5);
                if (clientes.Count > 0) gridCliente.Columns.RemoveAt(4);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            string numeroCartao = ((Button)sender).Tag.ToString();
            if (!string.IsNullOrEmpty(numeroCartao))
            {
                Client client = new Client(numeroCartao);
                client.Title = "Novo Cliente";
                bool? result = client.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    AtualizarGrid();
                }
            }
        }


        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            string numeroCartao = ((Button)sender).Tag.ToString();
            if (!string.IsNullOrEmpty(numeroCartao) && MessageBox.Show("Deseja excluir o cliente", "Excluir Cliente", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                HttpResponseMessage response = await client.DeleteAsync(string.Format("api/client/{0}", numeroCartao));
                response.EnsureSuccessStatusCode(); // Lança um código de erro
                string json = await response.Content.ReadAsStringAsync();
                var retornoTransacao = JsonConvert.DeserializeObject<RetornoTransacaoDTO>(json);
                if (retornoTransacao.Codigo == 1)
                {
                    AtualizarGrid();
                }
                MessageBox.Show(retornoTransacao.Descricao);
            }
        }
    }
}

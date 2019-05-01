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
using System.Windows.Shapes;

namespace Stone.WPF
{
    /// <summary>
    /// Lógica interna para Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private string numberCard { get; set; }

        HttpClient client = new HttpClient();
        public Client(string numeroCartao)
        {
            InitializeComponent();
            Bandeira.Items.Add("VISA");
            Bandeira.Items.Add("MasterCard");

            Tipo.Items.Add(new TipoDTO(1, "Tarja Magnética"));
            Tipo.Items.Add(new TipoDTO(2, "Chip"));

            client.BaseAddress = new Uri("http://localhost:54602");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            numberCard = numeroCartao;
            if (!string.IsNullOrEmpty(numberCard))
            {
                CarregaCliente();
            }
        }

        async void CarregaCliente()
        {

            HttpResponseMessage response = await client.GetAsync("/api/client/"+ numberCard);
            response.EnsureSuccessStatusCode(); //lança um código de erro
            string json = await response.Content.ReadAsStringAsync();
            ClienteDTO clienteDTO = JsonConvert.DeserializeObject<ClienteDTO>(json);

            NumeroCartao.Text = clienteDTO.NumeroCartao;
            NumeroCartao.IsReadOnly = true;

            Nome.Text = clienteDTO.Nome;
            Bandeira.SelectedItem = clienteDTO.Bandeira;
            Tipo.SelectedIndex = clienteDTO.Tipo - 1;
            Saldo.Text = clienteDTO.Saldo.ToString("N2");

        }

        private void Saldo_KeyDown(object sender, KeyEventArgs e)
        {
            KeyConverter key = new KeyConverter();
            if (new[] { Key.Tab, Key.OemComma }.Contains(e.Key)) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }

        private void Saldo_KeyUp(object sender, KeyEventArgs e)
        {

            KeyConverter key = new KeyConverter();

            if (e.Key == Key.Tab) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }

        private void Saldo_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!decimal.TryParse(textBox.Text, out _))
            {
                textBox.Clear();
            }
        }

        private void NumeroCartao_KeyDown(object sender, KeyEventArgs e)
        {
            KeyConverter key = new KeyConverter();

            if (e.Key == Key.Tab) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }

        private void NumeroCartao_KeyUp(object sender, KeyEventArgs e)
        {
            KeyConverter key = new KeyConverter();

            if (e.Key == Key.Tab) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }

        private void NumeroCartao_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!decimal.TryParse(textBox.Text, out _))
            {
                textBox.Clear();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void Salvar_ClickAsync(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(Nome.Text))
            {
                message += "-> Preencha o nome\n";
            }
            if (NumeroCartao.Text.Length < 12 || NumeroCartao.Text.Length > 19)
            {
                message += "-> Número de cartão inválido\n";
            }
            if (Bandeira.SelectedIndex == -1)
            {
                message += "-> Selecione uma bandeira\n";
            }
            if (Tipo.SelectedIndex == -1)
            {
                message += "-> Selecione um tipo\n";
            }

            if (Senha.Password.Length < 4 || Senha.Password.Length > 6 || Senha.Password != ConfirmarSenha.Password)
            {
                message += "-> Senhas inválidas\n";
            }

            if (!decimal.TryParse(Saldo.Text, out _) || decimal.Parse(Saldo.Text) <= 0)
            {
                message += "-> Saldo inválido\n";
            }

            if (!string.IsNullOrEmpty(message.Trim()))
            {
                MessageBox.Show(message.Trim());
            }
            else
            {
                var clienteDTO = new ClienteDTO()
                {
                    NumeroCartao = NumeroCartao.Text,
                    Saldo = decimal.Parse(Saldo.Text),
                    Tipo = ((TipoDTO)Tipo.SelectedItem).TipoID,
                    Nome = Nome.Text,
                    Bandeira = Bandeira.SelectedItem.ToString(),
                    Senha = Senha.Password
                };
                var response = await client.PostAsJsonAsync("/api/client/", clienteDTO);
                response.EnsureSuccessStatusCode(); //lança um código de erro
                string json = await response.Content.ReadAsStringAsync();
                var retorno = JsonConvert.DeserializeObject<RetornoTransacaoDTO>(json);

                if (retorno.Codigo == 1)
                {
                    MessageBox.Show("Dados salvos com sucesso!");
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception(retorno.Descricao);
                }
            }
        }
    }
}

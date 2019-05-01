using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interação lógica para TransactionPage.xam
    /// </summary>
    public partial class TransactionPage : Page
    {
        public TransactionPage()
        {
            InitializeComponent();
            Tipo.Items.Add(new TipoTransacaoDTO(1, "Crédito"));
            Tipo.Items.Add(new TipoTransacaoDTO(2, "Débito"));
        }

        private void Tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TipoTransacaoDTO tipo = (TipoTransacaoDTO)Tipo.SelectedItem;
            if (tipo != null && tipo.TipoTransacaoID == 1)
            {
                Parcelas.Visibility = Visibility.Visible;
            }
            else
            {
                Parcelas.Visibility = Visibility.Hidden;
            }
        }

        private void Limpar_Click(object sender, RoutedEventArgs e)
        {
            Senha.Clear();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Senha.Clear();
            Tipo.SelectedIndex = -1;
            NumeroCartao.Text = Valor.Text = Parcelas.Text = string.Empty;
        }

        private async void Enviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:54602");

                var responseAuthenticate = await client.PostAsJsonAsync("api/authenticate/", new PostAutenticacaoDTO { NumeroCartao = NumeroCartao.Text, Senha = Senha.Password });
                responseAuthenticate.EnsureSuccessStatusCode();
                string json = await responseAuthenticate.Content.ReadAsStringAsync();
                var authenticate = JsonConvert.DeserializeObject<AuthenticateDTO>(json);

                if (authenticate.RetornoTransacao != 8)
                {
                    throw new Exception(authenticate.Descricao);
                }

                var transacao = new TransacaoDTO()
                {
                    cartao = NumeroCartao.Text,
                    valor = decimal.Parse(Valor.Text),
                    tipo = ((TipoTransacaoDTO)Tipo.SelectedItem).TipoTransacaoID,
                    parcelas = int.TryParse(Parcelas.Text, out _) ? int.Parse(Parcelas.Text) : 1,
                    guid = authenticate.Guid
                };

                var response = await client.PostAsJsonAsync("/api/transaction/", transacao);
                response.EnsureSuccessStatusCode(); //lança um código de erro
                json = await response.Content.ReadAsStringAsync();
                var retorno = JsonConvert.DeserializeObject<RetornoTransacaoDTO>(json);

                if (retorno.Codigo == 1)
                {
                    MessageBox.Show("Transação efetuada com sucesso!");
                    Senha.Clear();
                    Tipo.SelectedIndex = -1;
                    NumeroCartao.Text = Valor.Text = Parcelas.Text = string.Empty;
                }
                else
                {
                    throw new Exception(retorno.Descricao);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void Parcelas_KeyDown(object sender, KeyEventArgs e)
        {
            KeyConverter key = new KeyConverter();

            if (e.Key == Key.Tab) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }

        private void Parcelas_KeyUp(object sender, KeyEventArgs e)
        {
            KeyConverter key = new KeyConverter();
            if (e.Key == Key.Tab) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }


        private void Valor_KeyDown(object sender, KeyEventArgs e)
        {
            KeyConverter key = new KeyConverter();
            if (new[] { Key.Tab, Key.OemComma }.Contains(e.Key)) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }

        private void Valor_KeyUp(object sender, KeyEventArgs e)
        {

            KeyConverter key = new KeyConverter();

            if (e.Key == Key.Tab) return;
            if ((char.IsNumber((string)key.ConvertTo(e.Key, typeof(string)), 0) == false))
            {
                e.Handled = true;
            }
        }

        private void Valor_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!decimal.TryParse(textBox.Text, out _))
            {
                textBox.Clear();
            }
        }

        private void Parcelas_LostFocus(object sender, RoutedEventArgs e)
        {

            TextBox textBox = (TextBox)sender;
            if (!decimal.TryParse(textBox.Text, out _))
            {
                textBox.Clear();
            }
        }
    }
}

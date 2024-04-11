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

namespace ZelentsovLanguage
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            LV_Clients.ItemsSource = ZelentsovLanguageEntities.GetContext().Client.ToList();
            UpdateClients();
        }
        int page = 0;
        int pageLength = -1;
        public void UpdateClients()
        {
            var allClients = ZelentsovLanguageEntities.GetContext().Client.ToList();
            //Filters and search
            allClients = allClients.FindAll(p =>
            p.Email.ToLower().Contains(TB_Serarch.Text.ToLower()) ||
            p.Phone.Replace("+", "").Replace("(", "").Replace(")", "").Contains(TB_Serarch.Text.Replace("+", "").Replace("(", "").Replace(")", "")) ||
            p.FirstName.ToLower().Contains(TB_Serarch.Text.ToLower()) ||
            p.LastName.ToLower().Contains(TB_Serarch.Text.ToLower()) ||
            p.Patronymic.ToLower().Contains(TB_Serarch.Text.ToLower()));
            switch ((CB_Filter.SelectedItem as ComboBoxItem).Tag)
            {
                case "Male":
                    allClients = allClients.FindAll(p => p.GenderName == "м");
                    break;
                case "Female":
                    allClients = allClients.FindAll(p => p.GenderName == "ж");
                    break;
            }

            switch ((CB_Search.SelectedItem as ComboBoxItem).Tag)
            {
                case "LastNameDesc":
                    allClients = allClients.OrderBy(p => p.LastName).ToList();
                    break;
                case "LastNameAsce":
                    allClients = allClients.OrderByDescending(p => p.LastName).ToList();
                    break;
                case "LastArrivalNew":
                    allClients = allClients.OrderByDescending(p => p.LastArrivalDate).ToList();
                    break;
                case "LastArrivalOld":
                    allClients = allClients.OrderBy(p => p.LastArrivalDate).ToList();
                    break;
                case "ArrivalCountDesc":
                    allClients = allClients.OrderBy(p => p.ArrivalCount).ToList();
                    break;
                case "ArrivalCountAsce":
                    allClients = allClients.OrderByDescending(p => p.ArrivalCount).ToList();
                    break;
            }
            switch (CB_PageLen.SelectedIndex)
            {
                case 0:
                    pageLength = -1;
                    break;
                case 1:
                    pageLength = 10;
                    break;
                case 2:
                    pageLength = 50;
                    break;
                case 3:
                    pageLength = 200;
                    break;
            }
            LB_pages.Items.Clear();
            decimal MaxPages = decimal.Round((decimal)allClients.Count / pageLength);
            for (int i = 0; i < MaxPages; i++)
            {
                LB_pages.Items.Add(i + 1);
            }
            LB_pages.SelectedIndex = page;
            TB_allRec.Text = ZelentsovLanguageEntities.GetContext().Client.ToList().Count.ToString();
            TB_selectedRec.Text = allClients.Count.ToString();
            if (pageLength != -1)
            {
                var cutLength = 0;
                if (((page + 1) * pageLength) > allClients.Count)
                {
                    cutLength = allClients.Count - (page * pageLength);
                    if(cutLength > 0) LV_Clients.ItemsSource = allClients.GetRange(page * pageLength, cutLength);

                }
                else
                    LV_Clients.ItemsSource = allClients.GetRange(page * pageLength, pageLength);
            }
            else if (pageLength == -1)
                LV_Clients.ItemsSource = allClients;

        }

        private void Btn_right_page_Click(object sender, RoutedEventArgs e)
        {
            if (((page + 1) * pageLength) >= ZelentsovLanguageEntities.GetContext().Client.ToList().Count)
                return;
            else
                page++;
            UpdateClients();
        }

        private void Btn_left_page_Click(object sender, RoutedEventArgs e)
        {
            if (page != 0)
                page--;
            UpdateClients();
        }

        private void Btn_RemoveClient_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as Button).DataContext as Client;
            if (client.ClientService.Count > 0)
            {
                MessageBox.Show("Невозможно удалить клинета у которого были посещения");
                return;
            }
            try
            {
                ZelentsovLanguageEntities.GetContext().Client.Remove(client);
                ZelentsovLanguageEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения изменений");
            }
            UpdateClients();
        }

        private void CB_PageLen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                UpdateClients();
        }

        private void TB_Serarch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClients();
        }

        private void CB_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                UpdateClients();
            }
        }

        private void CB_Search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                UpdateClients();
            }

        }
    }
}

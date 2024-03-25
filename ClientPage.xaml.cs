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
        int page = 1;
        int pageLength = -1;
        public void UpdateClients()
        {
            var allClients = ZelentsovLanguageEntities.GetContext().Client.ToList();
            //Filters and search
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
            TB_allRec.Text = ZelentsovLanguageEntities.GetContext().Client.ToList().Count.ToString();
            TB_selectedRec.Text = allClients.Count.ToString();
            if (pageLength != -1 && (page * pageLength) < allClients.Count)
                LV_Clients.ItemsSource = allClients.GetRange(page * pageLength, pageLength);
            else if(pageLength==-1)
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
    }
}

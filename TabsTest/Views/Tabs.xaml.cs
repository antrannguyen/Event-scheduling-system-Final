using System;
using System.Collections.Generic;
using System.Data.Linq;
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
using System.Windows.Shapes;

namespace TabsTest.Views
{
    /// <summary>
    /// Interaction logic for Tabs.xaml
    /// </summary>
    public partial class Tabs : Window
    {
        public Tabs()
        {
            InitializeComponent();
        }

        private void UpdateCurrentUSer_Button_Click(object sender, RoutedEventArgs e)
        {
            // User Management Tab
            // Will use ViewModels function

            var vm = (TabsTest.ViewModels.TabsViewModel)this.DataContext;

            if (String.IsNullOrEmpty(UserMan_Name.Text))
            {
                return;
            }


            string userName = (UserMan_Name.Text ?? String.Empty);
            string phoneNumber = (UserMan_PhoneNum.Text ?? String.Empty); 
            string roleID = (UserMan_RoleID.Text ?? String.Empty);
            string organization = (UserMan_Organization.Text ?? String.Empty);
            string enabled = (UserMan_Enabled.Text ?? String.Empty);            

            vm.UpdateUserInformationToDatabase(userName, phoneNumber, roleID, organization, enabled);

            UserMan_Status.Content = userName + " entry was updated";
        }

        private void DeleteUser_Button_Click(object sender, RoutedEventArgs e)
        {

            // User Management Tab
            // Will use ViewModels function

            var vm = (TabsTest.ViewModels.TabsViewModel)this.DataContext;

            if (String.IsNullOrEmpty(UserMan_Name.Text))
            {
                return;
            }


            string userName = (UserMan_Name.Text ?? String.Empty);

            vm.DeleteUserCompletely(userName);

            UserMan_Status.Content = userName + " was deleted";

        }
    }
}

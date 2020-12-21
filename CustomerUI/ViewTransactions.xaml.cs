using SharedCode;
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
using System.Windows.Shapes;

namespace CustomerUI
{
    /// <summary>
    /// Interaction logic for ViewTransactions.xaml
    /// </summary>
    public partial class ViewTransactions : Window
    {
        //public List<>
        User loggedInUser = null;
        public ViewTransactions(User loggedInUser)
        {
            InitializeComponent();
            lblLoggedInAs.Content = "Logged as " + loggedInUser.FirstName + " " + loggedInUser.LastName;
        }
    }
}

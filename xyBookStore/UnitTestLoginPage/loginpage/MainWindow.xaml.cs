//Ali Mula
//104979831
//COMP-4220
//2021-09-21
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
using UnitTestProject1;

namespace loginpage
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)                        //Private Function for the ok button
        {
            string usernm = this.nameTextBox.Text;
            string passwd = this.passwordTextBox.Password;
            bool containsSpecialCharacter = false;
            foreach (char c in passwd)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    containsSpecialCharacter = true; break;
                }
            }
                if (containsSpecialCharacter)
            {
                MessageBox.Show("A valid password needs to have at least six characters with both letters and numbers and no symbols."); //checking for special character
            }
             else if (passwd.Length > 5 && passwd.Any(Char.IsLetter) && passwd.Any(Char.IsNumber) && passwd[0] > 63 && passwd[0] < 123) //If statement to check if all requirements are met 
            {
                {
                    var userData = new UserData(); // get the user data for the current user.
                                                   // Check if the user has logged in by calling the LogIn method from the UserData class.
                    if (userData.LogIn(usernm, passwd) == true)
                        MessageBox.Show("You are logged in as User #" + userData.UserID);
                    else
                        MessageBox.Show("You could not be verified. Please try again.");
                    return;
                }

            }
            else if (String.IsNullOrEmpty(usernm) || (String.IsNullOrEmpty(passwd)))         //else statement checking if the inputs are left empty
                MessageBox.Show("Please fill in all slots.");

            else                                                                             //else statement if both statements above aren't fufilled 
                MessageBox.Show("A valid password needs to have at least six characters with both letters and numbers and no symbols.");
        }
        private void cancelButton_Click(object sneder, RoutedEventArgs e)                   //Private Function for the close button
        {
            this.Close();                                                                   //Closes program once cancel button is closed
        }
    }
}

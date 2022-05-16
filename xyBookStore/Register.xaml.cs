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

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        public bool usernameValid(string username)
        {
            // variable to validate field
            bool validity = true;

            // variables for various tests
            bool letterStart = false; // check that username starts with a letter
            bool badChar = false; // check that username only contains allowed characters

            // Test 1: Check that field is not empty
            if (String.IsNullOrEmpty(username) == true) validity = false;

            // Test 2: Username can only be 30 characters max
            if (username.Length > 30) validity = false;

            // Test 3: Username must start with a letter
            for (int ch = 65; ch < 123; ch++)
            {
                // letters in ASCII are 65-90 (uppercase) or 97-122 (lowercase)
                if (ch < 91 || ch > 96)
                {
                    // if the first char is any letter, password is valid
                    if (username[0] == (char)ch) letterStart = true;
                }
            }
            if (!letterStart) validity = false;

            // Test 4: Capitalize first letter if not already capitalized


            // Test 5: All characters must be alphanumeric or be of the symbols _.-'
            // check for each possible ASCII character
            for (int ch = 0; ch < 257; ch++)
            {
                // if char is neither a number nor a letter (ASCII 0-47, 58-64, 91-96, and 123+)
                if (ch < 48 || (ch > 57 && ch < 65) || (ch > 90 && ch < 97) || ch > 122)
                {
                    // if char is not ' (ASCII 39), - (ASCII 45), . (ASCII 46), or _ (ASCII 95)
                    if (ch != 39 || ch != 45 || ch != 46 || ch != 95)
                    {
                        // check if username contains invalid character
                        if (username.Contains((char)ch)) badChar = true;
                    }
                }
            }
            if (badChar) validity = false;

            /*
            for (int i = 0; i < username.Length; i++)
            {
                int ch = (int) username[i];
                // if char is neither a number nor a letter (ASCII 0-47, 58-64, 91-96, and 123+)
                if (ch < 48 || (ch > 57 && ch < 65) || (ch > 90 && ch < 97) || ch > 122)
                {
                    // if char is not ' (ASCII 39), - (ASCII 45), . (ASCII 46), or _ (ASCII 95)
                    if (ch != 39 || ch != 45 || ch != 46 || ch != 95)
                    {
                        // check if username contains invalid character
                        validity = false;
                    }
                }
            }
            */

            // Test 6: Check that username is available - not already used by existing acct


            return validity;
        }
        public bool namesValid(string first, string last)
        {
            // variable to validate field
            bool validity = true;

            // variables for testing
            bool firstHas = false;
            bool lastHas = false;
            bool badChar = false;

            // Test 1: Check that fields are not empty
            if (String.IsNullOrEmpty(first)) validity = false;
            if (String.IsNullOrEmpty(last)) validity = false;

            // Test 2: Each name can only be 30 characters max
            if (first.Length > 30) validity = false;
            if (last.Length > 30) validity = false;

            // Tests 3 & 4
            // check for each possible ASCII character
            for (int ch = 0; ch < 257; ch++)
            {
                // Test 3: Password contains at least 1 letter (ASCII 65-90, 97-122)
                if ((ch > 64 && ch < 91) || (ch > 96 && ch < 123))
                {
                    // if fields contain letter
                    if (first.Contains((char)ch)) firstHas = true;
                    if (last.Contains((char)ch)) lastHas = true;
                }
                // Test 4: If not a letter, only non-letter characters allowed are ' (ASCII 39) and - (ASCII 45)
                else if (ch != 39 && ch != 45)
                {
                    // if either contains invalid char
                    if (first.Contains((char)ch) || last.Contains((char)ch)) badChar = true;
                }
            }
            // check that names contain required and valid characters
            if (!firstHas || !lastHas || badChar) validity = false;

            // for each name, capitalize first letter if not already captialized
            return validity;
        }
        public bool passwordValid(string pw)
        {
            // variable to validate password
            bool validity = true;

            // variables for testing
            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumber = false;
            bool hasSymbol = false;

            // Test 1: Password is at least 6 characters long
            if (pw.Length < 6) validity = false;

            // Test 2: Password is at most 30 characters long
            if (pw.Length > 30) validity = false;

            // Tests 3-6
            // check for each possible ASCII character
            for (int ch = 0; ch < 257; ch++)
            {
                // Test 3: Password contains at least 1 uppercase letter (ASCII 65-90)
                if (ch > 64 && ch < 91)
                {
                    // if password contains uppercase letter
                    if (pw.Contains((char)ch)) hasUpper = true;
                }

                // Test 4: Password contains at least 1 lowercase letter (ASCII 97-122)
                else if (ch > 96 && ch < 123)
                {
                    // if password contains lowercase letter
                    if (pw.Contains((char)ch)) hasLower = true;
                }

                // Test 5: Password contains at least 1 number (ASCII 48-57)
                else if (ch > 47 && ch < 58)
                {
                    // if password contains number
                    if (pw.Contains((char)ch)) hasNumber = true;
                }

                // Test 6: Password contains at least 1 non-alphanumeric character
                else
                {
                    // if password contains non-alphanumeric character
                    if (pw.Contains((char)ch)) hasSymbol = true;
                }
            }
            // check that all required characters are present
            if (!hasUpper || !hasLower || !hasNumber || !hasSymbol) validity = false;

            // return validity of password
            return validity;
        }
        public bool passwordConfirmed(string pw, string pwcheck)
        {
            // variable to validate field
            bool validity;

            // ensure password check is a valid password
            if (passwordValid(pwcheck))
            {
                // if so, ensure password check matches password
                validity = pw.Equals(pwcheck);
            }
            // if password check is invalid, failure
            else validity = false;

            return validity;
        }
        public bool emailValid(string email)
        {
            // variable to validate field
            bool validity = true;

            // check variables
            bool hasAt = false;
            bool hasDot = false;

            // Test 1: Check that field is not empty
            if (String.IsNullOrEmpty(email)) validity = false;

            // Test 2: Check that email is in proper format - contains both '@' (ASCII 40) and '.' (ASCII 46)
            // check for each possible ASCII character
            for (int ch = 0; ch < 257; ch++)
            {
                // Test 3: Password contains at least 1 uppercase letter (ASCII 65-90)
                if (ch == 64)
                {
                    if (email.Contains((char)ch)) hasAt = true;
                }
                // Test 4: If char is '.'
                else if (ch == 46)
                {
                    if (email.Contains((char)ch)) hasDot = true;
                }
            }
            // check that email contains both required chars
            if (!hasAt || !hasDot) validity = false;

            // Test 3: Check that email is available - not already used by existing acct


            return validity;
        }
        public bool emailConfirmed(string em, string emcheck)
        {
            // variable to validate field
            bool validity;

            // Test 1: Ensure emcheck is a valid email address
            if (emailValid(emcheck))
            {
                // Test 2: If so, ensure emcheck matches em
                validity = em.Equals(emcheck);
            }
            // if emcheck is invalid, failure
            else validity = false;

            return validity;
        }

        private void okay_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            // get registration input
            string un = this.usernameBox.Text;
            string fn = this.fnameBox.Text;
            string ln = this.surnameBox.Text;
            string em = this.email.Text;
            string emchk = this.emailCheck.Text;
            string pw = this.password.Password;
            string pwchk = this.passwordCheck.Password;

            // check that username is valid
            if (usernameValid(un))
            {
                // check that first and last names are valid
                if (namesValid(fn, ln))
                {
                    if (emailValid(em))
                    {
                        if (emailConfirmed(em, emchk))
                        {
                            // check that password passes all validity checks
                            if (passwordValid(pw))
                            {
                                if (passwordConfirmed(pw, pwchk))
                                {
                                    // success message
                                    MessageBox.Show("Thank you! All input is validated.");
                                }
                                else
                                {
                                    // password mismatch message
                                    MessageBox.Show("Passwords don't match.");
                                }

                            }
                            else
                            {
                                // invalid password message
                                MessageBox.Show("A valid password needs to have at least six characters with both letters and numbers.");
                            }
                        }
                    }

                }
                // invalid input message
                else MessageBox.Show("Please fill in all slots.");
            }
            // invalid input message
            else MessageBox.Show("Please fill in all slots.");
        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

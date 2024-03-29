﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFUIKitProfessional.Service;

namespace WPFUIKitProfessional.Authorization
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_MouseEnter(object sender, MouseEventArgs e) => Cursor = Cursors.Hand;
        private void LoginBtn_MouseLeave(object sender, MouseEventArgs e) => Cursor = Cursors.Arrow;
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e) => Cursor = Cursors.IBeam;

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var account = Window.GetWindow(this);
            (account as Account).Registration();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text == string.Empty || pb.Password == string.Empty)
            {
                alert.Foreground = Brushes.Red;
                alert.Text = "Fill all fields";
                return;
            }
            alert.Text = string.Empty;

            var account = (Account)Window.GetWindow(this);
            if (account.IsAuthorized(login.Text).Result)
            {
                if (account.IsAuthorized(login.Text, Encryption.GetHashString(pb.Password)).Result)
                {
                    var MainWindow = (App.Current.MainWindow as MainWindow);
                    MainWindow.CurrentUser = account.GetUser(login.Text, Encryption.GetHashString(pb.Password)).Result;
                    MainWindow.Visibility = Visibility.Visible;
                    if (login.Text == "admin")
                        MainWindow.rdLevelConstructor.Visibility = Visibility.Visible;
                    else
                        MainWindow.rdLevelConstructor.Visibility = Visibility.Hidden;

                    MainWindow.Users.id.Text = MainWindow.CurrentUser.Id.ToString();
                    MainWindow.Users.login.Text = MainWindow.CurrentUser.Login;
                    MainWindow.Users.date.Text = MainWindow.CurrentUser.Date.ToString();

                    account.Close();
                }
            }
            alert.Foreground = Brushes.Red;
            alert.Text = "Invalid login or password";
        }
    }
}
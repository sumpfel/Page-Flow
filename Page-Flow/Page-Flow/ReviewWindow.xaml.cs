using BookLib;
using HTTPClient;
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

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for ReviewWindow.xaml
    /// </summary>
    public partial class ReviewWindow : Window
    {
        IReviewable Reviewable;
        HttpControler Client;
        public ReviewWindow(IReviewable Reviewable_, HttpControler Client_)
        {
            InitializeComponent();
            Reviewable=Reviewable_;
            Client = Client_;
            UpdateLikes();
            UpdateComments();
            LabelComments.Content = $"{Reviewable.Comments.Count} 🗨";
        }

        private void UpdateLikes()
        {
            try
            {
                int likes;
                int disLikes;
                if (Reviewable.FakeLikes < 0)
                {
                    likes = Convert.ToInt32(Convert.ToDouble(Reviewable.Likes) / (Reviewable.DisLikes + Reviewable.Likes - Reviewable.FakeLikes) * 100);
                    disLikes = Convert.ToInt32(Convert.ToDouble(Reviewable.DisLikes - Reviewable.FakeLikes) / (Reviewable.DisLikes + Reviewable.Likes - Reviewable.FakeLikes) * 100);
                }
                else
                {
                    likes = Convert.ToInt32(Convert.ToDouble(Reviewable.Likes + Reviewable.FakeLikes) / (Reviewable.DisLikes + Reviewable.Likes + Reviewable.FakeLikes) * 100);
                    disLikes = Convert.ToInt32(Convert.ToDouble(Reviewable.DisLikes) / (Reviewable.DisLikes + Reviewable.Likes + Reviewable.FakeLikes) * 100);
                }
                LabelLikes.Content = $"{likes}% 👍";
                LabelDisLikes.Content = $"{disLikes}% 👎";
            }
            catch
            {
                LabelLikes.Content = $"--- 👍";
                LabelDisLikes.Content = $"--- 👎";
            }
        }

        private void UpdateComments()
        {
            CommentView.Children.Clear();
            foreach (string comment_ in Reviewable.Comments)
            {
                try
                {
                    string[] comment = comment_.Split("@@");
                    TextBlock commentText = new TextBlock();
                    commentText.Text = comment[1];
                    commentText.FontSize = 15;
                    commentText.Padding = new Thickness(10, 0, 0, 0);
                    commentText.TextWrapping = TextWrapping.Wrap;
                    Label nameText = new Label();
                    nameText.Content = comment[0];
                    nameText.FontSize = 20;
                    nameText.Padding = new Thickness(0);

                    StackPanel commentsPanel = new StackPanel();
                    commentsPanel.Margin = new Thickness(5);
                    commentsPanel.Children.Add(nameText);
                    commentsPanel.Children.Add(commentText);

                    Border border = new Border();
                    border.Child = commentsPanel;
                    border.BorderThickness = new Thickness(5);
                    border.CornerRadius = new CornerRadius(10);
                    border.Background = Brushes.CadetBlue;

                    CommentView.Children.Add(border);
                }catch { }
            }
                
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Reviewable.Path);
            if (CommentTextBox.Text != "")
            {
                bool succes=await Client.sendComment(Reviewable.Path, CommentTextBox.Text);
                if (succes)
                {
                    Reviewable.Comments.Add($"{Client.GetUserName()}@@{CommentTextBox.Text}");
                    LabelComments.Content = $"{Reviewable.Comments.Count - 1} 🗨";
                    UpdateComments();
                }
                else { MessageBox.Show("Error: Comment couldn't be sent try again later"); }
            }
            else { MessageBox.Show("enter a comment first"); }
        }

        private async void MuchBad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                bool succes;
                int likes;
                switch (button.Name)
                {
                    case "MuchBad":
                        likes = -2;
                        break;

                    case "Bad":
                        likes = -1;
                        break;

                    case "Good":
                        likes=1;
                        break;

                    case "MuchGood":
                        likes = 2;
                        break;

                    default:
                        likes = 0;
                        break;
                }
                succes=await Client.sendLikes(Reviewable.Path,likes.ToString());
                if (succes)
                {
                    Reviewable.FakeLikes = likes;
                    UpdateLikes();
                }
                else
                {
                    MessageBox.Show("Error: Reaction couldn't be sent try again later");
                }
            }
            
        }
    }
}

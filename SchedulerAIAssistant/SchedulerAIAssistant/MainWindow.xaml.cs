namespace SchedulerAIAssistant
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Syncfusion.UI.Xaml.Chat;
    using Microsoft.UI.Xaml.Input;

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            if(this.Content is Grid grid)
            {
                grid.Loaded += MainWindow_Loaded;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.AzureBaseService = new AzureOpenAIBaseService();
        }

        private void OnAIButtonClick(object sender, RoutedEventArgs e)
        {
            this.assistViewBorder.Visibility = Visibility.Visible;
        }

        private void OnChatSuggestionSelected(object sender, SuggestionClickedEventArgs e)
        {
            if (sender is SfAIAssistView assistView && assistView.DataContext is SmartSchedulerViewModel viewModel)
            {
                viewModel.Chats.Add(new TextMessage { Text = e.Item.ToString(), DateTime = DateTime.Now, Author = assistView.CurrentUser });
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.assistViewBorder.Visibility = Visibility.Collapsed;
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is SmartSchedulerViewModel viewModel)
            {
                for (int j = 0; j < viewModel.Chats.Count; j++)
                {
                    viewModel.Chats.RemoveAt(0);
                }
                viewModel.Suggestions.Clear();
            }
        }

        private void OnOptionstListViewTapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is ListView listView && listView.DataContext is SmartSchedulerViewModel viewModel)
            {
                if (e.OriginalSource is TextBlock textBlock)
                {
                    viewModel.HandleOfflineAppointmentChat(textBlock.Text);
                }
            }
        }

        private void SfScheduler_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.assistViewBorder.Visibility = Visibility.Collapsed;
        }
    }
}
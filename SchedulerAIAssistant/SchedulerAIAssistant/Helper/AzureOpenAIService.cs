using Microsoft.Extensions.AI;
using Azure.AI.OpenAI;
using Azure;
using System;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace SchedulerAIAssistant
{
    internal class AzureBaseService
    {
        #region Fields

        /// <summary>
        /// The EndPoint
        /// </summary>
        private const string endpoint = "AZURE_OPENAI_ENDPOINT";

        /// <summary>
        /// The Deployment name
        /// </summary>
        private const string deploymentName = "DEPLOYMENT_NAME";

        /// <summary>
        /// The API key
        /// </summary>
        private const string key = "API_KEY";

        /// <summary>
        /// The OpenAI
        /// </summary>
        private IChatClient? client;

        /// <summary>
        /// The credential valid field
        /// </summary>
        private bool isCredentialValid = false;

        /// <summary>
        /// The already credential validated field
        /// </summary>
        private bool isAlreadyValidated = false;

        /// <summary>
        /// The chat history
        /// </summary>
        private string? chatHistory;

        #endregion

        public AzureBaseService()
        {
            ValidateCredential();
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating the chat history object
        /// </summary>
        public string? ChatHistory
        {
            get
            {
                return chatHistory;
            }
            set
            {
                chatHistory = value;
            }
        }

        /// <summary>
        /// Gets or Set a value indicating whether an credentials are valid or not.
        /// Returns <c>true</c> if the credentials are valid; otherwise, <c>false</c>.
        /// </summary>
        public bool IsCredentialValid
        {
            get
            {
                return isCredentialValid;
            }
            set
            {
                isCredentialValid = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the OpenAI object
        /// </summary>
        public IChatClient? Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validate Azure Credentials
        /// </summary>
        private async void ValidateCredential()
        {
            #region Azure OpenAI
            // Use below method for Azure Open AI
            this.GetAzureOpenAIKernal();
            #endregion

            #endregion

            if (isAlreadyValidated)
            {
                return;
            }

            try
            {
                if (Client != null)
                {
                    await Client!.CompleteAsync("Hello, Test Check");
                    chatHistory = string.Empty;
                    IsCredentialValid = true;
                    isAlreadyValidated = true;
                }
                else
                {
                    ShowAlertAsync();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        #region Azure OpenAI
        /// <summary>
        /// To get the Azure OpenAI method
        /// </summary>
        private void GetAzureOpenAIKernal()
        {
            try
            {
                var client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key)).AsChatClient(modelId: deploymentName);
                this.client = client;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Show Alert 
        /// </summary>
        private static async void ShowAlertAsync()
        {
            var window = (Application.Current as App)?.m_window as MainWindow;

            if (window != null)
            {
                var dialog = new ContentDialog
                {
                    Content = "The Azure API key or endpoint is missing or incorrect. Please verify your credentials. You can also continue with the offline data.",
                    CloseButtonText = "OK",
                    XamlRoot = window?.Content.XamlRoot
                };
                try
                {
                    await dialog.ShowAsync();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Gets the AI response.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns>The AI response.</returns>
        internal async Task<string> GetAIResponse(string userPrompt)
        {
            if (IsCredentialValid && Client != null)
            {
                ChatHistory = string.Empty;
                // Add the system message and user message to the options
                ChatHistory = ChatHistory + "You are a predictive analytics assistant.";
                ChatHistory = ChatHistory + userPrompt;
                try
                {
                    var response = await Client.CompleteAsync(ChatHistory);
                    return response.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
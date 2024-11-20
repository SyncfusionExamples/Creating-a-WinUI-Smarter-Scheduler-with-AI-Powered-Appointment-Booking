namespace SchedulerAIAssistant
{
    using Microsoft.SemanticKernel;
    using Microsoft.SemanticKernel.ChatCompletion;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the Semantic Kernel Service.
    /// </summary>
    internal class SemanticKernelService
    {
        IChatCompletionService? chatCompletionService;
        Kernel? kernel;

        /// <summary>
        /// Gets or sets the model name in the AI Setup Window.
        /// </summary>
        private string DeploymentName;

        /// <summary>
        /// Gets or sets the end point in the AI Setup Window.
        /// </summary>
        private string EndPoint;

        /// <summary>
        /// Gets or sets the key in the AI Setup Window.
        /// </summary>
        private string Key;

        /// <summary>
        /// Gets or sets a value indicating whether the AI Credentials is valid or not.
        /// </summary>
        internal static bool IsCredentialValid { get; set; }

        internal SemanticKernelService()
        {
            this.DeploymentName = "deployment name";
            this.EndPoint = "https://YOUR_ACCOUNT.openai.azure.com/";
            this.Key = "API key";
        }

        /// <summary>
        /// Gets the AI response.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns>The AI response.</returns>
        internal async Task<string> GetAIResponse(string prompt)
        {
            if (this.chatCompletionService == null)
            {
                return "";
            }

            var Conversation = new ChatHistory();
            Conversation.AddUserMessage(prompt);
            var response = await this.chatCompletionService.GetChatMessageContentAsync(chatHistory: Conversation, kernel: kernel);
            return response.ToString();
        }

        /// <summary>
        /// Validates the AI Credentials.
        /// </summary>
        internal async void CredentialValidation()
        {
            var ErrorMessage = string.Empty;
            Uri? uriResult;
            bool isValidUri = Uri.TryCreate(EndPoint, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isValidUri || !EndPoint.Contains("http"))
            {
                ErrorMessage = "Please enter valid EndPoint.";
            }
            else
            {
                try
                {
                    var aiMsg = new ChatHistory();
                    var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(DeploymentName, EndPoint, Key);

                    this.kernel = builder.Build();
                    this.chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
                    aiMsg.AddUserMessage("Hi");
                    var response = await this.chatCompletionService.GetChatMessageContentAsync(chatHistory: aiMsg, kernel: kernel);
                }
                catch (Exception ex)
                {
                    //Check the error message and display the appropriate message
                    if (ex.Message.Contains("API deployment"))
                    {
                        ErrorMessage = "Please enter valid ModelName.";
                    }
                    else if (ex.Message.Contains("Access denied"))
                    {
                        ErrorMessage = "Please enter valid Key.";
                    }
                    else
                    {
                        ErrorMessage = "Please enter valid EndPoint.";
                    }
                }
            }

            IsCredentialValid = string.IsNullOrEmpty(ErrorMessage) ? true : false;
        }
    }
}
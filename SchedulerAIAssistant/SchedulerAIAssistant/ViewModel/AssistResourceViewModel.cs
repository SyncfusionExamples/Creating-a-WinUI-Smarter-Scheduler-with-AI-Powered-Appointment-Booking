namespace SchedulerAIAssistant
{
    /// <summary>
    /// The AI assist resource view model class.
    /// </summary>
    public class AIAssistResourceViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AIAssistResourceViewModel"/> class.
        /// </summary>
        public AIAssistResourceViewModel()
        {
            this.Name = string.Empty;
            this.ImageName = string.Empty;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the image name.
        /// </summary>
        public string ImageName { get; set; }
    }
}
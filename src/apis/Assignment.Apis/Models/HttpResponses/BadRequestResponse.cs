namespace Assignment.Apis.Models.HttpResponses
{
    public class BadRequestResponse : HttpResponse
    {
        #region Constructor

        public BadRequestResponse(string[] messages)
        {
            Messages = messages;
        }

        #endregion

        #region Properties

        public string[] Messages { get; }

        #endregion
    }
}
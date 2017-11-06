namespace DevStats.Domain.MVP
{
    public class VoteResult
    {
        public bool WasSuccessful { get; private set; }

        public string Message { get; private set; }

        public VoteResult(bool wasSuccessful, string message)
        {
            WasSuccessful = wasSuccessful;
            Message = message;
        }
    }
}
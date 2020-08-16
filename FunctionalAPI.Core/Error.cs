namespace FunctionalAPI.Core
{
    public abstract class Error
    {
        public string ErrorMessage { get; protected set; } = "An error has occurred";
    }
}

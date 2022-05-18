namespace ToDoAPI.Exception
{
    public class InvalidCredentialsException : System.Exception
    {
        public InvalidCredentialsException()    
        {
        }

        public InvalidCredentialsException(string message) : base(message)
        {
        }

        public InvalidCredentialsException(string message,
            System.Exception exception) : base(message, exception)
        {
        }
    }
}

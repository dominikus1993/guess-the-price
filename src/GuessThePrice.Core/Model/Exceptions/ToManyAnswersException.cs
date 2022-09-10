namespace GuessThePrice.Core.Model.Exceptions;

public class ToManyAnswersException : Exception
{
    public ToManyAnswersException(string message) : base(message)
    {
    }

    public ToManyAnswersException()
    {
    }

    public ToManyAnswersException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
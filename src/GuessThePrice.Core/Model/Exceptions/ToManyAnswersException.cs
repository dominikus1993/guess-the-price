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

public class GameIsNotStartedException : Exception
{
    public GameIsNotStartedException(string message) : base(message)
    {
    }

    public GameIsNotStartedException()
    {
    }

    public GameIsNotStartedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class GameFinishedException : Exception
{
    public GameFinishedException(string message) : base(message)
    {
    }

    public GameFinishedException()
    {
    }

    public GameFinishedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class ProductsNotExistsException : Exception
{
    public ProductsNotExistsException(string message) : base(message)
    {
    }

    public ProductsNotExistsException()
    {
    }

    public ProductsNotExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class ResponseExistsException : Exception
{
    public ResponseExistsException(string message) : base(message)
    {
    }

    public ResponseExistsException()
    {
    }

    public ResponseExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
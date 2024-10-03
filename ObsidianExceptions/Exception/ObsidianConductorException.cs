namespace ObsidianExceptions;
/// <summary>
/// Represents an exception that occurs within the <see cref="ObsidianConductor"/> class, providing context 
/// about the operation that failed and any file or record-related issues.
/// </summary>
/// <remarks>
/// This exception is designed to handle errors specific to the operations of the ObsidianConductor class,
/// such as file handling, record manipulation, and parsing.
/// </remarks>
public sealed class ObsidianConductorException : Exception
{
    /// <summary>
    /// Gets the name of the operation (e.g., "CreateCharacterFile", "AddRecordToCharacter") that failed.
    /// </summary>
    public string Operation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianConductorException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the cause of the exception.</param>
    /// <param name="operation">The name of the operation that caused the error.</param>
    public ObsidianConductorException(string message, string operation)
        : base(message)
    {
        Operation = operation;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianConductorException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the cause of the exception.</param>
    /// <param name="operation">The name of the operation that caused the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ObsidianConductorException(string message, string operation, Exception innerException)
        : base(message, innerException)
    {
        Operation = operation;
    }

    /// <summary>
    /// Returns a string representation of the exception, including the message, operation, and stack trace.
    /// </summary>
    /// <returns>A string that represents the exception.</returns>
    public override string ToString() => $"{base.ToString()}\nOperation: {Operation}";
}
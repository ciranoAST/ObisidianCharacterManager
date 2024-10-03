namespace ObsidianExceptions;
/// <summary>
/// Represents an exception that occurs during record-related operations within the Obsidian commands.
/// </summary>
/// <remarks>
/// This exception is typically thrown when there are issues with input validation for records (e.g., missing or invalid data),
/// or when an error occurs during repository operations related to records (e.g., adding, updating, or deleting a record).
/// </remarks>
public sealed class ObsidianRecordException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianRecordException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the cause of the exception.</param>
    public ObsidianRecordException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianRecordException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the cause of the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ObsidianRecordException(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianRecordException"/> class using the message from the 
    /// inner exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ObsidianRecordException(Exception innerException)
        : base(innerException.Message, innerException) { }

    /// <summary>
    /// Returns a string representation of the exception, including the message and the stack trace.
    /// </summary>
    /// <returns>A string that represents the exception.</returns>
    public override string ToString() => $"{base.ToString()}\nRecord-related error occurred.";
}
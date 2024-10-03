namespace ObsidianExceptions;

/// <summary>
/// Represents an exception that occurs during file-related operations within the Obsidian system.
/// </summary>
/// <remarks>
/// This exception is intended to be thrown when errors occur while working with files, such as
/// reading, writing, or deleting files, and other file system issues in the context of character or record management.
/// </remarks>
public sealed class ObsidianFileException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianFileException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the cause of the exception.</param>
    public ObsidianFileException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianFileException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the cause of the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ObsidianFileException(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianFileException"/> class using the message from the
    /// inner exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ObsidianFileException(Exception innerException)
        : base(innerException.Message, innerException) { }

    /// <summary>
    /// Returns a string representation of the exception, including the message and the stack trace.
    /// </summary>
    /// <returns>A string that represents the exception.</returns>
    public override string ToString() => $"{base.ToString()}\nFile-related error occurred.";
}
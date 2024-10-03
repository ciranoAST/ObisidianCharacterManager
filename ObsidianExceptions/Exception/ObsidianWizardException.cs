namespace ObsidianExceptions;
/// <summary>
/// Represents an exception that occurs within the <see cref="ObsidianWizard"/> class, providing detailed context 
/// about the operation that failed and the content that caused the error.
/// </summary>
public sealed class ObsidianWizardException : Exception
{
    /// <summary>
    /// Gets the content of the file (or a truncated version of it) that caused the exception.
    /// </summary>
    public string FileContent { get; }

    /// <summary>
    /// Gets the name of the operation (e.g., "ParseCharacter", "ToMarkdown") that failed.
    /// </summary>
    public string Operation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianWizardException"/> class with a specified error message,
    /// the operation that failed, and the file content that caused the error.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="operation">The name of the operation that caused the error.</param>
    /// <param name="fileContent">The content of the file that caused the exception. If the content is too large, 
    /// it will be truncated to a maximum of 1000 characters.</param>
    public ObsidianWizardException(string message, string operation, string fileContent)
        : base(message)
    {
        Operation = operation;
        FileContent = fileContent.Length > 1000 ? string.Concat(fileContent.AsSpan(0, 1000), "...") : fileContent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObsidianWizardException"/> class with a specified error message, 
    /// the operation that failed, the file content that caused the error, and a reference to the inner exception 
    /// that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="operation">The name of the operation that caused the error.</param>
    /// <param name="fileContent">The content of the file that caused the exception. If the content is too large, 
    /// it will be truncated to a maximum of 1000 characters.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference 
    /// if no inner exception is specified.</param>
    public ObsidianWizardException(string message, string operation, string fileContent, Exception innerException)
        : base(message, innerException)
    {
        Operation = operation;
        FileContent = fileContent.Length > 1000 ? string.Concat(fileContent.AsSpan(0, 1000), "...") : fileContent;
    }

    /// <summary>
    /// Returns a string that represents the current exception, including the error message, operation name, 
    /// and the file content involved in the exception.
    /// </summary>
    /// <returns>A string representation of the exception.</returns>
    public override string ToString() => $"{base.ToString()}\nOperation: {Operation}\nFile Content: {FileContent}";
}


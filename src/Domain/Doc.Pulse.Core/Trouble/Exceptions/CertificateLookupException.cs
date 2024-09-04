namespace Doc.Pulse.Core.Trouble.Exceptions;

public class CertificateLookupException(string? strategy, string identifier, string? text = null, Exception? e = null) : Exception(text, e)
{
    public string Strategy { get; } = strategy ?? string.Empty;
    public string Identifier { get; } = identifier;
}

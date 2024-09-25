namespace NorthernSpectrums.Services.AudioProcessService
{
    /// <summary>
    /// <c>Exception</c> An invalid effects provider was referenced.
    /// </summary>
    public class InvalidEffectsProviderReferenceException : Exception
    {
        public InvalidEffectsProviderReferenceException() { }

        public InvalidEffectsProviderReferenceException(string message) : base(message) { }

        public InvalidEffectsProviderReferenceException(string message, Exception inner) :base(message, inner) { }
    }
}

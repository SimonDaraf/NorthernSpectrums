namespace NorthernSpectrums.MVVM.Model.Audio.DeviceManager
{
    /// <summary>
    /// <c>Exception</c> thrown if a driver mode is not supported.
    /// </summary>
    public class UnsupportedDriverModeException : Exception
    {
        public UnsupportedDriverModeException() { }

        public UnsupportedDriverModeException(string message) : base(message) { }

        public UnsupportedDriverModeException(string message, Exception inner) : base(message, inner) { }
    }
}

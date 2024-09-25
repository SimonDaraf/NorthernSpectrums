namespace NorthernSpectrums.MVVM.Model
{
    /// <summary>
    /// <c>Interface</c> Defines an instance as preservable.
    /// </summary>
    public interface IPreservable
    {
        /// <summary>
        /// <c>Method</c> Used to extract data to be saved.
        /// The value will have to be parsed as the relevant data type.
        /// This means that to effectively utilize this method prior knowledge on both structure and explicit types are necessary.
        /// </summary>
        /// <returns>A dictionary with a string key and object value.</returns>
        public Dictionary<string, object> Save();

        /// <summary>
        /// <c>Method</c> Used to load data into instance.
        /// The value will have to be parsed as the relevant data type.
        /// This means that to effectively utilize this method prior knowledge on both structure and explicit types are necessary.
        /// </summary>
        /// <param name="data">A dictionary with a string key and object value</param>
        public void Load(Dictionary<string, object> data);
    }
}

using System.Reflection;

namespace NorthernSpectrums.Core
{
    /// <summary>
    /// <c>Class</c> An enum extension method to extract a friendly name through the description attribute.
    /// Original implementation by AramT <see cref="https://www.codingame.com/playgrounds/2487/c---how-to-display-friendly-names-for-enumerations"/>
    /// </summary>
    public static class EnumExtensionMethod
    {
        /// <summary>
        /// <c>Method</c> Tries to accesses the description attribute from a generic enum.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="genericEnum">The enum.</param>
        /// <returns>The enum description as a string if available, else un-friendly name</returns>
        public static string GetDescription<T>(T genericEnum) where T : Enum
        {
            Type enumType = genericEnum.GetType();
            MemberInfo[] memberInfo = enumType.GetMember(genericEnum.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                // Try to access the description attribute.
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attributes.ElementAt(0)).Description;
                }
            }

            // If no description attribute could be found.
            return genericEnum.ToString();
        }
    }
}

namespace Survey.Presentaion.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// The Selected list item helper.
    /// </summary>
    public static class SelectedListItemHelper
    {
        #region public

        public static IEnumerable<SelectListItem> GetDropDownListFromEnum<T>(this T defaultValue)
        {
            Type enumType = typeof(T);
            IEnumerable<string> names = Enum.GetNames(enumType);
            IEnumerable<int> values = Enum.GetValues(enumType).Cast<int>();


            var items = names.Zip(values, (name, value) =>
                 new SelectListItem()
                 {
                     Value = value.ToString(),
                     Text = name.Replace("_", " "),
                     Selected =   (value == Convert.ToInt16(defaultValue))
                 });

            return items;

        }


        public static IEnumerable<SelectListItem> GetDropDownListFromEnum<T>()
        {
            Type enumType = typeof(T);
            IEnumerable<string> names = Enum.GetNames(enumType);
            IEnumerable<int> values = Enum.GetValues(enumType).Cast<int>();


            var items = names.Zip(values, (name, value) =>
                 new SelectListItem()
                 {
                     Value = value.ToString(),
                     Text = name.Replace("_", " ")                    
                 });

            return items;

        }

        #endregion public methods

        #region private methods


        #endregion private methods
    }
}
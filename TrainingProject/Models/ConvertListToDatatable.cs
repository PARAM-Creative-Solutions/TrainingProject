using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace TrainingProject.Models
{
    /// <summary>
    /// generic Method to Convert any type of object to datatable
    /// added by priyanka
    /// </summary>
    public class ConvertListToDatatable
    {
        /// <summary>
        /// added by priyanka gaharwal to convert list to datatable
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(object items)
        {
            List<T> itemslist = (List<T>)items;
            DataTable dataTable = new DataTable();

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in itemslist)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
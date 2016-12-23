using System;

namespace DdhpCore.Storage
{
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; }
    }
}
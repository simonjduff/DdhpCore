using System;
using System.Collections.Generic;
using LegacyDataImporter.Writers;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using AutoMapper;

namespace LegacyDataImporter.Importers
{
    public class Importer
    {
        private readonly CloudTableClient _tableClient;
        private readonly IMapper _mapper;

        public Importer(CloudTableClient tableClient, IMapper mapper)
        {
            _mapper = mapper;
            _tableClient = tableClient;
        }

        public void Import<TFrom, TTo>(string tableName, IEnumerable<TFrom> dbRoot)
            where TFrom : class 
            where TTo : ITableEntity, new()
        {
            Console.Write($"Importing {tableName}...");
            var table = _tableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            var writer = new TableWriter<TTo>(table);
            writer
                .ClearTable()
                .WriteData(dbRoot.Select(from => _mapper.Map<TFrom,TTo>(from)));

            Console.WriteLine("Done");
        }
    }
}
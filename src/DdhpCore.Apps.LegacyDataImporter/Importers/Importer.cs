using System;
using System.Collections.Generic;
using LegacyDataImporter.Writers;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using AutoMapper;

namespace LegacyDataImporter.Importers
{
    public class Importer<TFrom, TTo> where TFrom : class
            where TTo : ITableEntity, new()
    {
        private readonly CloudTableClient _tableClient;
        private readonly IMapper _mapper;
        private Func<IQueryable<TFrom>, IEnumerable<TTo>> _mapperFunc;
        private readonly string _tableName;

        public Importer(CloudTableClient tableClient, IMapper mapper, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Table Name cannot be null or whitespace");
            }

            _tableClient = tableClient;
            _mapper = mapper;
            _mapperFunc = (froms) => froms.Select(from => _mapper.Map<TFrom, TTo>(from));
            _tableName = tableName;
        }

        public Importer<TFrom, TTo> Mapper(Func<IQueryable<TFrom>, IEnumerable<TTo>> mapperFunc)
        {
            _mapperFunc = mapperFunc;
            return this;
        }

        public IEnumerable<TTo> Import(IQueryable<TFrom> dbRoot)
        {
            Console.Write($"Importing {_tableName}...");
            var table = _tableClient.GetTableReference(_tableName);
            table.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            var writer = new TableWriter<TTo>(table);
            var mappedData = _mapperFunc(dbRoot);
            writer
                .ClearTable()
                .WriteData(mappedData);

            Console.WriteLine("Done");

            return mappedData;
        }
    }

    public class ImporterFactory
    {
        private readonly IMapper _mapper;
        private readonly CloudTableClient _tableClient;

        public ImporterFactory(CloudTableClient tableClient, IMapper mapper)
        {
            _tableClient = tableClient;
            _mapper = mapper;
        }

        public Importer<TFrom, TTo> Importer<TFrom,TTo>(string tableName) where TFrom : class
            where TTo : ITableEntity, new()
        {
                return new Importer<TFrom, TTo>(_tableClient, _mapper, tableName);
        }
    }
}
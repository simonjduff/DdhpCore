using System.Collections.Generic;
using DdhpCore.FrontEnd.Models.Api.Read;
using Newtonsoft.Json;
using Xunit;

namespace MapperTests
{
    public class UnitTest1
    {
        [Fact]
        public void MapperTranslatesToApiObject()
        {
            // Given the storage json
            string input = @"[{'id':'65ca260e-2cac-44a4-a2bd-82e76207293d','clubName':'Bethnal Green Village','partitionKey':'ALL_CLUBS','rowKey':'65ca260e-2cac-44a4-a2bd-82e76207293d','timestamp':'2017-05-09T11:22:49.683+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.683Z'\''},{'id':'067a5714-dad6-49c9-8b78-7bb876e12af4','clubName':'Cheats','partitionKey':'ALL_CLUBS','rowKey':'067a5714-dad6-49c9-8b78-7bb876e12af4','timestamp':'2017-05-09T11:22:49.687+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.687Z'\''},{'id':'c2ddf696-2a4e-4b55-96c7-bab9ab424375','clubName':'Coolbellup Cazzu Grandes','partitionKey':'ALL_CLUBS','rowKey':'c2ddf696-2a4e-4b55-96c7-bab9ab424375','timestamp':'2017-05-09T11:22:49.687+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.687Z'\''},{'id':'b88d61fd-cffd-4231-aa04-7c5dccc07546','clubName':'Corio Bayside','partitionKey':'ALL_CLUBS','rowKey':'b88d61fd-cffd-4231-aa04-7c5dccc07546','timestamp':'2017-05-09T11:22:49.687+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.687Z'\''},{'id':'433eb5a2-2b63-4f39-8bc5-4a3f221e98a5','clubName':'Fabulous Phils','partitionKey':'ALL_CLUBS','rowKey':'433eb5a2-2b63-4f39-8bc5-4a3f221e98a5','timestamp':'2017-05-09T11:22:49.687+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.687Z'\''},{'id':'e03986c8-4c0c-42dc-85b9-969a95ce58cb','clubName':'Fitzroy','partitionKey':'ALL_CLUBS','rowKey':'e03986c8-4c0c-42dc-85b9-969a95ce58cb','timestamp':'2017-05-09T11:22:49.69+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.69Z'\''},{'id':'b85bc606-1b0b-43c4-b152-20ce3d39f66e','clubName':'North Suburban Boner','partitionKey':'ALL_CLUBS','rowKey':'b85bc606-1b0b-43c4-b152-20ce3d39f66e','timestamp':'2017-05-09T11:22:49.69+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.69Z'\''},{'id':'d9768274-c37b-4402-b967-e3422dd98593','clubName':'Pirates','partitionKey':'ALL_CLUBS','rowKey':'d9768274-c37b-4402-b967-e3422dd98593','timestamp':'2017-05-09T11:22:49.69+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.69Z'\''},{'id':'5e1fe4f3-5274-4520-8e9a-c7c76e1f8c41','clubName':'Santa's Little Helpers','partitionKey':'ALL_CLUBS','rowKey':'5e1fe4f3-5274-4520-8e9a-c7c76e1f8c41','timestamp':'2017-05-09T11:22:49.69+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.69Z'\''},{'id':'2bb7c721-ca94-4aac-bc57-ec0ff62481e7','clubName':'South East Suburban Flannie','partitionKey':'ALL_CLUBS','rowKey':'2bb7c721-ca94-4aac-bc57-ec0ff62481e7','timestamp':'2017-05-09T11:22:49.69+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.69Z'\''},{'id':'0e2f3ca1-b992-45c4-96a5-48f0d1903836','clubName':'Southside Insurgents','partitionKey':'ALL_CLUBS','rowKey':'0e2f3ca1-b992-45c4-96a5-48f0d1903836','timestamp':'2017-05-09T11:22:49.69+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.69Z'\''},{'id':'061dc5df-c9f5-4e78-be65-9f4c1067bc46','clubName':'Western Suburbs Walkabout','partitionKey':'ALL_CLUBS','rowKey':'061dc5df-c9f5-4e78-be65-9f4c1067bc46','timestamp':'2017-05-09T11:22:49.693+00:00','eTag':'W/\'datetime'2017-05-09T11%3A22%3A49.693Z'\''}]";
            IEnumerable<Club> clubs = JsonConvert.DeserializeObject<IEnumerable<Club>>(input);

            // When the mapper is run

        }
    }
}

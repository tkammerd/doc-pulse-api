//using LaGovCrosswalk.Core.Entities;
//using LaGovCrosswalk.Infrastructure.Shared.Utility;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Doc.Pulse.Tests.Functional._Root.Helpers
//{
//    internal static class HashAdaptorHelpers
//    {
//        public static string HashIdEncode<T>(this int id)
//        {
//            var _hashAdaptorOptions = Options.Create(new HashIdsAdaptorConfig() { Salt = "D'YaKnowWhatTheChainOfCommandIs", MinHashLength = 8 });

//            var hashAdaptor = new HashIdsAdaptor<T>(_hashAdaptorOptions);

//            return hashAdaptor.Encode(id);
//        }

//        public static string HashIdEncode<T>(this T entity) where T : BaseEntity<int>
//        {
//            var _hashAdaptorOptions = Options.Create(new HashIdsAdaptorConfig() { Salt = "D'YaKnowWhatTheChainOfCommandIs", MinHashLength = 8 });

//            var hashAdaptor = new HashIdsAdaptor<T>(_hashAdaptorOptions);

//            return hashAdaptor.Encode(entity.Id);
//        }
//    }
//}

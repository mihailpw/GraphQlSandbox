using System;
using System.Collections.Generic;

namespace GQL.Client.QueryBuilders.Infra
{
    public class RequiredArgumentNotAddedException : Exception
    {
        public RequiredArgumentNotAddedException(
            string builderName, 
            IEnumerable<string> notAddedArguments)
            : base(CreateMessage(builderName, notAddedArguments))
        {
        }


        private static string CreateMessage(string builderName, IEnumerable<string> notAddedArguments)
        {
            return $"Builder: {builderName}. Required field(s) not added: {string.Join(", ", notAddedArguments)}";
        }
    }
}
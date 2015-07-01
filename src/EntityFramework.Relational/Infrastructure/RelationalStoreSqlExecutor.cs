// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Query;
using Microsoft.Data.Entity.Storage;

namespace Microsoft.Data.Entity.Infrastructure
{
    public class RelationalStoreSqlExecutor
    {
        private ISqlStatementExecutor _statementExecutor;
        private IRelationalConnection _connection;
        private IRelationalTypeMapper _typeMapper;

        public RelationalStoreSqlExecutor(
            [NotNull] ISqlStatementExecutor statementExecutor,
            [NotNull] IRelationalConnection connection,
            [NotNull] IRelationalTypeMapper typeMapper)
        {
            _statementExecutor = statementExecutor;
            _connection = connection;
            _typeMapper = typeMapper;
        }

        public virtual void ExecuteStoreSql([NotNull] string sql, [NotNull] params object[] parameters)
        {
            var commandParameters = new CommandParameter[parameters.Length];
            var substitutions = new object[parameters.Length];

            for (var index = 0; index < parameters.Length; index++)
            {
                var parameterName = ParameterPrefix + "p" + index;

                var value = parameters[index];

                commandParameters[index] = new CommandParameter(parameterName, value, _typeMapper.GetDefaultMapping(value));

                substitutions[index] = parameterName;
            }

            _statementExecutor.ExecuteNonQuery(
                _connection,
                _connection.Transaction?.DbTransaction,
                new List<SqlBatch> {
                    new SqlBatch(
                        string.Format(sql, substitutions),
                        commandParameters)
                });
        }

        protected virtual string ParameterPrefix => "@";
    }
}

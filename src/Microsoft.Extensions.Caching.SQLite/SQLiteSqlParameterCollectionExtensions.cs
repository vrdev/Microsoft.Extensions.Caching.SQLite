// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Sqlite;
using System;
using System.Data;


namespace Microsoft.Extensions.Caching.SQLite
{
    // Since Mono currently does not have support for DateTimeOffset, we convert the time to UtcDateTime.
    // Even though the database column is of type 'datetimeoffset', we can store the UtcDateTime, in which case
    // the zone is set as 00:00. If you look at the below examples, DateTimeOffset.UtcNow
    // and DateTimeOffset.UtcDateTime are almost the same.
    //
    // Examples:
    // DateTimeOffset.Now:          6/29/2015 1:20:40 PM - 07:00
    // DateTimeOffset.UtcNow:       6/29/2015 8:20:40 PM + 00:00
    // DateTimeOffset.UtcDateTime:  6/29/2015 8:20:40 PM
    internal static class SQLiteSqlParameterCollectionExtensions
    {

        public static SqliteParameterCollection AddExpiresAtTimeSQLite(
            this SqliteParameterCollection parameters,
            DateTimeOffset utcTime)
        {
            return parameters.AddWithValue(Columns.Names.ExpiresAtTime, SqliteType.Integer, utcTime.ToUnixTimeMilliseconds());
        }


        public static SqliteParameterCollection AddAbsoluteExpirationSQLite(
                    this SqliteParameterCollection parameters,
                    DateTimeOffset? utcTime)
        {
            if (utcTime.HasValue)
            {
                return parameters.AddWithValue(
                    Columns.Names.AbsoluteExpiration, SqliteType.Integer, utcTime.Value.ToUnixTimeMilliseconds());
            }
            else
            {
                return parameters.AddWithValue(
                Columns.Names.AbsoluteExpiration, SqliteType.Integer, DBNull.Value);
            }
        }
    }
}

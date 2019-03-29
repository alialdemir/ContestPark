﻿using ContestPark.DataAccessLayer.Abctract;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ContestPark.DataAccessLayer.Dapper
{
    public class DatabaseConnection : Disposable
    {
        #region Constructor

        public DatabaseConnection(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        #endregion Constructor

        #region Properties

        private IDbConnection _connection;

        protected IDbConnection Connection
        {
            get
            {
                if (_connection.State != ConnectionState.Open && _connection.State != ConnectionState.Connecting)
                    _connection.Open();

                return _connection;
            }
        }

        #endregion Properties

        /// <summary>
        /// Close the connection if this is open
        /// </summary>
        protected override void DisposeCore()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                //   SqlConnection.ClearAllPools();
            }
            base.DisposeCore();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLawOffice.Data
{
    public class Transaction : IDisposable
    {
        private IDbConnection _conn;
        private bool _isStarted;
        private IDbTransaction _tran;
        private bool _closeConnectionOnDispose;

        public IDbConnection Connection { get { return _conn; } }

        public Transaction()
        {
            _conn = DataHelper.OpenIfNeeded(null);
        }

        public Transaction(IDbConnection conn)
        {
            _conn = DataHelper.OpenIfNeeded(conn);
        }

        public static Transaction Create(bool closeConnectionOnDispose = true)
        {
            return new Transaction().Start(closeConnectionOnDispose);
        }

        public Transaction Start(bool closeConnectionOnDispose = true)
        {
            _closeConnectionOnDispose = closeConnectionOnDispose;
            _isStarted = true;
            _tran = _conn.BeginTransaction();
            return this;
        }

        public void Commit()
        {
            _tran.Commit();
        }

        public void Rollback()
        {
            _tran.Rollback();
        }

        public void Dispose()
        {
            _tran.Dispose();
            if (_closeConnectionOnDispose)
                _conn.Close();
        }
    }
}

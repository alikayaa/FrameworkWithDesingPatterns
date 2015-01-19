using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheFramework
{
    public class CacheKey
    {
        #region Private fields

        private DateTime _expirationDate;//Biteceği olacağı zaman
        private bool _slidingExpiration;
        private TimeSpan _slidingExpirationWindowSize;
        private object _key;//key nesnesi

        #endregion

        #region Yapıcılar
        
        public CacheKey(object key, DateTime expirationDate)
        {
            _key = key;
            _slidingExpiration = false;
            _expirationDate = expirationDate;
        }
        
        public CacheKey(object key, TimeSpan slidingExpirationWindowSize)
        {
            _key = key;
            _slidingExpiration = true;
            _slidingExpirationWindowSize = slidingExpirationWindowSize;
            Accessed();
        }

        #endregion

        #region Properties

        
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
        }
        
        public object Key
        {
            get { return _key; }
        }
        
        public bool SlidingExpiration
        {
            get { return _slidingExpiration; }
        }
        
        public TimeSpan SlidingExpirationWindowSize
        {
            get { return _slidingExpirationWindowSize; }
        }

        #endregion


        public void Accessed()
        {
            if (_slidingExpiration)
            {
                _expirationDate = DateTime.Now.Add(_slidingExpirationWindowSize);
            }
        }
    }
}

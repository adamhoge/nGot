using System;

namespace nGot
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Gauge<T> where T : IComparable<T>
    {
        #region Member Variables
        
        private T m_value;
        private T m_min;
        private T m_max;
        
        #endregion

        #region Initialization

        public Gauge(T value, T min, T max)
        {
            m_value = value;
            m_min = min;
            m_max = max;

            nGot.Math.Clamp<T>(m_value, m_min, m_max);
        }

        #endregion

        #region Public Methods

        #endregion

        #region Accessors

        public T Min 
        { 
            get { return m_min; }
            set { m_min = value;  m_value = Math.Clamp<T>(m_value, m_min, m_max); }
        }

        #endregion
    }
}

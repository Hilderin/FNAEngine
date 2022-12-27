using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class AverageCalculator
    {
        /// <summary>
        /// Nb of numbers to keep in the average
        /// </summary>
        private int _bufferLength;

        /// <summary>
        /// Current index
        /// </summary>
        private int _index = 0;

        /// <summary>
        /// Nb of numbers in the buffer
        /// </summary>
        private int _cpt = 0;

        /// <summary>
        /// Buffer of data
        /// </summary>
        private decimal[] _buffer;

        /// <summary>
        /// Constructor
        /// </summary>
        public AverageCalculator(int bufferLength)
        {
            _bufferLength = bufferLength;
            _buffer = new decimal[bufferLength];
        }

        /// <summary>
        /// Add a number
        /// </summary>
        public void Add(int number)
        {
            Add((decimal)number);
        }

        /// <summary>
        /// Add a number
        /// </summary>
        public void Add(long number)
        {
            Add((decimal)number);
        }

        /// <summary>
        /// Add a number
        /// </summary>
        public void Add(double number)
        {
            Add((decimal)number);
        }

        /// <summary>
        /// Add a number
        /// </summary>
        public void Add(float number)
        {
            Add((decimal)number);
        }

        /// <summary>
        /// Add a number
        /// </summary>
        /// <param name="data"></param>
        public void Add(decimal number)
        {
            if (_cpt < _bufferLength)
            {
                //The buffer is not full...
                _buffer[_index++] = number;
                _cpt++;
            }
            else
            {
                //The buffer is full, check where we will put the next number...
                if (_index < _bufferLength)
                {
                    _buffer[_index++] = number;
                }
                else
                {
                    _index = 0;
                    _buffer[0] = number;
                }
            }
        }

        /// <summary>
        /// Get the average number
        /// </summary>
        public decimal GetAverage()
        {
            if (_cpt == 0)
                return 0;

            decimal total = 0;
            for (int index = 0; index < _cpt; index++)
                total += _buffer[index];
            return total / _cpt;

        }

    }
}

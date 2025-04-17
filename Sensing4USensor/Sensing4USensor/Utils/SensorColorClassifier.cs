using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensing4USensor.Models;

namespace Sensing4USensor.Utils
{
    /// <summary>
    /// Utility class for sensor color classification.
    /// </summary>
    public static class SensorColorClassifier
    {
        /// <summary>
        /// Returns a color category based on the value and given thresholds.
        /// </summary>
        /// <param name="value">The sensor value.</param>
        /// <param name="lower">Lower acceptable threshold.</param>
        /// <param name="upper">Upper acceptable threshold.</param>
        /// <returns>Low, Acceptable, or High category.</returns>
        public static ColorCategory GetColor(double value, double lower, double upper)
        {
            if (value < lower)
                return ColorCategory.Low;
            if (value > upper)
                return ColorCategory.High;
            return ColorCategory.Acceptable;
        }
    }
}
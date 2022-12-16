using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Content Manager
    /// </summary>
    public class ContentManager
    {

        /// <summary>
        /// Root du content folder
        /// </summary>
        private static string _contentFolder;

        /// <summary>
        /// Constructeur
        /// </summary>
        static ContentManager()
        {
            string assemblyLocation = Assembly.GetCallingAssembly().Location;
            if (assemblyLocation.IndexOf(@"bin\debug\", StringComparison.OrdinalIgnoreCase) > 0
                || assemblyLocation.IndexOf(@"bin\release\", StringComparison.OrdinalIgnoreCase) > 0)
                _contentFolder = @"..\..\Content\";
            else
                _contentFolder = @"Content\";
        }

        /// <summary>
        /// Permet de retourner le path du content folder
        /// </summary>
        public static string ContentFolder { get { return _contentFolder; } }



    }
}

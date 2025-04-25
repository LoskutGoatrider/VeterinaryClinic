using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Model;

namespace VeterinaryClinic.Services
{
        internal class Helper
        {
                public static VeterinaryСlinicEntities _context;
                public static VeterinaryСlinicEntities GetContext()
                {
                        if (_context == null)
                        {
                                _context = new VeterinaryСlinicEntities();
                        }
                        return _context;
                }
        }
}

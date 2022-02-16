using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingProjectDataLayer.DataLayer.Unit_of_Work.Interface
{
    interface IUnitofWork
    {
        int SaveChanges();
    }
}

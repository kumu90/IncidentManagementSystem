using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductDataAccess _iproductDataAccess;

        public ProductService(IProductDataAccess iproductDataAccess)
        {
            _iproductDataAccess = iproductDataAccess;
        }

        public SQLStatusDto AddService(ServiceDto service)
        {
            return _iproductDataAccess.AddService(service);
        }
      
        public List<ServiceDto> GetServices(string InstId = "")
        {
            return _iproductDataAccess.GetServices(InstId);
        }



        
    }

  
}

public interface IProductService
{
    SQLStatusDto AddService(ServiceDto service);
   

    List<ServiceDto> GetServices(string InstId = "");

   
    //SQLStatusDto AddService(object value1, object value2);

    //SQLStatusDto RegisterService(RegisterServiceDto _registerServiceDto);
}


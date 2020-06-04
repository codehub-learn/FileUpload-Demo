using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileUpload.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using FileUpload.Repository;
using FileUpload.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly MvcDbContext db;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment
            , MvcDbContext _db)
        {
            hostingEnvironment = environment;
            _logger = logger;
            db = _db;
        }

        [HttpGet("")]
        public IActionResult Home()
        {
            return View("Index");
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("Storage")]
        public IActionResult Storage()
        {
            return View();
        }


[HttpPost("GetCustomer")]
        public JsonResult GetCustomer([FromBody]Customer cust)
        {
            //gets only the orders without recursion of customers

            List<Order>
                orders = db.Set<Customer>()

                .Where(c => c.Id == cust.Id)
                .Select(c => c.Orders)
                .First()
                .ToList();
   
            return Json(orders);

            /*
             // the following does not return json but can be patched
                        Customer newCust = new Customer { Id=cust.Id };
                    Customer foundCust = db.Set<Customer>().Find(newCust.Id);
                   foundCust = db.Set<Customer>().Include(c => c.Orders).Where(c => c.Id == cust.Id).FirstOrDefault();
                   List<Order> orders = foundCust.Orders.ToList();
                      return Json(foundCust);
             */
        }


        [HttpPost("AddCustomer")]
        public Customer AddCustomer([FromBody]Customer cust)
        {
            Customer newCust = new Customer { Name=cust.Name };
            db.Add(newCust);
            db.SaveChanges();
            return newCust;
        }

        //cannot return Order since it has the Customer field
        [HttpPost("AddOrder")]
        public ReturnModel AddOrder([FromBody]OrderModel order)
        {
            Customer cust = db.Set<Customer>().Find(order.CustomerId);
            Order newOrd = new Order { Name= order.Name, Customer=cust  };
            db.Add(newOrd);
            db.SaveChanges();
            return new ReturnModel { ReturnValue = newOrd.Id } ;
        }




        //public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        //{
        //    long size = files.Sum(f => f.Length);

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var filePath = Path.GetTempFileName();

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    // Process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok(new { count = files.Count, size });
        //}

        [HttpGet("Order")]
        public ActionResult Order()
        {
            var orderVM = new OrderViewModel
            {
                Products = new List<ProductViewModel>
                    {
                        new ProductViewModel{ ID=1, Name="IPhone" },
                        new ProductViewModel{ ID=2, Name="MacBook Pro" },
                        new ProductViewModel{ ID=3, Name="iPod" }
                    },
                OrderNumber = 12,
                SelectedProductId = 2

            };
            return View(orderVM);
        }

[HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new CreatePost();
            return View(model);
        }

        [HttpPost("CreateUsingAjaX")]
        public ReturnModel CreateUsingAjaX( [FromForm]   CreatePost model )
        {

         // do other validations on your model as needed
            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   


                  return new ReturnModel { ReturnValue = 1 };
            }
            
            else return new ReturnModel { ReturnValue=-1 };
        }



        [HttpPost("CreateUsingAjaX1")]
        public ReturnModel CreateUsingAjaX1([FromBody]   CreatePost model)
        {

            // do other validations on your model as needed
            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   
                return new ReturnModel { ReturnValue = 1 };
            }

            else return new ReturnModel { ReturnValue = -1 };
        }


        [HttpPost("CreatePost")]
        public IActionResult Create( [FromForm]  CreatePost model)
        {
            // do other validations on your model as needed
            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   
            }
            // to do  : Return something
            return RedirectToAction("Index", "Home");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }



        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using ApplicationCore.ClientAddress;
using ApplicationCore.Clients;
using ApplicationCore.Core;
using Microsoft.AspNetCore.Mvc;

namespace ClientsWeb.Controllers
{
    public class ClientsController : Controller
    {
        private IClientContext _clientContext;
        private readonly IFileExporter _export;
        public ClientsController(IClientContext clientContext, IFileExporter export)
        {
            _clientContext = clientContext;
            _export = export;
        }

        public IActionResult Index()
        {
            return View(_clientContext.GetAll());
        }

        [HttpPost]
        public async Task<FileResult> Export()
        {
            return File(await _export.GetCsv(), "text/csv", "Data.csv");
        }
        public ActionResult Details(int id)
        {
            return View(_clientContext.GetById(id));
        }

        public ActionResult Create()
        {
            return View(_clientContext.GetById(0));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var client = GetClient(0, collection);
                client = _clientContext.Create(client);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            return View(_clientContext.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long id, IFormCollection collection)
        {
            try
            {
                var oldClient = GetClient(id, collection);
                _clientContext.Update(oldClient);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(long id)
        {
            var client = _clientContext.GetById(id);
            _clientContext.Delete(client);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Client GetClient(long id, IFormCollection collection)
        {
            var oldClient = _clientContext.GetById(id);
            oldClient.Title = Request.Form["Title"];
            oldClient.FirstName = Request.Form["FirstName"];
            oldClient.MiddleName = Request.Form["MiddleName"];
            oldClient.LastName = Request.Form["LastName"];
            oldClient.Gender = Convert.ToByte(Request.Form["Gender"]);
            var result = new List<Address>();
            for (int i = 0; i < Request.Form["item.id"].Count; i++)
            {
                var address = new Address
                {
                    Id = long.Parse(Request.Form["item.Id"][i]),
                    AddressLine1 = (string)Request.Form["item.AddressLine1"][i],
                    AddressLine2 = (string)Request.Form["item.AddressLine2"][i],
                    CellPhoneNumber = "",
                    ResidentialPhoneNumber = (string)Request.Form["item.ResidentialPhoneNumber"][i],
                    BusinessPhoneNumber = "",
                    Email = "",
                    City = (string)Request.Form["item.City"][i],
                    StateProvince = 0,
                    AddressTypeId = int.Parse(Request.Form["item.AddressTypeId"][i]),
                    PostalCode = "",
                    ModifiedDate = DateTime.UtcNow,
                    ClientId = oldClient.Id
                };
                result.Add(address);
            }
            oldClient.ClientAddresses = result;
            return oldClient;

        }
    }
}

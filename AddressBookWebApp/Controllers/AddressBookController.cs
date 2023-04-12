using AddressBookWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookWebApp.Controllers
{
    public class AddressBookController : Controller
    {
        private readonly AddressBookDataAccess _abda;

        public AddressBookController(AddressBookDataAccess abda)
        {
            _abda = abda;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<AddressBook> ab = new List<AddressBook>();
            try
            {
                ab = _abda.GetAllPersons();
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
            }
            return View(ab);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddressBook ab)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Object data is not valid!";
                }
                bool result = _abda.Insert(ab);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to insert person";
                    return View();
                }

                TempData["successMessage"] = "Address book details successfully inserted!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                AddressBook ab = _abda.GetPersonById(id);
                if (ab.AddressBookId == 0)
                {
                    TempData["errorMessage"] = $"No data found with an Id: {id}";
                    return RedirectToAction("Index");
                }
                return View(ab);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(AddressBook ab)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View();
                }
                bool result = _abda.Update(ab);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to update person data";
                    return View();
                }
                TempData["successMessage"] = "Person data has been updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                AddressBook ab = _abda.GetPersonById(id);
                if (ab.AddressBookId == 0)
                {
                    TempData["errorMessage"] = $"No data found with an Id: {id}";
                    return RedirectToAction("Index");
                }
                return View(ab);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(AddressBook ab)
        {
            try
            {
                bool result = _abda.Delete(ab.AddressBookId);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to delete person data";
                    return View();
                }
                TempData["successMessage"] = "Person data has been deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
    }
}

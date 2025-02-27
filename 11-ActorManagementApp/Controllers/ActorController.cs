using _11_ActorManagementApp.ProjectModels;
using _11_ActorManagementApp.ViewModels.ActorVM;
using ActorManagement.Models.EntityModels;
using ActorManagement.Services.Contracts.AllContracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace _11_ActorManagementApp.Controllers
{
    public class ActorController : Controller
    {
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileUpload _fileUpload;
        public ActorController(IUnitService unitService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this._unitService = unitService;
            this._mapper = mapper;
            this._webHostEnvironment = webHostEnvironment;
            this._fileUpload = new FileUpload(_webHostEnvironment.WebRootPath);
        }
        public async Task<IActionResult> Index(string text)
        {
            ViewBag.Message = text;
            IEnumerable<Actor> actors = await _unitService.ActorService.GetAllActorsAsync();
            IEnumerable<ActorIndexVM> actorsVM = _mapper.Map<IEnumerable<ActorIndexVM>>(actors);
            return View(actorsVM);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ActorCreateVM actorCreateVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Sorry! Invalid information, please enter valid information";
                return View(actorCreateVM);
            }

            //Check if another actor with the same email is already exists
            Actor? existingActor = await _unitService.ActorService.GetActorByEmailAsync(actorCreateVM.Email);
            if (existingActor != null)
            {
                ViewBag.Message = "An actor with the same email is already exists";
                return View(actorCreateVM);
            }

            //Ensure image file is provided
            if (actorCreateVM.ImageFile == null)
            {
                ViewBag.Message = "Please upload an actor image";
                return View(actorCreateVM);
            }
            else
            {
                try
                {
                    //Upload image
                    string folderName = "images-actors";
                    //Call ImageUploadAsync method to upload actor image
                    string imageName = await _fileUpload.ImageUploadAsync(actorCreateVM.ImageFile, folderName);
                    if (!string.IsNullOrEmpty(imageName))
                    {
                        actorCreateVM.ImageName = imageName;
                    }
                }
                catch (InvalidOperationException ex) //Catch only expected errors
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                    return View(actorCreateVM);
                }
                catch //Catch unexpected errors
                {
                    ViewBag.Message = "An unexpected error occurred. Please try again.";
                    return View(actorCreateVM);
                }

                //Map View model to domain model
                Actor actor = _mapper.Map<Actor>(actorCreateVM);

                //Save object to the repository
                await _unitService.ActorService.AddActorAsync(actor);
                bool isSaved = await _unitService.SaveChangesAsync();

                //Provide feedback
                TempData["Message"] = isSaved
                    ? "Information has been saved successfully, thanks."
                    : "Sorry! Information hasn't been saved.";

                return RedirectToAction("Index", "Actor");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Message"] = "Sorry! Actor email is not valid.";
                return RedirectToAction("Index", "Actor");
            }

            //Retrieve actor by email
            Actor? existingActor = await _unitService.ActorService.GetActorByEmailAsync(email);
            if (existingActor == null)
            {
                TempData["Message"] = $"Sorry! No Actor found with this email {email}";
                return RedirectToAction("Index", "Actor");
            }
            ActorEditVM actorEditVM = _mapper.Map<ActorEditVM>(existingActor);
            return View(actorEditVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ActorEditVM actorEditVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Sorry! Invalid information, please enter valid information.";
                return View(actorEditVM);
            }

            //Check if another actor with the same email already exists
            Actor? existingActor = await _unitService.ActorService.GetActorByEmailAsync(actorEditVM.Email);

            if (existingActor == null)
            {
                ViewBag.Message = $"Sorry! No Actor found with this email {actorEditVM.Email}";
                return View(actorEditVM);
            }
            if (existingActor?.ActorId != actorEditVM.ActorId)
            {
                ViewBag.Message = "Sorry! an Actor with the same email already exists";
                return View(actorEditVM);
            }

            //Maintain the existing file name
            actorEditVM.ImageName = existingActor.ImageName;

            //If a new image is uploaded, remove the old one
            if (actorEditVM.ImageFile != null)
            {
                try
                {
                    //Remove the old image from wwwroot/images-actors
                    if (!string.IsNullOrEmpty(existingActor.ImageName))
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string oldImagePath = Path.Combine(wwwRootPath, "images-actors", existingActor.ImageName);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //Upload new image
                    string folderName = "images-actors";
                    //Call ImageUploadAsync method to upload actor image
                    string imageName = await _fileUpload.ImageUploadAsync(actorEditVM.ImageFile, folderName);
                    if (!string.IsNullOrEmpty(imageName))
                    {
                        actorEditVM.ImageName = imageName;
                    }
                }
                catch (InvalidOperationException ex) //catch only expected errors
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                    return View(actorEditVM);
                }
                catch //catch unexpected errors
                {
                    ViewBag.Message = "An unexpected error occurred, please try again.";
                    return View(actorEditVM);
                }
            }

            //Map view model to domain model
            //existingActor.FirstName = actorEditVM.FirstName;
            //existingActor.LastName = actorEditVM.LastName;
            //existingActor.DateOfBirth = actorEditVM.DateOfBirth;
            //existingActor.Gender = actorEditVM.Gender;
            //existingActor.Email = actorEditVM.Email;
            //existingActor.IsActive = actorEditVM.IsActive;
            //existingActor.Address = actorEditVM.Address;

            _mapper.Map(actorEditVM, existingActor); //using automapper

            //Save changes to the database
            _unitService.ActorService.UpdateActor(existingActor);
            bool isUpdated = await _unitService.SaveChangesAsync();

            //Provide feedback
            TempData["Message"] = isUpdated
                ? "Information has been updated successfully, thanks."
                : "Sorry! Information hasn't been updated.";

            return RedirectToAction("Index", "Actor");
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Message"] = "Sorry! Actor email is not valid.";
                return RedirectToAction("Index", "Actor");
            }

            //Retrieve actor by email
            Actor? existingActor = await _unitService.ActorService.GetActorByEmailAsync(email);
            if (existingActor == null)
            {
                TempData["Message"] = $"Sorry! No Actor found with this email {email}";
                return RedirectToAction("Index", "Actor");
            }

            ActorDetailsVM actorDetailsVM = _mapper.Map<ActorDetailsVM>(existingActor);
            return View(actorDetailsVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Message"] = "❌ Sorry! No Actor email provided.";
                return RedirectToAction("Index", "Actor");
            }

            //Retrieve actor by email
            Actor? existingActor = await _unitService.ActorService.GetActorByEmailAsync(email);
            if (existingActor == null)
            {
                TempData["Message"] = "Sorry! No Actor found";
                return RedirectToAction("Index", "Actor");
            }

            try
            {
                //Delete image file if there is image associated
                if (!string.IsNullOrEmpty(existingActor.ImageName))
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string imagePath = Path.Combine(wwwRootPath, "images-actors", existingActor.ImageName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                //Remove actor from database
                _unitService.ActorService.RemoveActor(existingActor);
                bool isRemoved = await _unitService.SaveChangesAsync();

                if (isRemoved)
                {
                    TempData["Message"] = "Actor has been successfully deleted.";
                }
                else
                {
                    TempData["Message"] = "Sorry! this actor hasn't been deleted";
                }
            }
            catch (Exception ex)
            {
                // Log error (if logging is set up)
                TempData["Message"] = $"⚠️ Error: {ex.Message}";
            }
            return RedirectToAction("Index", "Actor");
        }
        [HttpGet]
        public async Task<IActionResult> DownloadBiography(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                TempData["Message"] = "Sorry! no biography found.";
                return RedirectToAction("Index", "Actor");
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(wwwRootPath, "description-bio", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                TempData["Message"] = "Sorry! no biograhy found";
                return RedirectToAction("Index", "Actor");
            }

            //Read the file content
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            //Return the file for download
            return File(memory, "application/pdf", fileName);
        }
        [HttpGet]
        public async Task<IActionResult> GetActorImagesPartial(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return PartialView("_ActorImagesPartial", new List<string>() { "No actor found" });
            }
            var actor = await _unitService.ActorService.GetActorByEmailAsync(email);
            List<string> imageName = new List<string>();

            if (actor?.Biography?.BiographyImages != null && actor.Biography.BiographyImages.Any())
            {
                foreach (var image in actor.Biography.BiographyImages)
                {
                    imageName.Add(image.ImageName);
                }
                return PartialView("_ActorImagesPartial", imageName);
            }
            return PartialView("_ActorImagesPartial", new List<string>() { "No images available" });
        }
    }
}

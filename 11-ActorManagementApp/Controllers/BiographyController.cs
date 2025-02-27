using _11_ActorManagementApp.ProjectModels;
using _11_ActorManagementApp.ViewModels.BiographyVM;
using ActorManagement.Models.EntityModels;
using ActorManagement.Services.Contracts.AllContracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace _11_ActorManagementApp.Controllers
{
    public class BiographyController : Controller
    {
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileUpload _fileUpload;
        public BiographyController(IUnitService unitService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this._unitService = unitService;
            this._mapper = mapper;
            this._webHostEnvironment = webHostEnvironment;
            this._fileUpload = new FileUpload(_webHostEnvironment.WebRootPath);
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        private async Task PopulateActors()
        {
            IEnumerable<Actor> actors = (await _unitService.ActorService.GetAllActorsAsync()).ToList();
            ViewBag.ActorIdType = new SelectList(actors, "ActorId", "FirstName");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //List<Actor> actors = (await _unitService.ActorService.GetAllActorsAsync()).ToList();
            //ViewBag.ActorIdType = new SelectList(actors, "ActorId", "FirstName");
            await PopulateActors();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(BiographyAddVM biographyAddVM)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Errors = errors; // Pass errors to the view
                ViewBag.Message = "Sorry! Invalid information, please enter valid information.";
                await PopulateActors();
                return View(biographyAddVM);
            }
            //Check if the actor already has a biography
            Biography? existBiography = await _unitService.BiographyService.GetBiographyByActorIdAsync(biographyAddVM.ActorId);
            if (existBiography != null)
            {
                ViewBag.Message = "This actor already has a biography, please update the existing biography.";
                await PopulateActors();
                return View(biographyAddVM);
            }
            //Ensure biography description file is provided
            if (biographyAddVM.DescriptionFile == null)
            {
                ViewBag.Message = "Please upload actor description.";
                await PopulateActors();
                return View(biographyAddVM);
            }
            //Ensure at least one image file is provided
            if (biographyAddVM.BioImages == null)
            {
                ViewBag.Message = "Please upload at least one actor image.";
                await PopulateActors();
                return View(biographyAddVM);
            }
            else
            {
                try
                {
                    //Upload description file(image or PDF)
                    string descriptionFolder = "description-bio";
                    //Call ImageOrPdfUploadAsync method to upload actor description
                    string descriptionName = await _fileUpload.ImageOrPdfUploadAsync(biographyAddVM.DescriptionFile, descriptionFolder);
                    if (!string.IsNullOrEmpty(descriptionName))
                    {
                        biographyAddVM.DescriptionFileName = descriptionName;
                    }

                    //Upload actor images(multiple)
                    string imageFolder = "images-bio";
                    //Call MultipleActorImagesAsync method to upload actor images
                    List<string> imageNames = await _fileUpload.MultipleImageUploadAsync(biographyAddVM.BioImages, imageFolder);
                    if (imageNames != null && imageNames.Any())
                    {
                        biographyAddVM.BiographyImages = imageNames
                            .Select(img => new BiographyImage()
                            {
                                ImageName = img
                            }).ToList();
                    }

                    //Save biography to the database
                    //Map viewmodel to domain model
                    Biography biography = _mapper.Map<Biography>(biographyAddVM);
                    await _unitService.BiographyService.AddBiographyAsync(biography);
                    bool isSaved = await _unitService.SaveChangesAsync();

                    //Provide feedback
                    TempData["Message"] = isSaved
                        ? "Biography information has been saved successfully, thanks"
                        : "Sorry! Information hasn't been saved";

                    return RedirectToAction("Index", "Actor");
                }
                catch (InvalidOperationException ex) //Catch only expected errors
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                    await PopulateActors();
                    return View(biographyAddVM);
                }
                catch //Catch unexpected errors
                {
                    ViewBag.Message = "An unexpected error occured. Please try again.";
                    await PopulateActors();
                    return View(biographyAddVM);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? biographyId)
        {

            if (biographyId == null)
            {
                ViewBag.Message = "Sorry! there is no biography found.";
                await PopulateActors();
                return View(new BiographyEditVM());
            }
            //Reterieve biography by id
            Biography? existBiography = await _unitService.BiographyService.GetBiographyByIdAsync(biographyId.Value);
            if (existBiography == null)
            {
                ViewBag.Message = "Sorry! there is no biography found.";
                await PopulateActors();
                return View(new BiographyEditVM());
            }
            await PopulateActors();
            BiographyEditVM editVM = _mapper.Map<BiographyEditVM>(existBiography);

            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BiographyEditVM biographyEditVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Sorry! Invalid information, please enter valid information.";
                await PopulateActors();
                return View(biographyEditVM);
            }

            //Check Biography exist or not
            Biography? existBiography = await _unitService.BiographyService.GetBiographyByIdAsync(biographyEditVM.BiographyId);

            if (existBiography == null)
            {
                ViewBag.Message = "Sorry! there is not biography found.";
                await PopulateActors();
                return View(biographyEditVM);
            }

            //Maintain the existing file name if no new file is uploaded
            biographyEditVM.DescriptionFileName = existBiography.DescriptionFileName;

            //if a new DescriptionFile is uploaded, remove the old one
            if (biographyEditVM.DescriptionFile != null)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string descriptionFolder = "description-bio";

                    //Remove the old descriptionfile from wwwroot/description-bio
                    if (!string.IsNullOrEmpty(existBiography.DescriptionFileName))
                    {
                        string oldFilePath = Path.Combine(wwwRootPath, descriptionFolder, existBiography.DescriptionFileName);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    //Upload new descriptioin file
                    string descriptionFileName = await _fileUpload.ImageOrPdfUploadAsync(biographyEditVM.DescriptionFile, descriptionFolder);

                    if (!string.IsNullOrEmpty(descriptionFileName))
                    {
                        biographyEditVM.DescriptionFileName = descriptionFileName;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                    await PopulateActors();
                    return View(biographyEditVM);
                }
                catch //catch unexpected errors
                {
                    ViewBag.Message = "An unexpected error occurred. Please try again.";
                    await PopulateActors();
                    return View(biographyEditVM);
                }
            }

            //Handle if new biography images(collection) uploaded and remove old images(collection)
            if (biographyEditVM.BioImages != null && biographyEditVM.BioImages.Any())
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string imageFolder = "images-bio";

                    //Remove old biography images if there are any images
                    if (existBiography.BiographyImages.Any())
                    {
                        foreach (var existingImage in existBiography.BiographyImages.ToList())
                        {
                            string oldImagePath = Path.Combine(wwwRootPath, imageFolder, existingImage.ImageName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                                //Asynchronous way file deletion
                                //await Task.Run(() => System.IO.File.Delete(oldImagePath));
                            }
                            // Remove image from the Biography collection and database
                            //existBiography.BiographyImages.Remove(existingImage);
                            _unitService.BiographyService.RemoveImageFromBiography(existingImage);
                        }
                    }

                    //Upload new images
                    List<string> newImageNames = await _fileUpload.MultipleImageUploadAsync(biographyEditVM.BioImages, imageFolder);

                    if (newImageNames.Any())
                    {
                        //Add new images to the biography image collection
                        foreach (var imageName in newImageNames)
                        {
                            var newBioImage = new BiographyImage()
                            {
                                BiographyId = existBiography.BiographyId,
                                ImageName = imageName
                            };
                            existBiography.BiographyImages.Add(newBioImage);
                            //existBiography.BiographyImages.Add(new BiographyImage()
                            //{
                            //    ImageName = imageName
                            //});
                        }
                        //using LINQ to add new images
                        //biographyEditVM.BiographyImages = newImageNames.Select(imageName => new BiographyImage()
                        //{
                        //    ImageName = imageName
                        //}).ToList();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                    await PopulateActors();
                    return View(biographyEditVM);
                }
                catch //Catch unexpected errors
                {
                    ViewBag.Message = "An unexpected error occurred. Please try again.";
                    await PopulateActors();
                    return View(biographyEditVM);
                }
            }

            //Map updated viewmodel to existing domain model
            _mapper.Map(biographyEditVM, existBiography);
            //existBiography.Title = biographyEditVM.Title;
            //existBiography.DescriptionFileName = biographyEditVM.DescriptionFileName;
            //existBiography.ActorId = biographyEditVM.ActorId;

            //Save changes to the database
            _unitService.BiographyService.UpdateBiography(existBiography);
            bool isUpdated = await _unitService.SaveChangesAsync();

            //Provide feedback
            TempData["Message"] = isUpdated
                ? "Biography has been updated successfully, thanks"
                : "Sorry! Biography hasn't been updated";

            return RedirectToAction("Index", "Actor");
        }
    }
}

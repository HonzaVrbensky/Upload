using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aspnet_upload.Pages;

public class Upload : PageModel
{
    private IWebHostEnvironment _environment;

    [TempData] public string SuccessMessage { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public ICollection<IFormFile> UploadFile { get; set; }

    public Upload(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        int successfulProcessing = 0;
        int failedProcessing = 0;
        foreach (var uploadedFile in UploadFile)
        {
            try
            {
                var file = Path.Combine(_environment.ContentRootPath, "Uploads", uploadedFile.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                ;
                successfulProcessing++;
            }
            catch
            {
                failedProcessing++;
            }

            if (failedProcessing == 0)
            {
                SuccessMessage = "All files has been uploaded successfuly.";
            }
            else
            {
                ErrorMessage = "There were <b>" + failedProcessing +
                               "</b> errors during uploading and processing of files.";
            }
        }

        return RedirectToPage("/Index");
    }
}

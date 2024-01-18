using System;
using Microsoft.AspNetCore.Mvc;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Components
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        private readonly ILanguageService _languageService;

        public LanguageSelectorViewComponent(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        public IViewComponentResult Invoke()
        {
            var model = new LanguageViewModel
            {
                Language = _languageService.SetCulture(HttpContext.Request.Query["language"])
            };

            return View(model);
        }
    }
}
using MengGrocery.DAL;
using MengGrocery.Models;
using Microsoft.AspNetCore.Mvc;
using System;

[Area("Confirmation")]

public class ConfirmationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }


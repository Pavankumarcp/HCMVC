using HCMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HCMVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;
        public string login2 = null;
        public AccountsController(IConfiguration configuration) { _configuration = configuration; }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            RolesRegisterVM model = new RolesRegisterVM
            {
                Values = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" }
                }
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RolesRegisterVM model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear(); client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    LoginModel user = new LoginModel
                    {
                        EmailId = model.RegisterRoles.EmailId,
                        Password = model.RegisterRoles.Password,
                        Role = model.SelectedValue
                    };
                    var result = await client.PostAsJsonAsync("Accounts/Register", user);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            LoginModel user1 = new LoginModel
            {
                EmailId = model.RegisterRoles.EmailId,
                Password = model.RegisterRoles.Password,
                Role = model.SelectedValue
            };
            RolesRegisterVM model1 = new RolesRegisterVM
            {
                RegisterRoles = user1,
                Values = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                }
            };
            return View(model1);
        }
        [HttpGet]
        public IActionResult RegisterDoctor()
        {
            RolesRegisterVM model1 = new RolesRegisterVM
            {
                Values = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Doctor", Text = "Doctor" }
                }
            };
            return View(model1);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterDoctor([FromForm] RolesRegisterVM model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear(); client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    LoginModel userdoctor = new LoginModel
                    {
                        EmailId = model.RegisterRoles.EmailId,
                        Password = model.RegisterRoles.Password,
                        Role = model.SelectedValue
                    };
                    var result = await client.PostAsJsonAsync("Accounts/Register", userdoctor);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            LoginModel user1 = new LoginModel
            {
                EmailId = model.RegisterRoles.EmailId,
                Password = model.RegisterRoles.Password,
                Role = model.SelectedValue
            };
            RolesRegisterVM model1 = new RolesRegisterVM
            {
                RegisterRoles = user1,
                Values = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Doctor", Text = "Doctor" },
                }
            };
            return View(model1);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                    var result = await client.PostAsJsonAsync("Accounts/Login", login);
                    if (result.IsSuccessStatusCode)
                    {

                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);

                        string role = await ExtractRole();
                        if (role == "Patient")
                        {

                            return RedirectToAction("PatientDetails", "Patient");
                        }
                        else if (role == "Admin")
                        {
                            return RedirectToAction("DoctorDetails", "Doctor");
                        }
                        else if (role == "Doctor")
                        {
                            return RedirectToAction("DoctorDetails", "Doctor");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    ModelState.AddModelError("", "Invalid LoginID or Password");
                }
            }
            return View(login);
        }
        [NonAction]
        public async Task<string> ExtractRole()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);        
                var roleResult = await client.GetAsync("Accounts/GetRole");
                if (roleResult.IsSuccessStatusCode)
                {                    
                    var role = await roleResult.Content.ReadAsAsync<string>();                   
                    return role;
                }
                return null;
            }
        }
        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Home");
        }
    }
}
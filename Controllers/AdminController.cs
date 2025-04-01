using Microsoft.AspNetCore.Mvc;
using Diploma.Models.ViewModels;
using Diploma.Services;
using Diploma.Entities;
using System.Web;
using Diploma.DTO;

namespace Diploma.Controllers;

public class AdminController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IRegisteredUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    public AdminController(IEmployeeService employeeService, IRegisteredUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _employeeService = _employeeService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }
    public IActionResult AdminPanel()
    {
        return View();
    }

    public IActionResult AddEmployee()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddEmployee(RegisterViewModel employeeViewModel)
    {
        if (ModelState.IsValid)
        {

            var result = await _employeeService.RegisterEmployee(employeeViewModel);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Потребителското име вече се използва!";
        }
        return View(employeeViewModel);
    }

    public IActionResult Employees()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> EditEmployee(string id)
    {
        RegisteredUser employee = _employeeService.Get(id);

        RegisterViewModel employeeViewModel = new RegisterViewModel
        {
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Password = employee.Password,
            PhoneNumber = employee.PhoneNumber,
            UserId = employee.Id,
            UserName = employee.UserName,
            Role = await _userService.GetUserRole(employee.Id)
        };

        return View(employeeViewModel);
    }
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        await _userService.DeleteAsync(id);
        return RedirectToAction("Users", "Admin");
    }
    [HttpPost]
    public async Task<IActionResult> EditEmployee(RegisterViewModel employeeViewModel)
    {
        await _employeeService.EditAsync(employeeViewModel);
        return RedirectToAction("Users", "Admin");
    }
    public IActionResult AllUsers()
    {
        return View();
    }
    public async Task<IActionResult> GetUser(int draw, int start, int length)
    {
        string urlQuery = _httpContextAccessor.HttpContext.Request.QueryString.Value;
        var paramsCollection = HttpUtility.ParseQueryString(urlQuery);

        //Get search params
        string? UserName = paramsCollection["columns[0][search][value]"];
        string? FirstName = paramsCollection["columns[1][search][value]"];
        string? LastName = paramsCollection["columns[2][search][value]"];
        string? Role = paramsCollection["columns[3][search][value]"];

        // string? defaultOrder = paramsCollection["columns[1][search][value]"];

        //Get sort
        string? sortColumnIndex = paramsCollection["order[0][column]"];
        string? sortColumnName = paramsCollection["columns[" + sortColumnIndex + "][data]"];
        string? sortDirection = paramsCollection["order[0][dir]"];
        string sortColumn = "";

        RegisteredUserSearch searchModel = new RegisteredUserSearch();
        searchModel.UserName = UserName;
        searchModel.FirstName = FirstName;
        searchModel.LastName = LastName;
        searchModel.Role = Role;

        if (sortDirection == "asc")
            sortColumn = sortColumnName;
        else
            sortColumn = $"-{sortColumnName}";

        SearchResult<RegisteredUserSearch> result = await _userService.Search(searchModel, sortColumn, start, length);

        return Ok(new
        {
            draw = draw,
            recordsTotal = result.RecordsTotal,
            recordsFiltered = result.RecordsFiltered,
            data = result.Data
        });
    }
    public async Task<IActionResult> ChangeRole(string id)
    {
        await _userService.ChangeRole(id);
        return RedirectToAction("AllUsers");
    }
}
﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazorTemplate.Data.Entities;
using MudBlazorTemplate.Extensions;
using MudBlazorTemplate.Models;
using MudBlazorTemplate.Shared;

namespace MudBlazorTemplate.Pages.UserManagement
{
    public class UsersBase : ComponentBase
    {
        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;

        [Inject]
        private IDialogService _dialogService { get; set; } = null!;

        [Inject]
        private UserManager<User> _userManager { get; set; } = null!;

        [CascadingParameter]
        private Task<AuthenticationState> _authStateTask { get; set; } = null!;

        protected bool IsLoading { get; set; }
        protected string? SearchQuery { get; set; }
        protected string CurrentUserId { get; set; } = null!;
        protected List<User> Users { get; set; } = new();

        private async Task LoadUsers()
        {
            IsLoading = true;
            Users = await _userManager.Users.ToListAsync();
            var authState = await _authStateTask;
            CurrentUserId = authState.User.GetId()!;
            IsLoading = false;
        }

        protected override async Task OnInitializedAsync()
            => await LoadUsers();

        protected bool Filter(User user) => FilterUsers(user, SearchQuery);

        protected bool FilterUsers(User user, string? searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return true;
            if (user.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (!string.IsNullOrWhiteSpace(user.FirstName)
                && user.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (!string.IsNullOrWhiteSpace(user.LastName)
                && user.LastName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        protected async Task CreateUser()
        {
            var result = await _dialogService.Show<CreateUser>("Create user").Result;

            if (!result.Cancelled)
            {
                var userData = (CreateUserDto)result.Data;
                var user = new User
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    UserName = userData.Email
                };

                var identityResult = await _userManager.CreateAsync(user, userData.Password);

                if (identityResult.Succeeded)
                {
                    _snackbar.Add(string.Format(Messages.SuccessfulCreationFormat, user.Email),
                        Severity.Success);
                    Users.Add(user);
                }
                else
                {
                    _snackbar.Add(
                        string.Join(Environment.NewLine,
                            identityResult.Errors.Select(e => e.Description)),
                        Severity.Error);
                }
            }
        }

        protected async Task AssignRoles(User user)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);

            var parameters = new DialogParameters();
            parameters.Add("UserRoles", currentRoles);
            parameters.Add("SelectMultipleRoles", true);

            var result = await _dialogService
                .Show<AssignRoles>("Assign Roles", parameters).Result;

            if (!result.Cancelled)
            {
                IdentityResult? identityResult = null;
                var newRoles = (IEnumerable<string>)result.Data;
                var rolesToRemove = currentRoles.Except(newRoles);
                var rolesToAdd = newRoles.Except(currentRoles);

                if (rolesToRemove.Any())
                {
                    identityResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!identityResult.Succeeded)
                    {
                        _snackbar.Add(string.Join(Environment.NewLine,
                            identityResult.Errors.Select(e => e.Description)), Severity.Error);
                        return;
                    }
                }
                if (rolesToAdd.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(user, newRoles);
                    if (!identityResult.Succeeded)
                    {
                        _snackbar.Add(string.Join(Environment.NewLine,
                            identityResult.Errors.Select(e => e.Description)), Severity.Error);
                        return;
                    }
                }
                if (identityResult != null)
                {
                    _snackbar.Add(string.Format(Messages.SuccessfulUpdateFormat, user.Email),
                        Severity.Success);
                }
            }
        }

        protected async Task BlockOrUnblockUser(User user)
        {
            var action = user.IsBlocked ? "unblock" : "block";

            user.IsBlocked = !user.IsBlocked;
            var identityResult = await _userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
            {
                _snackbar.Add($"User {user.Email} has been {action}ed.", Severity.Success);
            }
            else
            {
                _snackbar.Add(string.Join(Environment.NewLine,
                    identityResult.Errors.Select(e => e.Description)), Severity.Error);
            }
        }

        protected async Task DeleteUser(User user)
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", "Delete user");
            parameters.Add("Content", $"Are you sure you want to delete \"{user.Email}\"?");

            var result = await _dialogService.Show<DeleteConfirmation>("", parameters).Result;

            if (!result.Cancelled)
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (identityResult.Succeeded)
                {
                    _snackbar.Add(
                        string.Format(Messages.SuccessfulDeletionFormat, user.Email),
                        Severity.Success);
                    Users.Remove(user);
                }
                else
                {
                    _snackbar.Add(string.Join(Environment.NewLine,
                        identityResult.Errors.Select(e => e.Description)), Severity.Error);
                }
            }
        }
    }
}

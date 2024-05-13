using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using WebDating.DTOs;
using WebDating.Entities.MessageEntities;
using WebDating.Extensions;
using WebDating.Interfaces;

namespace WebDating.SignalR
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IUnitOfWork _uow;
        public NotificationHub(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}

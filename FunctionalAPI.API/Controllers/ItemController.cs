using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunctionalAPI.Business;
using FunctionalAPI.Data;
using FunctionalAPI.Core;
using FunctionalAPI.Domain;
using SuccincT.Functional;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;
using FunctionalAPI.Domain.Commands;
using FunctionalAPI.Commands;

namespace FunctionalAPI.API.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly IManageItemState _stateService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IManageItemState stateService, ILogger<ItemController> logger)
        {
            _stateService = stateService;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetItem(int id) =>
            // Get the item by id and return
            _stateService.GetItemById(id).Into(ToResponse);

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateItem(Item updatedItem)
        {
            // Prepare required data
            var updateTime = DateTime.Now;
            // This is the func for getting the data
            ItemResult g(int i) => _stateService.GetItemById(i);
            // This is the func for saving the data
            ItemResult s(Item i) => _stateService.UpdateItem(i);
            
            return
            // Validate the incoming state
            ItemFunctions.ValidateItem(updatedItem)
            // Create and execute the command based 
            .IfSuccess(_ => ItemCommandExecutors.Execute(
                new UpdateItemCommand(
                    () => g(updatedItem.Id), s,
                    updatedItem.Data,
                    updatedItem.ModifiedAt)))
            // Handle the result of the command
            .IfSuccess(updatedItem => _stateService.UpdateItem(updatedItem))
            // Return the appropriate response
            .Into(ToResponse);
        }        
        
        [HttpPost]
        public IActionResult CreateItem(Item item)
        {
            var creationTime = DateTime.Now;

            return
               // Make sure the updateItem is valid
               ItemFunctions.ValidateItem(item, creationTime)
               // Save the state
               .IfSuccess(item => _stateService.CreateItem(item))
               // Return the appropriate response
               .Into(item => ToResponse(item, 201));
        }

        protected IActionResult ToResponse(ItemResult item) => ToResponse(item, 200);
        protected IActionResult ToResponse(ItemResult item, int successCode)
        {
            static bool IsSuccessCode(int code) => (code >= 200 && code < 300);

            return item switch
            {
                // Success code as defined
                var r when r.HasValue && IsSuccessCode(successCode) => StatusCode(successCode, r.Value),
                // 400 BadRequest
                var e when e.Error is BusinessInvalidItemError => BadRequest(e.Error.ErrorMessage),
                var e when e.Error is BusinessInvalidModificationDateError => BadRequest(e.Error.ErrorMessage),
                // 404 NotFound
                var e when e.Error is RepositoryNotFoundError => NotFound(e.Error.ErrorMessage),
                // 409 Conflict
                var e when e.Error is RepositoryOptimisticConcurrencyError => Conflict(e.Error.ErrorMessage),
                _ => StatusCode(500)
            };
        }
    }
}

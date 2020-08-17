using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunctionalAPI.Business;
using FunctionalAPI.Data;
using FunctionalAPI.Core;
using FunctionalAPI.Domain;
using SuccincT.Functional;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;


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
            var updateTime = DateTime.Now;

            return
                // Get the item we want to update
                _stateService.GetItemById(updatedItem.Id)
                // Make sure the updateItem is valid
                .IfSuccess(item => ItemFunctions.ValidateItem(updatedItem, updateTime))
                // Update the item using business rules
                .IfSuccess(item => ItemFunctions.UpdateItem(updatedItem.Data, updateTime, item))
                // Save the state
                .IfSuccess(item => _stateService.UpdateItem(item))
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

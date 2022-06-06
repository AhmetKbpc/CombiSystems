using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Iyzipay.Model;
using CombiSystems.Business.Services.Payment;
using CombiSystems.Business.Repositories.Abstracts;
using CombiSystems.Core.Entities;
using CombiSystems.Core.Payments;

namespace CombiSystems.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IRepository<Product, int> _productRepo;

        public PaymentController(IPaymentService paymentService, IRepository<Product, int> productRepo)
        {
            _paymentService = paymentService;
            _productRepo = productRepo;
        }

        [Route("guvenli-odeme")]
        public IActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        [Route("taksit-kontrol")]
        public IActionResult CheckInstallments(string cardNumber, decimal price)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return BadRequest();
            var result = _paymentService.CheckInstallments(cardNumber, price);
            return Ok(result);
        }
        [HttpPost]
        [Route("odeme-yap")]
        public IActionResult Pay(PaymentViewModel model)
        {
            //card validation
            //if (string.IsNullOrEmpty(model.CardNumber))
            //    return BadRequest();
            var basketModel = new List<BasketModel>();
            var total = 0m;
            foreach (var item in model.Carts)
            {
                var found = _productRepo.GetById(item.ProductId);
                if (found != null)
                {
                    basketModel.Add(new BasketModel()
                    {
                        Name = found.ProductName,
                        Category1 = found.Category?.CategoryName,
                        Id = found.ProductId.ToString(),
                        ItemType = "PHYSICAL",  //"VIRTUAL" "PHYSICAL",
                        Price = found.UnitPrice?.ToString(new CultureInfo("en-us")),
                    });
                    total += found.UnitPrice.GetValueOrDefault() * item.Count;
                }
            }

            var checkInstallmentsResult = _paymentService.CheckInstallments(model.CardModel.CardNumber, total);

            var paid = checkInstallmentsResult.InstallmentPrices
                .FirstOrDefault(x => x.InstallmentNumber == model.Installment, new InstallmentPriceModel()
                {
                    TotalPrice = total.ToString(new CultureInfo("en-us")),
                    InstallmentNumber = 1,
                    Price = total.ToString(new CultureInfo("en-us"))
                });

            var paymentModel = new PaymentModel()
            {
                CardModel = model.CardModel,
                Address = new AddressModel()
                {
                    Id = "1",
                    City = "İstanbul",
                    ContactName = "Kamil",
                    Country = "Turkiye",
                    ZipCode = "34152",
                    Description = "Caminin yanında"
                },
                Customer = new CustomerModel()
                {
                    Id = "1",
                    Email = "kamil@kamil.com",
                    GsmNumber = "15151515151",
                    IdentityNumber = "12345678911",
                    LastLoginDate = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    RegistrationDate = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                    City = "İstanbul",
                    Country = "Turkiye",
                    ZipCode = "34152",
                    Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Name = "Kamil",
                    Surname = "Fıdıl",
                    RegistrationAddress = "Beşiktaş istanbul"
                },
                Installment = model.Installment,
                Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString(),
                BasketList = basketModel,
                Price = total,
                //PaidPrice = paid.TotalPrice
            };

            var result = _paymentService.Pay(paymentModel);
            return Ok(result);

            return Ok();
        }
    }
}
